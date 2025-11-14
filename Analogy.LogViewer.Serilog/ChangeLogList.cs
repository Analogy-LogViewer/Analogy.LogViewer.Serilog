using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using System;
using System.Collections.Generic;

namespace Analogy.LogViewer.Serilog
{
    public static class ChangeLogList
    {
        public static IEnumerable<AnalogyChangeLog> GetChangeLog() =>
        new List<AnalogyChangeLog>
        {
#pragma warning disable MA0132
            new("[V6.0.0] - NET8", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2023, 11, 16), "6.0.0"),
            new("[V2.6.0] - [UI] Add parsing counter #374", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2022, 04, 04), ""),
            new("[V2.5.0] - Add NET6 compilation target #330", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2021, 11, 09), ""),
            new("[V2.3.0] - fill the new Analogy Interface fields #231", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2021, 02, 11), ""),
            new("[V2.3.0] - Add portable option. #225", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2021, 01, 30), ""),
            new("[V2.2.0] - Bump Serilog from 2.9.0 to 2.10.0 #151", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2020, 12, 01), ""),
            new("[V2.1.4] - File format detection fails if file is already in use. #166", AnalogChangeLogType.Bug, "Lior Banai", new DateTime(2020, 10, 26), ""),
            new("[V2.1.3] - Add Update mechanism #164", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2020, 10, 25), ""),
            new("[V2.1.1] - When application is not running in admin UnauthorizedAccessException can be thrown #150", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2020, 10, 21), ""),
            new("[V2.1.0] - Serilog Trace Log Level are skipped in the parsers since debug is default level #148", AnalogChangeLogType.Bug, "Darko Vujičić", new DateTime(2020, 10, 18), ""),
            new("[V2.1.0] - [Implementation] Simplified code by using Analogy.LogViewer.Template as base implementation #121", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2020, 10, 06), ""),
            new("[V2.0.0] - Add Raw message data property and Json Viewer when relevant #109", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2020, 09, 26), ""),
            new("[V2.0.0] - Remove Regex parser from Serilog parser #112", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2020, 09, 26), ""),
            new("[V2.0.0] - Add Auto detect formatter used in log file so user won't need to define the formatter in the user settings #61", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2020, 09, 26), ""),
            new("[V2.0.0] - Add Additional Text Formatters (CompactJsonFormatter,JsonFormatter ) #6", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2020, 09, 26), ""),
            new("Support for reading compressed files. #45", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2020, 07, 22), ""),
            new("Duplicated/Extra columns with the dynamic columns feature. #44", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2020, 07, 10), ""),
            new("Add dynamic columns per file properties. #43", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2020, 07, 03), ""),
            new("Initial version", AnalogChangeLogType.None, "Lior Banai", new DateTime(2019, 12, 14), ""),
#pragma warning restore MA0132
        };
    }
}