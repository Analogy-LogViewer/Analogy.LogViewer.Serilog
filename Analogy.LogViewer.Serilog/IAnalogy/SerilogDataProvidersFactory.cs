using Analogy.Interfaces;
using Analogy.LogViewer.Template;
using System;
using System.Collections.Generic;

namespace Analogy.LogViewer.Serilog.IAnalogy
{
    public class SerilogDataProvidersFactory : DataProvidersFactory
    {
        public override Guid FactoryId { get; set; } = SerilogPrimaryFactory.Id;
        public override string Title { get; set; } = "Serilog Parser";

        public override IEnumerable<IAnalogyDataProvider> DataProviders { get; set; } = new List<IAnalogyDataProvider> { new SerilogOfflineDataProvider() };
    }
}