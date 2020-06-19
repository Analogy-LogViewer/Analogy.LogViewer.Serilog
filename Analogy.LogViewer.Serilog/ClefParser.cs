using Analogy.Interfaces;
using Analogy.LogViewer.Serilog.CompactClef;
using Serilog;
using Serilog.Events;
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
                                m.Text = AnalogySink.output;

                                if (evt.Properties.TryGetValue(global::Serilog.Core.Constants.SourceContextPropertyName, out var sourceContext))
                                {
                                    if (sourceContext is ScalarValue scalarValue && scalarValue.Value is string sourceContextString)
                                    {
                                        m.Source = sourceContextString;
                                    }
                                    else
                                    {
                                        m.Source = sourceContext.ToString();
                                    }
                                }

                                if (evt.Properties.TryGetValue("ThreadId", out var threadId))
                                {
                                    if (threadId is ScalarValue scalarValue)
                                    {
                                        if (scalarValue.Value is int intValue)
                                        {
                                            m.Thread = intValue;
                                        }
                                        else if (scalarValue.Value is long longValue && longValue <= int.MaxValue)
                                        {
                                            m.Thread = (int)longValue;
                                        }
                                    }
                                }

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
