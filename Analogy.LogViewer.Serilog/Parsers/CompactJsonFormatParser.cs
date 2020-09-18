using Analogy.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Analogy.LogViewer.Serilog.DataTypes;

namespace Analogy.LogViewer.Serilog
{
    public class CompactJsonFormatParser
    {
        private static IMessageFields messageFields;

        static CompactJsonFormatParser()
        {
            messageFields=new CompactJsonFormatMessageFields();
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
                        {
                            if (fileName.EndsWith(".gz", StringComparison.InvariantCultureIgnoreCase))
                            {
                                using (var gzStream = new GZipStream(fileStream, CompressionMode.Decompress))
                                {
                                    using (var streamReader = new StreamReader(gzStream, encoding: Encoding.UTF8))
                                    {
                                        var reader = new LogEventReader(streamReader, messageFields);
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
                            else
                            {
                                using (var streamReader = new StreamReader(fileStream, encoding: Encoding.UTF8))
                                {
                                    var reader = new LogEventReader(streamReader, messageFields);
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


            });
            return messages;
        }

    }
}
