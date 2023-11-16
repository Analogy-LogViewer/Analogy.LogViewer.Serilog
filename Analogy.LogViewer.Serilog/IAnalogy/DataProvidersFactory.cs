using Analogy.Interfaces;
using System;
using System.Collections.Generic;

namespace Analogy.LogViewer.Serilog.IAnalogy
{
    public class DataProvidersFactory : Analogy.LogViewer.Template.DataProvidersFactory
    {
        public override Guid FactoryId { get; set; } = PrimaryFactory.Id;
        public override string Title { get; set; } = "Serilog Parser";

        public override IEnumerable<IAnalogyDataProvider> DataProviders { get; set; } =
            new List<IAnalogyDataProvider> { new OfflineDataProvider() };
    }
}