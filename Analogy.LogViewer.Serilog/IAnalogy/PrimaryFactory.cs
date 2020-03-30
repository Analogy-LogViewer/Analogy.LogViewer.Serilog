using Analogy.Interfaces;
using Analogy.Interfaces.Factories;
using System;
using System.Collections.Generic;

namespace Analogy.LogViewer.Serilog.IAnalogy
{
    public class PrimaryFactory : IAnalogyFactory
    {
        internal static Guid Id = new Guid("513A4393-425E-4054-92D4-6A816983E51F");
        public Guid FactoryId { get; } = Id;
        public string Title { get; } = "Serilog Parser";
        public IEnumerable<IAnalogyChangeLog> ChangeLog { get; } = ChangeLogList.GetChangeLog();
        public IEnumerable<string> Contributors { get; } = new List<string> { "Lior Banai" };
        public string About { get; } = "Serilog Parser for Analogy Log Viewer";
    }
}