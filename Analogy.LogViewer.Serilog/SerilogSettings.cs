using Analogy.LogViewer.Serilog.Regex;
using System.Collections.Generic;

namespace Analogy.LogViewer.Serilog
{
    public enum SerilogFileFormat
    {
        CLEF,
        JSON,
        REGEX
    }
    public class SerilogSettings
    {
        public string FileOpenDialogFilters { get; set; }
        public string FileSaveDialogFilters { get; } = string.Empty;
        public List<string> SupportFormats { get; set; }
        public List<RegexPattern> RegexPatterns { get; set; }
        public string Directory { get; set; }
        public SerilogFileFormat Format { get; set; }
        public List<string> IgnoredAttributes { get; set; }

        public SerilogSettings()
        {
            Format = SerilogFileFormat.CLEF;
            Directory = string.Empty;
            IgnoredAttributes = new List<string>() { "N" };
            FileOpenDialogFilters = "All Supported formats (*.Clef;*.log;*.gz)|*.clef;*.log;*.gz|Clef format (*.clef)|*.clef|Plain log text file (*.log)|*.log|GZIP file (*.gz)|*.gz";
            SupportFormats = new List<string> { "*.Clef", "*.log", "*.gz" };
            RegexPatterns = new List<RegexPattern>();
            RegexPatterns.Add(new RegexPattern(@"\$(?<Date>\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2},\d{3})+\|+(?<Thread>\d+)+\|(?<Level>\w+)+\|+(?<Source>.*)\|(?<Text>.*)", "yyyy-MM-dd HH:mm:ss,fff", ""));

        }




    }
}
