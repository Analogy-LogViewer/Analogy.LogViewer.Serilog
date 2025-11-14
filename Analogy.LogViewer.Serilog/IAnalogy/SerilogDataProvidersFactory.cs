using Analogy.Interfaces;
using Analogy.Interfaces.WinForms;
using Analogy.LogViewer.Template.WinForms;
using System;
using System.Collections.Generic;

namespace Analogy.LogViewer.Serilog.IAnalogy
{
    public class SerilogDataProvidersFactory : DataProvidersFactoryWinForms
    {
        public override Guid FactoryId { get; set; } = SerilogPrimaryFactory.Id;
        public override string Title { get; set; } = "Serilog Parser";

        public override IEnumerable<IAnalogyDataProviderWinForms> DataProviders { get; } = new List<IAnalogyDataProviderWinForms> { new SerilogOfflineDataProvider() };
    }
}