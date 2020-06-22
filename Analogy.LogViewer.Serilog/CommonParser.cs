using Analogy.Interfaces;
using Analogy.LogViewer.Serilog.Managers;
using Serilog.Events;

namespace Analogy.LogViewer.Serilog
{
    public static class CommonParser
    {

        public static AnalogyLogMessage ParseLogEventProperties(LogEvent evt)
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
                    {
                        LogManager.Instance.LogWarning("Serilog parser", "invalid log level:" + evt.Level);
                        m.Level = AnalogyLogLevel.Unknown;
                        break;
                    }
            }

            m.Date = evt.Timestamp.DateTime;
            m.Text = AnalogySink.output;
            if (evt.Properties.TryGetValue(Constants.ProcessName, out var processName))
            {
                if (processName is ScalarValue scalarValue &&
                    scalarValue.Value is string processNameString)
                {
                    m.Module = processNameString;
                }
            }

            if (evt.Properties.TryGetValue(Constants.Source, out var sourceContext))
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

            if (evt.Properties.TryGetValue(Constants.ThreadId, out var threadId))
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
            if (evt.Properties.TryGetValue(Constants.ProcessId, out var processId))
            {
                if (processId is ScalarValue scalarValue)
                {
                    if (scalarValue.Value is int intValue)
                    {
                        m.ProcessID = intValue;
                    }
                    else if (scalarValue.Value is long longValue && longValue <= int.MaxValue)
                    {
                        m.ProcessID = (int)longValue;
                    }
                }
            }
            if (evt.Properties.TryGetValue(Constants.MachineName, out var machineName))
            {
                if (machineName is ScalarValue scalarValue &&
                    scalarValue.Value is string machineNameString)
                {
                    m.MachineName = machineNameString;
                }
            }
            if (evt.Properties.TryGetValue(Constants.EnvironmentUserName, out var environmentUserName))
            {
                if (environmentUserName is ScalarValue scalarValue &&
                    scalarValue.Value is string environmentUserNameString)
                {
                    m.User = environmentUserNameString;
                }
            }
            return m;
        }
    }
}
