using System;
using System.Collections.Generic;
using Analogy.Interfaces;
using Analogy.Interfaces.Factories;

namespace Analogy.LogViewer.Serilog
{
    public class PrimaryFactory : IAnalogyFactory
    {
        public Guid FactoryID { get; } = new Guid("513A4393-425E-4054-92D4-6A816983E51F");
        public string Title { get; } = "Serilog Parser";
        public IAnalogyDataProvidersFactory DataProviders { get; }
        public IAnalogyCustomActionsFactory Actions { get; } = new EmptyActionsFactory(); // if no custom action needed
        public IEnumerable<IAnalogyChangeLog> ChangeLog { get; } = ChangeLogList.GetChangeLog();
        public IEnumerable<string> Contributors { get; } = new List<string> { "Lior Banai" };
        public string About { get; } = "Serilog Parser for Analogy Log Viewer";

        public PrimaryFactory()
        {
            DataProviders = new DataProvidersFactory();
        }
    }
}