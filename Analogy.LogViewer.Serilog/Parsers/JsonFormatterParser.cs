using Analogy.Interfaces;
using Analogy.LogViewer.Serilog.DataTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Analogy.Interfaces.DataTypes;

namespace Analogy.LogViewer.Serilog
{
    public class JsonFormatterParser
    {
        private IMessageFields messageFields;
        private JsonSerializerSettings JsonSerializerSettings;

        public JsonFormatterParser(IMessageFields messageFields)
        {
            this.messageFields = messageFields;
            JsonSerializerSettings = new JsonSerializerSettings
            {
                DateParseHandling = DateParseHandling.DateTimeOffset,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            };

        }
        public async Task<IEnumerable<IAnalogyLogMessage>> Process(string fileName, CancellationToken token,
            ILogMessageCreatedHandler messagesHandler)
        {
            //var formatter = new JsonFormatter();
            List<IAnalogyLogMessage> parsedMessages = new List<IAnalogyLogMessage>();
            try
            {
                using (var analogy = new LoggerConfiguration()
                    .MinimumLevel.Verbose()
                    .WriteTo.Analogy()
                    .CreateLogger())
                {
                    using (var fileStream =
                        new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {

                        if (fileName.EndsWith(".gz", StringComparison.InvariantCultureIgnoreCase))
                        {
                            using (var gzStream = new GZipStream(fileStream, CompressionMode.Decompress))
                            {
                                using (var streamReader = new StreamReader(gzStream, encoding: Encoding.UTF8))
                                {
                                    string json;
                                    long count = 0;
                                    while ((json = await streamReader.ReadLineAsync()) != null)
                                    {

                                        var data = JsonConvert.DeserializeObject(json, JsonSerializerSettings);
                                        var jo = data as JObject;
                                        var evt = LogEventReader.ReadFromJObject(jo, messageFields);
                                        {
                                            analogy.Write(evt);
                                            AnalogyLogMessage m = CommonParser.ParseLogEventProperties(evt);
                                            m.RawText = jo.ToString(Formatting.None);
                                            m.RawTextType = AnalogyRowTextType.JSON;
                                            parsedMessages.Add(m);
                                            messagesHandler.ReportFileReadProgress(new AnalogyFileReadProgress(AnalogyFileReadProgressType.Incremental, 1, count, count));

                                        }
                                    }

                                }
                            }
                        }


                        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                        {
                            string json;
                            long count = 0;
                            while ((json = await streamReader.ReadLineAsync()) != null)
                            {
                                var data = JsonConvert.DeserializeObject(json, JsonSerializerSettings);
                                var jo = data as JObject;
                                var evt = LogEventReader.ReadFromJObject(jo, messageFields);
                                {
                                    analogy.Write(evt);
                                    AnalogyLogMessage m = CommonParser.ParseLogEventProperties(evt);
                                    m.RawText = jo.ToString(Formatting.None);
                                    m.RawTextType = AnalogyRowTextType.JSON;
                                    parsedMessages.Add(m);
                                    count++;
                                    messagesHandler.ReportFileReadProgress(new AnalogyFileReadProgress(AnalogyFileReadProgressType.Incremental, 1, count, count));

                                }
                            }
                        }

                    }

                    messagesHandler.AppendMessages(parsedMessages, fileName);
                    return parsedMessages;
                }
            }
            catch (Exception e)
            {
                AnalogyLogMessage empty = new AnalogyLogMessage($"Error reading file {fileName}: Error: {e.Message}",
                    AnalogyLogLevel.Error, AnalogyLogClass.General, "Analogy", "None");
                empty.Source = nameof(CompactJsonFormatParser);
                empty.Module = "Analogy.LogViewer.Serilog";
                parsedMessages.Add(empty);
                messagesHandler.AppendMessages(parsedMessages, fileName);
                return parsedMessages;
            }
        }
    }
}