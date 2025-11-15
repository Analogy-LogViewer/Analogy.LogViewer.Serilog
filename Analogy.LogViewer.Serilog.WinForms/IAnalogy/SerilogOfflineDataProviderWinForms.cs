using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using Analogy.Interfaces.WinForms;
using Analogy.Interfaces.WinForms.DataTypes;
using Analogy.LogViewer.Serilog.DataTypes;
using Analogy.LogViewer.Serilog.IAnalogy;
using Analogy.LogViewer.Serilog.Managers;
using Analogy.LogViewer.Serilog.WinForms.Properties;
using Analogy.LogViewer.Template.WinForms;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Serilog.WinForms
{
    public class SerilogOfflineDataProviderWinForms : SerilogOfflineDataProvider, IAnalogyOfflineDataProviderWinForms
    {
        public virtual Image LargeImage { get; set; } = Resources.AnalogySerilog32x32;
        public virtual Image SmallImage { get; set; } = Resources.AnalogySerilog16x16;

        public virtual Image? GetDataProviderSmallImage() => SmallImage;

        public virtual Image? GetDataProviderLargeImage() => LargeImage;

        public virtual Image? GetDataProviderToolTipSmallImage() => SmallImage;

        public virtual Image? GetDataProviderToolTipLargeImage() => LargeImage;
    }
}