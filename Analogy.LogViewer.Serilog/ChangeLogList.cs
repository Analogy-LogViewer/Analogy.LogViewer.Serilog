using System;
using System.Collections.Generic;
using Analogy.Interfaces;

namespace Analogy.LogViewer.Serilog
{
    public static class ChangeLogList
    {
        public static IEnumerable<AnalogyChangeLog> GetChangeLog()=>
        new List<AnalogyChangeLog>()
        {
            new AnalogyChangeLog("[V2.1.0] - Serilog Trace Log Level are skipped in the parsers since debug is default level #148", AnalogChangeLogType.Bug, "Darko Vujičić", new DateTime(2020, 10, 18)),
            new AnalogyChangeLog("[V2.1.0] - [Implementation] Simplified code by using Analogy.LogViewer.Template as base implementation #121", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2020, 10, 06)),
            new AnalogyChangeLog("[V2.0.0] - Add Raw message data property and Json Viewer when relevant #109", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2020, 09, 26)),
            new AnalogyChangeLog("[V2.0.0] - Remove Regex parser from Serilog parser #112", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2020, 09, 26)),
            new AnalogyChangeLog("[V2.0.0] - Add Auto detect formatter used in log file so user won't need to define the formatter in the user settings #61", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2020, 09, 26)),
            new AnalogyChangeLog("[V2.0.0] - Add Additional Text Formatters (CompactJsonFormatter,JsonFormatter ) #6", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2020, 09, 26)),
            new AnalogyChangeLog("Support for reading compressed files. #45", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2020, 07, 22)),
            new AnalogyChangeLog("Duplicated/Extra columns with the dynamic columns feature. #44", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2020, 07, 10)),
            new AnalogyChangeLog("Add dynamic columns per file properties. #43", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2020, 07, 03)),
            new AnalogyChangeLog("Initial version", AnalogChangeLogType.None, "Lior Banai", new DateTime(2019, 12, 14))
        };
    }
}
