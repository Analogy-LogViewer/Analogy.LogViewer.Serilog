using Analogy.Interfaces.WinForms;
using Analogy.Interfaces.WinForms.Factories;
using Analogy.LogViewer.Serilog.IAnalogy;
using System;
using System.Collections.Generic;

namespace Analogy.LogViewer.Serilog.WinForms
{
    public class SerilogDataProvidersFactoryWinForms : SerilogDataProvidersFactory, IAnalogyDataProvidersFactoryWinForms
    {
        public override Guid FactoryId { get; set; } = SerilogPrimaryFactory.Id;
        public override string Title { get; set; } = "Serilog Parser";

        public new IEnumerable<IAnalogyDataProviderWinForms> DataProviders { get; set; } = new List<IAnalogyDataProviderWinForms> { new SerilogOfflineDataProviderWinForms() };
    }
}