using System.Collections.Generic;
using System.Linq;
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
                    m.Level = AnalogyLogLevel.Information;
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
                        LogManager.Instance.LogWarning("invalid log level:" + evt.Level,"Serilog parser");
                        m.Level = AnalogyLogLevel.Unknown;
                        break;
                    }
            }

            m.Date = evt.Timestamp.DateTime;
            m.Text = AnalogySink.output;// evt.MessageTemplate.Text;
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
                        m.ThreadId = intValue;
                    }
                    else if (scalarValue.Value is long longValue && longValue <= int.MaxValue)
                    {
                        m.ThreadId = (int)longValue;
                    }
                }
            }
            if (evt.Properties.TryGetValue(Constants.ProcessId, out var processId))
            {
                if (processId is ScalarValue scalarValue)
                {
                    if (scalarValue.Value is int intValue)
                    {
                        m.ProcessId = intValue;
                    }
                    else if (scalarValue.Value is long longValue && longValue <= int.MaxValue)
                    {
                        m.ProcessId = (int)longValue;
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
                if (environmentUserName is ScalarValue {Value: string environmentUserNameString})
                {
                    m.User = environmentUserNameString;
                }
            }
            if (evt.Properties.TryGetValue(Constants.User, out var environmentUser))
            {
                if (environmentUser is ScalarValue {Value: string environmentUserString} scalarValue)
                {
                    m.User = environmentUserString;
                }
                if (environmentUser is StructureValue structure)

                {
                    m.User = structure.ToString();
                }
            }
            m.AdditionalInformation = new Dictionary<string, string>();
            foreach (KeyValuePair<string, LogEventPropertyValue> property in evt.Properties)
            {
                if (property.Key.Equals(Constants.EnvironmentUserName) ||
                    property.Key.Equals(Constants.Source) ||
                    property.Key.Equals(Constants.ThreadId) ||
                    property.Key.Equals(Constants.ProcessName) ||
                    property.Key.Equals(Constants.MachineName) ||
                    property.Key.Equals(Constants.User) ||
                    property.Key.Equals(Constants.ProcessId) ||
                    UserSettingsManager.UserSettings.Settings.IgnoredAttributes.Contains(property.Key))
                {
                    continue;
                }

                m.AdditionalInformation.Add(property.Key, property.Value.ToString());
            }
            return m;
        }
    }
}
