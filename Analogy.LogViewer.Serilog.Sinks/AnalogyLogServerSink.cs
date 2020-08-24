using System;
using Analogy.LogServer.Clients;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Configuration;

namespace Analogy.LogViewer.Serilog.Sinks
{
    public class AnalogyLogServerSink : ILogEventSink, IDisposable
    {
        private readonly IFormatProvider _formatProvider;
        private AnalogyMessageProducer logServerMessageProducer;

        public AnalogyLogServerSink(IFormatProvider formatProvider)
        {
            _formatProvider = formatProvider;
            logServerMessageProducer = new AnalogyMessageProducer("http://localhost:6000", null);
        }
        public void Emit(LogEvent logEvent)
        {

            var message = logEvent.RenderMessage(_formatProvider);
            var alm = CommonParser.ParseLogEventProperties(logEvent);
            alm.Text = message;
            logServerMessageProducer?.Log(alm.Text, alm.Source, alm.Level, alm.Category, alm.MethodName, alm.LineNumber,
                alm.FileName);
        }

        public void Dispose()
        {
            logServerMessageProducer = null;
            //logServerMessageProducer.StopReceiving();
        }
    }
}
