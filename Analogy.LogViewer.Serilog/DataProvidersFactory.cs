using Analogy.Interfaces;
using Analogy.Interfaces.Factories;
using Analogy.LogViewer.Serilog.IAnalogy;
using System.Collections.Generic;

namespace Analogy.LogViewer.Serilog
{
    public class DataProvidersFactory : IAnalogyDataProvidersFactory
    {
        public string Title { get; } = "Serilog Parser";
        public IEnumerable<IAnalogyDataProvider> Items { get; }

        public DataProvidersFactory()
        {
            //get some data provider
            List<IAnalogyDataProvider> dataProviders = new List<IAnalogyDataProvider>();
            dataProviders.Add(new OfflineDataProvider());
            //dataProviders.Add(new OnlineDataProvider());
            Items = dataProviders;
        }
    }
}
