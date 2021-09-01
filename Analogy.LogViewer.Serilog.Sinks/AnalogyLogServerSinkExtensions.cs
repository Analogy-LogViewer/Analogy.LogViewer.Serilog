using System;
using System.Collections.Generic;
using System.Text;
using Serilog;
using Serilog.Configuration;

namespace Analogy.LogViewer.Serilog.Sinks
{
    public static class AnalogyLogServerSinkExtensions
    {
        public static LoggerConfiguration AnalogyLogServerSink(this LoggerSinkConfiguration loggerConfiguration, IFormatProvider formatProvider = null)
        {
            return loggerConfiguration.Sink(new AnalogyLogServerSink(formatProvider));
        }
        public static LoggerConfiguration AnalogyLogServerSink(this LoggerSinkConfiguration loggerConfiguration, string address, IFormatProvider formatProvider = null)
        {
            return loggerConfiguration.Sink(new AnalogyLogServerSink(formatProvider, address));
        }
    }
}
