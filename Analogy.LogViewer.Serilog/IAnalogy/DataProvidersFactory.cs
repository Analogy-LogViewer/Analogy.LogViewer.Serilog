using Analogy.Interfaces;
using Analogy.Interfaces.Factories;
using Analogy.LogViewer.Serilog.IAnalogy;
using System;
using System.Collections.Generic;

namespace Analogy.LogViewer.Serilog
{
    public class DataProvidersFactory : IAnalogyDataProvidersFactory
    {
        public Guid FactoryId { get; } = PrimaryFactory.Id;
        public string Title { get; } = "Serilog Parser";

        public IEnumerable<IAnalogyDataProvider> DataProviders { get; } =
            new List<IAnalogyDataProvider> { new OfflineDataProvider() };

    }
}
