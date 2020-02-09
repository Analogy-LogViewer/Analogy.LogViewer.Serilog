using System;
using System.Collections.Generic;
using Analogy.Interfaces;

namespace Analogy.LogViewer.Serilog
{
    public static class ChangeLogList
    {
        public static IEnumerable<AnalogyChangeLog> GetChangeLog()
        {
            yield return new AnalogyChangeLog("Initial version", AnalogChangeLogType.None, "Lior Banai", new DateTime(2019, 12, 14));
        }
    }
}
