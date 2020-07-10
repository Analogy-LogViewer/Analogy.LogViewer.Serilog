using System;
using System.Collections.Generic;
using Analogy.Interfaces;

namespace Analogy.LogViewer.Serilog
{
    public static class ChangeLogList
    {
        public static IEnumerable<AnalogyChangeLog> GetChangeLog()
        {
            yield return new AnalogyChangeLog("Duplicated/Extra columns with the dynamic columns feature. #44", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2020, 07, 10));
            yield return new AnalogyChangeLog("Add dynamic columns per file properties. #43", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2020, 07, 03));
            yield return new AnalogyChangeLog("Initial version", AnalogChangeLogType.None, "Lior Banai", new DateTime(2019, 12, 14));
        }
    }
}
