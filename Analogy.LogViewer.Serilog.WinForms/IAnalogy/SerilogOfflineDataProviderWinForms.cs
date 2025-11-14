using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using Analogy.Interfaces.WinForms;
using Analogy.Interfaces.WinForms.DataTypes;
using Analogy.LogViewer.Serilog.DataTypes;
using Analogy.LogViewer.Serilog.IAnalogy;
using Analogy.LogViewer.Serilog.Managers;
using Analogy.LogViewer.Template.WinForms;
using Analogy.LogViewer.Template.WinForms.Properties;
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
        public Image LargeImage { get; set; } = Resources.Analogy32x32;
        public Image SmallImage { get; set; } = Resources.Analogy32x32;
        
        public AnalogyToolTipWinForms? ToolTip { get; set; }
    }
}