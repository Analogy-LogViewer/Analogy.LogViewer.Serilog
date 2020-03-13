using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Analogy.LogViewer.Serilog.Managers;
using Analogy.LogViewer.Serilog.Properties;

namespace Analogy.LogViewer.Serilog.IAnalogy
{
   public class UserSettingsFactory:Analogy.DataProviders.Extensions.IAnalogyDataProviderSettings
   {
       public string Title { get; } = "Serilog User Settings";
        public UserControl DataProviderSettings { get; }=new SerilogUCSettings();
        public Image Icon { get; } = Resources.AnalogySerilog16x16;

        public Task SaveSettingsAsync()
        {
            ((SerilogUCSettings) DataProviderSettings)?.SaveSettings();
            return Task.CompletedTask;
        }
        }
}
