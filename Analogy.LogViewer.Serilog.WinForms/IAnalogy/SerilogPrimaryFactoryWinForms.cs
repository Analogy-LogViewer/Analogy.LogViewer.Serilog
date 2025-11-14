using Analogy.Interfaces;
using Analogy.Interfaces.WinForms.Factories;
using Analogy.LogViewer.Serilog.IAnalogy;
using Analogy.LogViewer.Serilog.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Analogy.LogViewer.Serilog.WinForms
{
    public class SerilogPrimaryFactoryWinForms : SerilogPrimaryFactory, IAnalogyFactoryWinForms
    {
        public override Guid FactoryId { get; set; } = Id;
        public override string Title { get; set; } = "Serilog Parser";
        public override IEnumerable<IAnalogyChangeLog> ChangeLog { get; set; } = ChangeLogList.GetChangeLog();
        public override IEnumerable<string> Contributors { get; set; } = new List<string> { "Lior Banai" };
        public override string About { get; set; } = "Serilog Parser for Analogy Log Viewer";
        public Image SmallImage { get; set; } = Resources.AnalogySerilog16x16;
        public Image LargeImage { get; set; } = Resources.AnalogySerilog32x32;
    }
}