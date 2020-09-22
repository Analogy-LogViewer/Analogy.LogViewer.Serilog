using Analogy.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using Serilog.Formatting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Analogy.LogViewer.Serilog.DataTypes;

namespace Analogy.LogViewer.Serilog
{
    public class JsonFileParser
    {
        private static IMessageFields messageFields;
        public static ITextFormatter textFormatter;

        static  JsonFileParser()
        {
            messageFields= new JsonFormatMessageFields();
        }
        public async Task<IEnumerable<AnalogyLogMessage>> Process(string fileName, CancellationToken token, ILogMessageCreatedHandler messagesHandler)
        {
            var messages = await Task<IEnumerable<AnalogyLogMessage>>.Factory.StartNew(() =>
            {
                List<AnalogyLogMessage> parsedMessages = new List<AnalogyLogMessage>();
                try
                {
                    using (var analogy = new LoggerConfiguration()
                        .WriteTo.Analogy()
                        .CreateLogger())
                    {
                        using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                        {
                            LogEventReader reader = new LogEventReader(streamReader, messageFields);
                            var json = streamReader.ReadToEnd();
                            var data = JsonConvert.DeserializeObject(json);
                            if (data is JObject jo)
                            {
                                while (reader.TryRead(out var evt))
                                {
                                    analogy.Write(evt);
                                    AnalogyLogMessage m = CommonParser.ParseLogEventProperties(evt);
                                    parsedMessages.Add(m);
                                }
                            }
                            else if (data is JArray arr)
                            {
                                foreach (var obj in arr.ToList())
                                {
                                    if (obj is JObject j)
                                    {
                                        while (reader.TryRead(out var evt))
                                        {
                                            analogy.Write(evt);
                                            AnalogyLogMessage m = CommonParser.ParseLogEventProperties(evt);
                                            parsedMessages.Add(m);
                                        }
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
