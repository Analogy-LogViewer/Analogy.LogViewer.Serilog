using Analogy.Interfaces;
using Analogy.LogViewer.Serilog.DataTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Analogy.Interfaces.DataTypes;

namespace Analogy.LogViewer.Serilog
{
    public class JsonFileParser
    {
        private IMessageFields messageFields;

        public JsonFileParser(IMessageFields messageFields)
        {
            this.messageFields = messageFields;
        }
        public async Task<IEnumerable<AnalogyLogMessage>> Process(string fileName, CancellationToken token, ILogMessageCreatedHandler messagesHandler)
        {
            var messages = await Task<IEnumerable<AnalogyLogMessage>>.Factory.StartNew(() =>
            {
                List<AnalogyLogMessage> parsedMessages = new List<AnalogyLogMessage>();
                try
                {
                    using (var analogy = new LoggerConfiguration()
                        .MinimumLevel.Verbose()
                        .WriteTo.Analogy()
                        .CreateLogger())
                    {
                        using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            string jsonData;
                            if (fileName.EndsWith(".gz", StringComparison.InvariantCultureIgnoreCase))
                            {
                                using (var gzStream = new GZipStream(fileStream, CompressionMode.Decompress))
                                {
                                    using (var streamReader = new StreamReader(gzStream, encoding: Encoding.UTF8))
                                    {
                                        jsonData = streamReader.ReadToEnd();

                                    }
                                }
                            }
                            else
                            {
                                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                                {
                                    jsonData = streamReader.ReadToEnd();
                                }
                            }


                            var data = JsonConvert.DeserializeObject(jsonData);
                            if (data is JObject jo)
                            {
                                var evt = LogEventReader.ReadFromJObject(jo, messageFields);
                                analogy.Write(evt);
                                AnalogyLogMessage m = CommonParser.ParseLogEventProperties(evt);
                                m.RawText = jo.ToString(Formatting.None);
                                m.RawTextType = AnalogyRowTextType.JSON;
                                parsedMessages.Add(m);
                                messagesHandler.ReportFileReadProgress(new AnalogyFileReadProgress(AnalogyFileReadProgressType.Percentage, 1, 1));
                            }
                            else if (data is JArray arr)
                            {
                                for (var i = 0; i < arr.Count; i++)
                                {
                                    var obj = arr[i];
                                    if (obj is JObject j)
                                    {
                                        var evt = LogEventReader.ReadFromJObject(j, messageFields);
                                        analogy.Write(evt);
                                        AnalogyLogMessage m = CommonParser.ParseLogEventProperties(evt);
                                        m.RawText = j.ToString(Formatting.None);
                                        m.RawTextType = AnalogyRowTextType.JSON;
                                        parsedMessages.Add(m);
                                        messagesHandler.ReportFileReadProgress(new AnalogyFileReadProgress(AnalogyFileReadProgressType.Percentage, i, arr.Count));
                                    }
                                }
                            }
                        }
                    }

                    messagesHandler.AppendMessages(parsedMessages, fileName);
                    return parsedMessages;
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
            });
            return messages;
        }
    }
}
