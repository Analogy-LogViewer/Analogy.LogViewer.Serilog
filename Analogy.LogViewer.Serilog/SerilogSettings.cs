using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Serilog
{
    public enum SerilogFileFormat
    {
        CLEF,
        JSON,
        TEXT
    }
   public class SerilogSettings
    {
        public List<string> SupportedFilesExtensions { get; set; }
        public string Directory { get; set; }
        public SerilogFileFormat Format { get; set; }

        public SerilogSettings()
        {
            SupportedFilesExtensions = new List<string>();
            Format = SerilogFileFormat.CLEF;
            SupportedFilesExtensions = new List<string> { "*.Clef" };
        }
    }
}
