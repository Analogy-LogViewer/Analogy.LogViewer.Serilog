using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using Serilog.Formatting.Json;
using System;
using System.IO;

namespace Analogy.LogViewer.Serilog
{
    public class AnalogySink : ILogEventSink
    {
        private readonly ITextFormatter _textFormatter;
        public static string output = string.Empty;
        public AnalogySink(ITextFormatter textFormatter)
        {
            _textFormatter = textFormatter ?? throw new ArgumentNullException(nameof(textFormatter));
        }

        public void Emit(LogEvent logEvent)
        {
            if (logEvent == null)
            {
                throw new ArgumentNullException(nameof(logEvent));
            }

            var sr = new StringWriter();
            _textFormatter.Format(logEvent, sr);
            output = sr.ToString().Trim();
        }
    }

    public static class LoggerConfigurationExtensions
    {
        private const string DefaultOutputTemplate = "{Message}{NewLine}{Exception}";

        /// <summary>
        /// Write log events to the <see cref="System.Diagnostics.Trace"/>.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="restrictedToMinimumLevel">The minimum level for
        /// events passed through the sink. Ignored when <paramref name="levelSwitch"/> is specified.</param>
        /// <param name="levelSwitch">A switch allowing the pass-through minimum level
        /// to be changed at runtime.</param>
        /// <param name="outputTemplate">A message template describing the format used to write to the sink.
        /// the default is "{Timestamp} [{Level}] {Message}{NewLine}{Exception}".</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggerConfiguration Analogy(
            this LoggerSinkConfiguration sinkConfiguration,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            string outputTemplate = DefaultOutputTemplate,
            IFormatProvider formatProvider = null,
            LoggingLevelSwitch levelSwitch = null)
        {
            if (sinkConfiguration == null)
            {
                throw new ArgumentNullException(nameof(sinkConfiguration));
            }

            if (outputTemplate == null)
            {
                throw new ArgumentNullException(nameof(outputTemplate));
            }

            var formatter = new MessageTemplateTextFormatter(outputTemplate, formatProvider);
            return Analogy(sinkConfiguration, formatter, restrictedToMinimumLevel, levelSwitch);
        }

        /// <summary>
        /// Write log events to the <see cref="System.Diagnostics.Trace"/>.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="restrictedToMinimumLevel">The minimum level for
        /// events passed through the sink. Ignored when <paramref name="levelSwitch"/> is specified.</param>
        /// <param name="levelSwitch">A switch allowing the pass-through minimum level
        /// to be changed at runtime.</param>
        /// <param name="formatter">A custom formatter to apply to the output events. This can be used with
        /// e.g. <see cref="JsonFormatter"/> to produce JSON output. To customize the text layout only, use the
        /// overload that accepts an output template instead.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggerConfiguration Analogy(
            this LoggerSinkConfiguration sinkConfiguration,
            ITextFormatter formatter,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            LoggingLevelSwitch levelSwitch = null)
        {
            if (sinkConfiguration == null)
            {
                throw new ArgumentNullException(nameof(sinkConfiguration));
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            return sinkConfiguration.Sink(new AnalogySink(formatter), restrictedToMinimumLevel, levelSwitch);
        }
    }
}