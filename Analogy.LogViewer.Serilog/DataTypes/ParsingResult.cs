using Serilog.Events;

namespace Analogy.LogViewer.Serilog.DataTypes
{
    public class ParsingResult
    {
        public LogEvent? evt { get; set; }
        public string Line { get; set; }

        public ParsingResult(LogEvent? evt, string line)
        {
            this.evt = evt;
            this.Line = line;
        }
    }
}