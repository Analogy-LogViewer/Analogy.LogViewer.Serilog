using Analogy.LogViewer.Serilog.DataTypes;
using System.Collections.Generic;

namespace Analogy.LogViewer.Serilog
{
    public class SerilogSettings
    {
        public bool UseApplicationFolderForSettings { get; set; }
        public string FileOpenDialogFilters { get; set; } = "All Supported formats (*.Clef;*.log;*.gz)|*.clef;*.log;*.gz;*.zip|Clef format (*.clef)|*.clef|Plain log text file (*.log)|*.log|GZIP file (*.gz)|*.gz|ZIP file (*.zip)|*.zip";
        public string FileSaveDialogFilters { get; } = string.Empty;
        public List<string> SupportFormats { get; set; } = new() { "*.Clef", "*.log"};
        public string Directory { get; set; } = string.Empty;
        public FileFormat Format { get; set; } = FileFormat.Unknown;
        public FileFormatDetection FileFormatDetection { get; set; } = FileFormatDetection.Automatic;
        public List<string> IgnoredAttributes { get; set; } = new() { "N" };
    }
}