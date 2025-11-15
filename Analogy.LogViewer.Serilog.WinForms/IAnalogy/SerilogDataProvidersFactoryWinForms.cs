using Analogy.Interfaces;
using Analogy.Interfaces.WinForms.Factories;
using Analogy.LogViewer.Serilog.IAnalogy;
using Analogy.LogViewer.Serilog.WinForms.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Analogy.LogViewer.Serilog.WinForms
{
    public class SerilogDataProvidersFactoryWinForms : SerilogDataProvidersFactory, IAnalogyDataProvidersFactoryWinForms
    {
        public override Guid FactoryId { get; set; } = SerilogPrimaryFactory.Id;
        public override string Title { get; set; } = "Serilog Parser";

        public override IEnumerable<IAnalogyDataProvider> DataProviders { get; set; } = new List<IAnalogyDataProvider> { new SerilogOfflineDataProviderWinForms() };
        public Image? GetDataFactorySmallImage(Guid componentId) => Resources.AnalogySerilog16x16;
        public Image? GetDataFacoryLargeImage(Guid componentId) => Resources.AnalogySerilog32x32;
    }
}