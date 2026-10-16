using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using Analogy.LogViewer.Serilog.Managers;
using Analogy.LogViewer.Template.Managers;
using Microsoft.Extensions.Logging;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Analogy.LogViewer.Serilog
{
    public static class CommonParser
    {
        private const string DatePropertyName = "Date";
        private const string DatePropertyAlias = "Date (property)";

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
                        LogManager.Instance.LogWarning("invalid log level:" + evt.Level);
                        m.Level = AnalogyLogLevel.Unknown;
                        break;
                    }
            }

            m.Date = evt.Timestamp;
            m.Text = AnalogySink.output;
            if (evt.Properties.TryGetValue(Constants.ProcessName, out var processName))
            {
                if (processName is ScalarValue { Value: string processNameString })
                {
                    m.Module = processNameString;
                }
            }

            if (evt.Properties.TryGetValue(Constants.Source, out var sourceContext))
            {
                if (sourceContext is ScalarValue { Value: string sourceContextString })
                {
                    m.Source = sourceContextString;
                }
                else
                {
                    m.Source = sourceContext.ToString(null, System.Globalization.CultureInfo.InvariantCulture);
                }
            }

            if (evt.Properties.TryGetValue(Constants.ThreadId, out var threadId))
            {
                if (threadId is ScalarValue scalarValue)
                {
                    m.ThreadId = scalarValue.Value switch
                    {
                        int intValue => intValue,
                        long longValue and <= int.MaxValue => (int)longValue,
                        _ => m.ThreadId,
                    };
                }
            }
            if (evt.Properties.TryGetValue(Constants.ProcessId, out var processId))
            {
                if (processId is ScalarValue scalarValue)
                {
                    m.ProcessId = scalarValue.Value switch
                    {
                        int intValue => intValue,
                        long longValue and <= int.MaxValue => (int)longValue,
                        _ => m.ProcessId,
                    };
                }
            }
            if (evt.Properties.TryGetValue(Constants.MachineName, out var machineName))
            {
                if (machineName is ScalarValue { Value: string machineNameString })
                {
                    m.MachineName = machineNameString;
                }
            }
            if (evt.Properties.TryGetValue(Constants.EnvironmentUserName, out var environmentUserName))
            {
                if (environmentUserName is ScalarValue { Value: string environmentUserNameString})
                {
                    m.User = environmentUserNameString;
                }
            }
            if (evt.Properties.TryGetValue(Constants.User, out var environmentUser))
            {
                m.User = environmentUser switch
                {
                    ScalarValue { Value: string environmentUserString } scalarValue => environmentUserString,
                    StructureValue structure => structure.ToString(null,
                        System.Globalization.CultureInfo.InvariantCulture),
                    _ => m.User,
                };
            }
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

                var key = property.Key.Equals(DatePropertyName, StringComparison.OrdinalIgnoreCase)
                    ? DatePropertyAlias
                    : property.Key;
                m.AddOrReplaceAdditionalProperty(key, property.Value.ToString(null, System.Globalization.CultureInfo.InvariantCulture));
            }
            return m;
        }
    }
}