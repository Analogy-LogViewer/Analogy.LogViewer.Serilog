using Analogy.Interfaces;
using Analogy.Interfaces.Factories;
using Analogy.LogViewer.Serilog.IAnalogy;
using System;
using System.Collections.Generic;

namespace Analogy.LogViewer.Serilog
{
    public class DataProvidersFactory : IAnalogyDataProvidersFactory
    {
        public Guid FactoryId { get; set; } = PrimaryFactory.Id;
        public string Title { get; set; } = "Serilog Parser";

        public IEnumerable<IAnalogyDataProvider> DataProviders { get; } =
            new List<IAnalogyDataProvider> { new OfflineDataProvider() };

    }
}
