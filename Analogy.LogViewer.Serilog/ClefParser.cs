using Analogy.Interfaces;
using Analogy.LogViewer.Serilog.CompactClef;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Serilog
{
    public class ClefParser
    {
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
                        using (var clef = new StreamReader(fileStream, encoding: Encoding.UTF8))
                        {
                            var reader = new LogEventReader(clef);
                            while (reader.TryRead(out var evt))
                            {
                                analogy.Write(evt);
                                AnalogyLogMessage m = CommonParser.ParseLogEventProperties(evt);
                                parsedMessages.Add(m);
                            }

                            messagesHandler.AppendMessages(parsedMessages, fileName);
                            return parsedMessages;
                        }
                    }
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

    }
}
