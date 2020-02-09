using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Analogy.Interfaces;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact.Reader;

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
                        using (var clef = File.OpenText(fileName))
                        {
                            var reader = new LogEventReader(clef);
                            LogEvent evt;
                            while (reader.TryRead(out evt))
                            {
                                analogy.Write(evt);
                                var m = ParseEvent(evt);
                                parsedMessages.Add(m);
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
        private AnalogyLogMessage ParseEvent(LogEvent evt)
        {
            AnalogyLogMessage m = new AnalogyLogMessage();
            switch (evt.Level)
            {
                case LogEventLevel.Verbose:
                    m.Level = AnalogyLogLevel.Verbose;
                    break;
                case LogEventLevel.Debug:
                    m.Level = AnalogyLogLevel.Debug;
                    break;
                case LogEventLevel.Information:
                    m.Level = AnalogyLogLevel.Event;
                    break;
                case LogEventLevel.Warning:
                    m.Level = AnalogyLogLevel.Warning;
                    break;
                case LogEventLevel.Error:
                    m.Level = AnalogyLogLevel.Error;
                    break;
                case LogEventLevel.Fatal:
                    m.Level = AnalogyLogLevel.Critical;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            m.Date = evt.Timestamp.DateTime;
            m.Text = evt.ToString();
            return m;
        }
    }
}
