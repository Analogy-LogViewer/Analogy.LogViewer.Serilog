using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog.Events;
using Serilog.Parsing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Analogy.LogViewer.Serilog.DataTypes
{
    /// <summary>
    /// Reads files produced by <em>Serilog.Formatting.Compact.CompactJsonFormatter</em>. Events
    /// are expected to be encoded as newline-separated JSON documents.
    /// </summary>
    public class LogEventReader : IDisposable
    {
        static readonly MessageTemplateParser Parser = new MessageTemplateParser();
        readonly TextReader _text;
        readonly JsonSerializer _serializer;
        private readonly IMessageFields _messageFields;
        int _lineNumber;

        /// <summary>
        /// Construct a <see cref="LogEventReader"/>.
        /// </summary>
        /// <param name="text">Text to read from.</param>
        /// <param name="messageFields"></param>
        /// <param name="serializer">If specified, a JSON serializer used when converting event documents.</param>
        public LogEventReader(TextReader text, IMessageFields messageFields, JsonSerializer serializer = null)
        {
            _messageFields = messageFields;
            _text = text ?? throw new ArgumentNullException(nameof(text));
            _serializer = serializer ?? CreateSerializer();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _text.Dispose();
        }

        /// <summary>
        /// Read a line from the input. Blank lines are skipped.
        /// </summary>
        /// <param name="result"></param>
        /// <returns>True if an event could be read; false if the end-of-file was encountered.</returns>
        /// <exception cref="InvalidDataException">The data format is invalid.</exception>
        public bool TryRead(out ParsingResult result)
        {
            var line = _text.ReadLine();
            _lineNumber++;
            while (string.IsNullOrWhiteSpace(line))
            {
                if (line == null)
                {
                    result = new ParsingResult(null, string.Empty);
                    return false;
                }
                line = _text.ReadLine();
                _lineNumber++;
            }

            var data = _serializer.Deserialize(new JsonTextReader(new StringReader(line)));
            if (!(data is JObject fields))
            {
                throw new InvalidDataException($"The data on line {_lineNumber} is not a complete JSON object.");
            }

            result = new ParsingResult(ReadFromJObject(_lineNumber, fields, _messageFields), line);
            return true;
        }

        /// <summary>
        /// Read a single log event from a JSON-encoded document.
        /// </summary>
        /// <param name="document">The event in compact-JSON.</param>
        /// <param name="serializer">If specified, a JSON serializer used when converting event documents.</param>
        /// <returns>The log event.</returns>
        public LogEvent ReadFromString(string document, JsonSerializer serializer = null)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            serializer = serializer ?? CreateSerializer();
            var jObject = serializer.Deserialize<JObject>(new JsonTextReader(new StringReader(document)));
            return ReadFromJObject(jObject, _messageFields);

        }

        /// <summary>
        /// Read a single log event from an already-deserialized JSON object.
        /// </summary>
        /// <param name="jObject">The deserialized compact-JSON event.</param>
        /// <returns>The log event.</returns>
        public static LogEvent ReadFromJObject(JObject jObject, IMessageFields messageFields)
        {
            if (jObject == null)
            {
                throw new ArgumentNullException(nameof(jObject));
            }

            return ReadFromJObject(1, jObject, messageFields);
        }

        private static LogEvent ReadFromJObject(int lineNumber, JObject jObject, IMessageFields messageFields)
        {
            var timestamp = GetRequiredTimestampField(lineNumber, jObject, messageFields.Timestamp);

            string messageTemplate;
            if (TryGetOptionalField(lineNumber, jObject, messageFields.MessageTemplate, out var mt))
            {
                messageTemplate = mt;
            }
            else if (TryGetOptionalField(lineNumber, jObject, messageFields.Message, out var m))
            {
                messageTemplate = MessageTemplateSyntax.Escape(m);
            }
            else
            {
                messageTemplate = null;
            }

            var level = LogEventLevel.Information;
            if (TryGetOptionalField(lineNumber, jObject, messageFields.Level, out string l))
            {
                level = (LogEventLevel)Enum.Parse(typeof(LogEventLevel), l);
            }

            Exception exception = null;
            if (TryGetOptionalField(lineNumber, jObject, messageFields.Exception, out string ex))
            {
                exception = TryPopulateException(ex, exception, jObject);
            }

            var parsedTemplate = messageTemplate == null
                ? new MessageTemplate(Enumerable.Empty<MessageTemplateToken>())
                : Parser.Parse(messageTemplate);

            var renderings = Enumerable.Empty<Rendering>();

            if (jObject.TryGetValue(messageFields.Renderings, out JToken r))
            {
                var renderedByIndex = r as JArray;
                if (renderedByIndex == null)
                {
                    throw new InvalidDataException(
                        $"The `{messageFields.Renderings}` value on line {lineNumber} is not an array as expected.");
                }

                renderings = parsedTemplate.Tokens
                    .OfType<PropertyToken>()
                    .Where(t => t.Format != null)
                    .Zip(renderedByIndex, (t, rd) => new Rendering(t.PropertyName, t.Format, rd.Value<string>()))
                    .ToArray();
            }

            List<LogEventProperty> properties;
            if (jObject.ContainsKey("Properties") && jObject["Properties"] is JObject props)
            {
                properties = props.Properties()
                    .Where(f => !messageFields.All.Contains(f.Name))
                    .Select(f =>
                    {
                        var name = messageFields.Unescape(f.Name);
                        var renderingsByFormat = renderings.Where(rd => rd.Name == name).ToList();
                        return PropertyFactory.CreateProperty(name, f.Value, renderingsByFormat);
                    })
                    .ToList();
            }
            else
            {
                properties = jObject
                        .Properties()
                        .Where(f => !messageFields.All.Contains(f.Name))
                        .Select(f =>
                        {
                            var name = messageFields.Unescape(f.Name);
                            var renderingsByFormat = renderings.Where(rd => rd.Name == name).ToList();
                            return PropertyFactory.CreateProperty(name, f.Value, renderingsByFormat);
                        })
                        .ToList();
            }
            if (TryGetOptionalField(lineNumber, jObject, messageFields.EventId, out var eventId)) // TODO; should support numeric ids.
            {
                properties.Add(new LogEventProperty("@i", new ScalarValue(eventId)));
            }
            properties.Add(new LogEventProperty("Raw Data", new ScalarValue(jObject.ToString())));
            return new LogEvent(timestamp, level, exception, parsedTemplate, properties);
        }

        private static Exception TryPopulateException(string header, Exception exception, JObject data)
        {
            if (data.TryGetValue("ExceptionDetail", out var info))
            {
                var ex = new ExternalException(string.Join(" ", header, info["Message"]?.Value<string>()),
                    (info["HResult"].HasValues ? info["HResult"].Value<int>() : -1))
                {
                    Source = info["Source"]?.Value<string>()
                };
                return ex;
            }
            return new TextException(header);
        }

        private static bool TryGetOptionalField(int lineNumber, JObject data, string field, out string value)
        {
            JToken token;
            if (!data.TryGetValue(field, out token) || token.Type == JTokenType.Null)
            {
                value = null;
                return false;
            }

            if (token.Type != JTokenType.String)
            {
                throw new InvalidDataException($"The value of `{field}` on line {lineNumber} is not in a supported format.");
            }

            value = token.Value<string>();
            return true;
        }

        private static DateTimeOffset GetRequiredTimestampField(int lineNumber, JObject data, string field)
        {
            if (!data.TryGetValue(field, out var token) || token.Type == JTokenType.Null)
            {
                throw new InvalidDataException($"The data on line {lineNumber} does not include the required `{field}` field.");
            }

            if (token.Type == JTokenType.Date)
            {
                var dt = token.Value<JValue>().Value;
                if (dt is DateTimeOffset offset)
                {
                    return offset;
                }

                return (DateTime)dt;
            }

            if (token.Type != JTokenType.String)
            {
                throw new InvalidDataException($"The value of `{field}` on line {lineNumber} is not in a supported format.");
            }

            var text = token.Value<string>();
            return DateTimeOffset.Parse(text);
        }

        private static JsonSerializer CreateSerializer()
        {
            return JsonSerializer.Create(new JsonSerializerSettings
            {
                DateParseHandling = DateParseHandling.None,
                Culture = CultureInfo.InvariantCulture
            });
        }
    }
}
