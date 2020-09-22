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
                        using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                        {
                            var json = streamReader.ReadToEnd();
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
