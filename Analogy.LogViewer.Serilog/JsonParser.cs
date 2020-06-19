using Analogy.Interfaces;
using Analogy.LogViewer.Serilog.CompactClef;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using Serilog.Formatting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Serilog
{
    public class JsonParser
    {
        public static ITextFormatter textFormatter;
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
                        string json = File.ReadAllText(fileName);
                        var data = JsonConvert.DeserializeObject(json);
                        if (data is JObject jo)
                        {
                            var m = ParserJObject(jo, analogy);
                            parsedMessages.Add(m);
                        }
                        else if (data is JArray arr)
                        {
                            foreach (var obj in arr.ToList())
                            {
                                if (obj is JObject j)
                                {
                                    var m = ParserJObject(j, analogy);
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
                    AnalogyLogMessage empty = new AnalogyLogMessage($"Error reading file {fileName}: Error: {e.Message}",
                        AnalogyLogLevel.Error, AnalogyLogClass.General, "Analogy", "None");
                    empty.Source = nameof(ClefParser);
                    empty.Module = "Analogy.LogViewer.Serilog";
                    parsedMessages.Add(empty);
                    messagesHandler.AppendMessages(parsedMessages, fileName);
                    return parsedMessages;
                }
            });
            return messages;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private AnalogyLogMessage ParserJObject(JObject jo, ILogger analogy)
        {
            var evt = LogEventReader.ReadFromJObject(jo);
            {
                analogy.Write(evt);
                return CommonParser.ParseLogEventProperties(evt);

            }

        }

    }
}
