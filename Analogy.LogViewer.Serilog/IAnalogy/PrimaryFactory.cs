using Analogy.Interfaces;
using Analogy.Interfaces.Factories;
using System;
using System.Collections.Generic;
using System.Drawing;
using Analogy.LogViewer.Serilog.Properties;

namespace Analogy.LogViewer.Serilog.IAnalogy
{
    public class PrimaryFactory : IAnalogyFactory
    {
        internal static Guid Id { get; } = new Guid("513A4393-425E-4054-92D4-6A816983E51F");
        public Guid FactoryId { get; set; } = Id;
        public string Title { get; set; } = "Serilog Parser";
        public IEnumerable<IAnalogyChangeLog> ChangeLog { get; set; } = ChangeLogList.GetChangeLog();
        public IEnumerable<string> Contributors { get; set; } = new List<string> { "Lior Banai" };
        public string About { get; set; } = "Serilog Parser for Analogy Log Viewer";
        public Image SmallImage { get; set; } = Resources.AnalogySerilog16x16;
        public Image LargeImage { get; set; } = Resources.AnalogySerilog32x32;


    }
}