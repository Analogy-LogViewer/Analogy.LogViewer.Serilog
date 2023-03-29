using Analogy.LogViewer.Serilog.DataTypes;
using System.Collections.Generic;

namespace Analogy.LogViewer.Serilog
{

    public class SerilogSettings
    {
        public bool UseApplicationFolderForSettings { get; set; } = false;
        public string FileOpenDialogFilters { get; set; }
        public string FileSaveDialogFilters { get; } = string.Empty;
        public List<string> SupportFormats { get; set; }
        public string Directory { get; set; }
        public FileFormat Format { get; set; }
        public FileFormatDetection FileFormatDetection { get; set; }
        public List<string> IgnoredAttributes { get; set; }

        public SerilogSettings()
        {
            FileFormatDetection = FileFormatDetection.Automatic;
            Format = FileFormat.Unknown;
            Directory = string.Empty;
            IgnoredAttributes = new List<string>() { "N" };
            FileOpenDialogFilters = "All Supported formats (*.Clef;*.log;*.gz)|*.clef;*.log;*.gz;*.zip|Clef format (*.clef)|*.clef|Plain log text file (*.log)|*.log|GZIP file (*.gz)|*.gz|ZIP file (*.zip)|*.zip";
            SupportFormats = new List<string> { "*.Clef", "*.log", "*.gz" , "*.zip"};
        }




    }
}
