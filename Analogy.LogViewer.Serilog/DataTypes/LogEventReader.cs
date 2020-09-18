using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog.Events;
using Serilog.Parsing;

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

        int _lineNumber;

        /// <summary>
        /// Construct a <see cref="LogEventReader"/>.
        /// </summary>
        /// <param name="text">Text to read from.</param>
        /// <param name="serializer">If specified, a JSON serializer used when converting event documents.</param>
        public LogEventReader(TextReader text, JsonSerializer serializer = null)
        {
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
        /// <param name="evt"></param>
        /// <returns>True if an event could be read; false if the end-of-file was encountered.</returns>
        /// <exception cref="InvalidDataException">The data format is invalid.</exception>
        public bool TryRead(out LogEvent evt)
        {
            var line = _text.ReadLine();
            _lineNumber++;
            while (string.IsNullOrWhiteSpace(line))
            {
                if (line == null)
                {
                    evt = null;
                    return false;
                }
                line = _text.ReadLine();
                _lineNumber++;
            }

            var data = _serializer.Deserialize(new JsonTextReader(new StringReader(line)));
            var fields = data as JObject;
            if (fields == null)
                throw new InvalidDataException($"The data on line {_lineNumber} is not a complete JSON object.");

            evt = ReadFromJObject(_lineNumber, fields);
            return true;
        }

        /// <summary>
        /// Read a single log event from a JSON-encoded document.
        /// </summary>
        /// <param name="document">The event in compact-JSON.</param>
        /// <param name="serializer">If specified, a JSON serializer used when converting event documents.</param>
        /// <returns>The log event.</returns>
        public static LogEvent ReadFromString(string document, JsonSerializer serializer = null)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));

            serializer = serializer ?? CreateSerializer();
            var jObject = serializer.Deserialize<JObject>(new JsonTextReader(new StringReader(document)));
            return ReadFromJObject(jObject);

        }

        /// <summary>
        /// Read a single log event from an already-deserialized JSON object.
        /// </summary>
        /// <param name="jObject">The deserialized compact-JSON event.</param>
        /// <returns>The log event.</returns>
        public static LogEvent ReadFromJObject(JObject jObject)
        {
            if (jObject == null) throw new ArgumentNullException(nameof(jObject));
            return ReadFromJObject(1, jObject);
        }

        static LogEvent ReadFromJObject(int lineNumber, JObject jObject)
        {
            var timestamp = GetRequiredTimestampField(lineNumber, jObject, ClefFields.Timestamp);

            string messageTemplate;
            if (TryGetOptionalField(lineNumber, jObject, ClefFields.MessageTemplate, out var mt))
                messageTemplate = mt;
            else if (TryGetOptionalField(lineNumber, jObject, ClefFields.Message, out var m))
                messageTemplate = MessageTemplateSyntax.Escape(m);
            else
                messageTemplate = null;

            var level = LogEventLevel.Information;
            if (TryGetOptionalField(lineNumber, jObject, ClefFields.Level, out string l))
                level = (LogEventLevel)Enum.Parse(typeof(LogEventLevel), l);
            Exception exception = null;
            if (TryGetOptionalField(lineNumber, jObject, ClefFields.Exception, out string ex))
            {
                exception = TryPopulateException(ex, exception, jObject);
            }

            var parsedTemplate = messageTemplate == null ?
                new MessageTemplate(Enumerable.Empty<MessageTemplateToken>()) :
                Parser.Parse(messageTemplate);

            var renderings = Enumerable.Empty<Rendering>();

            if (jObject.TryGetValue(ClefFields.Renderings, out JToken r))
            {
                var renderedByIndex = r as JArray;
                if (renderedByIndex == null)
                    throw new InvalidDataException($"The `{ClefFields.Renderings}` value on line {lineNumber} is not an array as expected.");

                renderings = parsedTemplate.Tokens
                    .OfType<PropertyToken>()
                    .Where(t => t.Format != null)
                    .Zip(renderedByIndex, (t, rd) => new Rendering(t.PropertyName, t.Format, rd.Value<string>()))
                    .ToArray();
            }

            var properties = jObject
                .Properties()
                .Where(f => !ClefFields.All.Contains(f.Name))
                .Select(f =>
                {
                    var name = ClefFields.Unescape(f.Name);
                    var renderingsByFormat = renderings.Where(rd => rd.Name == name);
                    return PropertyFactory.CreateProperty(name, f.Value, renderingsByFormat);
                })
                .ToList();

            string eventId;
            if (TryGetOptionalField(lineNumber, jObject, ClefFields.EventId, out eventId)) // TODO; should support numeric ids.
            {
                properties.Add(new LogEventProperty("@i", new ScalarValue(eventId)));
            }

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

        static bool TryGetOptionalField(int lineNumber, JObject data, string field, out string value)
        {
            JToken token;
            if (!data.TryGetValue(field, out token) || token.Type == JTokenType.Null)
            {
                value = null;
                return false;
            }

            if (token.Type != JTokenType.String)
                throw new InvalidDataException($"The value of `{field}` on line {lineNumber} is not in a supported format.");

            value = token.Value<string>();
            return true;
        }

        static DateTimeOffset GetRequiredTimestampField(int lineNumber, JObject data, string field)
        {
            JToken token;
            if (!data.TryGetValue(field, out token) || token.Type == JTokenType.Null)
                throw new InvalidDataException($"The data on line {lineNumber} does not include the required `{field}` field.");

            if (token.Type == JTokenType.Date)
            {
                var dt = token.Value<JValue>().Value;
                if (dt is DateTimeOffset)
                    return (DateTimeOffset)dt;

                return (DateTime)dt;
            }

            if (token.Type != JTokenType.String)
                throw new InvalidDataException($"The value of `{field}` on line {lineNumber} is not in a supported format.");

            var text = token.Value<string>();
            return DateTimeOffset.Parse(text);
        }

        static JsonSerializer CreateSerializer()
        {
            return JsonSerializer.Create(new JsonSerializerSettings
            {
                DateParseHandling = DateParseHandling.None,
                Culture = CultureInfo.InvariantCulture
            });
        }
    }
}
