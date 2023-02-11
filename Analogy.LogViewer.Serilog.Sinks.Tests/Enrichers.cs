using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog.Events;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Analogy.LogViewer.Serilog.Sinks.Tests
{
    public class ProcessNameEnricher: ILogEventEnricher
    {
        LogEventProperty _cachedProperty;

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(GetLogEventProperty(propertyFactory));

        }
        private LogEventProperty GetLogEventProperty(ILogEventPropertyFactory propertyFactory)
        {
            // Don't care about thread-safety, in the worst case the field gets overwritten and one property will be GCed
            return _cachedProperty ??= CreateProperty(propertyFactory);
        }
 
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static LogEventProperty CreateProperty(ILogEventPropertyFactory propertyFactory)
        {
            var value = Process.GetCurrentProcess().ProcessName;
            return propertyFactory.CreateProperty("Process Name", value);
        }
    }
}
