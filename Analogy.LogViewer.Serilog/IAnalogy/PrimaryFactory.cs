using Analogy.Interfaces;
using Analogy.Interfaces.Factories;
using System;
using System.Collections.Generic;
using System.Drawing;
using Analogy.LogViewer.Serilog.Properties;

namespace Analogy.LogViewer.Serilog.IAnalogy
{
    public class PrimaryFactory : Analogy.LogViewer.Template.PrimaryFactory
    {
        internal static Guid Id { get; } = new Guid("513A4393-425E-4054-92D4-6A816983E51F");
        public override Guid FactoryId { get; set; } = Id;
        public override string Title { get; set; } = "Serilog Parser";
        public override IEnumerable<IAnalogyChangeLog> ChangeLog { get; set; } = ChangeLogList.GetChangeLog();
        public override IEnumerable<string> Contributors { get; set; } = new List<string> { "Lior Banai" };
        public override string About { get; set; } = "Serilog Parser for Analogy Log Viewer";
        public override Image SmallImage { get; set; } = Resources.AnalogySerilog16x16;
        public override Image LargeImage { get; set; } = Resources.AnalogySerilog32x32;


    }
}