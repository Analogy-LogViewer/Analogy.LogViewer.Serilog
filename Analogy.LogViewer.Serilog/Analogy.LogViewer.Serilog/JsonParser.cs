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
    public class JsonParser
    {
        public async Task<IEnumerable<AnalogyLogMessage>> Process(string fileName, CancellationToken token, ILogMessageCreatedHandler messagesHandler)
        {
            var messages = await Task<IEnumerable<AnalogyLogMessage>>.Factory.StartNew(() =>
            {
                List<AnalogyLogMessage> parsedMessages = new List<AnalogyLogMessage>();
                try
                {
                    string json = File.ReadAllText(fileName);
                    var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    AnalogyLogMessage m = new AnalogyLogMessage();
                    foreach (KeyValuePair<string, object> kvp in data)
                    {
                        switch (kvp.Key)
                        {
                            case "@t":
                                m.Date = (DateTime)kvp.Value;
                                break;
                            case "@mt":
                                m.Text = kvp.Value.ToString();
                                break;
                            case "@l":
                                m.Level = ParseEvent(kvp.Value.ToString());
                                break;
                            default:
                                m.Text += Environment.NewLine + kvp.Key + " - " + kvp.Value.ToString();
                                break;
                        }
                    }
                    messagesHandler.AppendMessage(m, fileName);
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
        private AnalogyLogLevel ParseEvent(string evt)
        {

            switch (evt)
            {
                case "Verbose":
                    return AnalogyLogLevel.Verbose;

                case "Debug":
                    return AnalogyLogLevel.Debug;

                case "Information":
                    return AnalogyLogLevel.Event;

                case "Warning":
                    return AnalogyLogLevel.Warning;

                case "Error":
                    return AnalogyLogLevel.Error;

                case "Fatal":
                    return AnalogyLogLevel.Critical;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
