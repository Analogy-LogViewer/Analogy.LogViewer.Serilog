using Analogy.Interfaces;
using Analogy.Interfaces.Winforms;
using System;
using System.Collections.Generic;

namespace Analogy.LogViewer.Serilog.IAnalogy
{
    public class DataProvidersFactory : Analogy.LogViewer.Template.DataProvidersFactoryWinforms
    {
        public override Guid FactoryId { get; set; } = PrimaryFactory.Id;
        public override string Title { get; set; } = "Serilog Parser";

        public override IEnumerable<IAnalogyDataProviderWinforms> DataProviders { get; set; } =
            new List<IAnalogyDataProviderWinforms> { new OfflineDataProvider() };
    }
}