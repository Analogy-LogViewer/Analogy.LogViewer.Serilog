using Serilog.Events;

namespace Analogy.LogViewer.Serilog.DataTypes
{
    public class ParsingResult
    {
        public LogEvent? Evt { get; set; }
        public string Line { get; set; }

        public ParsingResult(LogEvent? evt, string line)
        {
            this.Evt = evt;
            this.Line = line;
        }
    }
}