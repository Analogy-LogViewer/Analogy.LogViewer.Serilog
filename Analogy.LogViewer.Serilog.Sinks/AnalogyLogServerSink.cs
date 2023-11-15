using Analogy.Interfaces;
using Analogy.LogServer.Clients;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;

namespace Analogy.LogViewer.Serilog.Sinks
{
    public class AnalogyLogServerSink : ILogEventSink, IDisposable
    {
        private readonly IFormatProvider _formatProvider;
        private AnalogyMessageProducer logServerMessageProducer;

        public AnalogyLogServerSink(IFormatProvider formatProvider)
        {
            _formatProvider = formatProvider;
            logServerMessageProducer = new AnalogyMessageProducer("http://localhost:6000");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formatProvider"></param>
        /// <param name="address"></param>
        public AnalogyLogServerSink(IFormatProvider formatProvider, string address)
        {
            _formatProvider = formatProvider;
            logServerMessageProducer = new AnalogyMessageProducer(address);
        }
        public void Emit(LogEvent logEvent)
        {
            var alm = ParseLogEventProperties(logEvent, in _formatProvider);
            _ = (logServerMessageProducer?.Log(alm.Text, alm.Source, alm.Level, "", alm.MachineName, alm.User,
                alm.Module, alm.ProcessId, alm.ThreadId, alm.AdditionalProperties, alm.MethodName, alm.LineNumber, alm.FileName));
        }

        public static AnalogyLogMessage ParseLogEventProperties(LogEvent evt, in IFormatProvider formatProvider)
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
                        m.Level = AnalogyLogLevel.Unknown;
                        break;
                    }
            }

            m.Date = evt.Timestamp.DateTime;
            m.Text = evt.RenderMessage(formatProvider);
            if (evt.Properties.TryGetValue(Constants.ProcessName, out var processName))
            {
                if (processName is ScalarValue { Value: string processNameString })
                {
                    m.Module = processNameString;
                }
            }

            if (evt.Properties.TryGetValue(Constants.Source, out var sourceContext))
            {
                m.Source = sourceContext is ScalarValue { Value: string sourceContextString }
                    ? sourceContextString
                    : sourceContext.ToString();
            }

            if (evt.Properties.TryGetValue(Constants.ThreadId, out var threadId))
            {
                if (threadId is ScalarValue scalarValue)
                {
                    if (scalarValue.Value is int intValue)
                    {
                        m.ThreadId = intValue;
                    }
                    else if (scalarValue.Value is long longValue and <= int.MaxValue)
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
                    else if (scalarValue.Value is long longValue and <= int.MaxValue)
                    {
                        m.ProcessId = (int)longValue;
                    }
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
                if (environmentUserName is ScalarValue { Value: string environmentUserNameString })
                {
                    m.User = environmentUserNameString;
                }
            }
            if (evt.Properties.TryGetValue(Constants.User, out var environmentUser))
            {
                if (environmentUser is ScalarValue { Value: string environmentUserString })
                {
                    m.User = environmentUserString;
                }
                if (environmentUser is StructureValue structure)

                {
                    m.User = structure.ToString();
                }
            }
            foreach (KeyValuePair<string, LogEventPropertyValue> property in evt.Properties)
            {
                if (property.Key.Equals(Constants.EnvironmentUserName) ||
                    property.Key.Equals(Constants.Source) ||
                    property.Key.Equals(Constants.ThreadId) ||
                    property.Key.Equals(Constants.ProcessName) ||
                    property.Key.Equals(Constants.MachineName) ||
                    property.Key.Equals(Constants.User) ||
                    property.Key.Equals(Constants.ProcessId))
                {
                    continue;
                }
                m.AddOrReplaceAdditionalProperty(property.Key, property.Value.ToString());
            }
            return m;
        }

        public void Dispose()
        {
            logServerMessageProducer.StopReceiving();
            logServerMessageProducer.Dispose();
        }
    }
}