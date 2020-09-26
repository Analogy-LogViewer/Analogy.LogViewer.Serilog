using System;
using System.Collections.Generic;
using Analogy.Interfaces;
using Analogy.Interfaces.Factories;

namespace Analogy.LogViewer.Serilog.IAnalogy
{
    public class DataProvidersFactory : IAnalogyDataProvidersFactory
    {
        public Guid FactoryId { get; set; } = PrimaryFactory.Id;
        public string Title { get; set; } = "Serilog Parser";

        public IEnumerable<IAnalogyDataProvider> DataProviders { get; } =
            new List<IAnalogyDataProvider> { new OfflineDataProvider() };

    }
}
