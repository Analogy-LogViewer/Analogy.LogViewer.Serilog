using Analogy.Interfaces;
using Analogy.LogViewer.Serilog.DataTypes;
using Serilog;
using Serilog.Formatting.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Analogy.LogViewer.Serilog
{
    public class JsonFormatterParser
    {
        private static IMessageFields messageFields;

        static JsonFormatterParser()
        {
            messageFields = new JsonFormatMessageFields();
        }
        public async Task<IEnumerable<AnalogyLogMessage>> Process(string fileName, CancellationToken token,
            ILogMessageCreatedHandler messagesHandler)
        {
            var formatter = new JsonFormatter();
            List<AnalogyLogMessage> parsedMessages = new List<AnalogyLogMessage>();
            try
            {
                using (var analogy = new LoggerConfiguration().WriteTo.Analogy(formatter)
                    .CreateLogger())
                {
                    using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        string json;
                        while ((json = await streamReader.ReadLineAsync()) != null)
                        {
                            var data = JsonConvert.DeserializeObject(json);
                            var evt = LogEventReader.ReadFromJObject(data as JObject, messageFields);
                            {
                                AnalogyLogMessage m = CommonParser.ParseLogEventProperties(evt);
                                parsedMessages.Add(m);
                            }
                        }
                    }

                }
                messagesHandler.AppendMessages(parsedMessages, fileName);
                return parsedMessages;
            }

            catch (Exception e)
            {
                AnalogyLogMessage empty = new AnalogyLogMessage($"Error reading file {fileName}: Error: {e.Message}", AnalogyLogLevel.Error, AnalogyLogClass.General, "Analogy", "None");
                empty.Source = nameof(CompactJsonFormatParser);
                empty.Module = "Analogy.LogViewer.Serilog";
                parsedMessages.Add(empty);
                messagesHandler.AppendMessages(parsedMessages, fileName);
                return parsedMessages;
            }
        }
    }
}