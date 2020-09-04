using Analogy.LogViewer.Serilog.Properties;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Analogy.Interfaces;

namespace Analogy.LogViewer.Serilog.IAnalogy
{
    public class UserSettingsFactory : IAnalogyDataProviderSettings
    {
        public string Title { get; set; } = "Serilog User Settings";
        public UserControl DataProviderSettings { get; } = new SerilogUCSettings();
        public Image SmallImage { get; set; } = Resources.AnalogySerilog16x16;
        public Image LargeImage { get; set; } = Resources.AnalogySerilog32x32;
        public Guid FactoryId { get; set; } = new Guid("513A4393-425E-4054-92D4-6A816983E51F");
        public Guid Id { get; set; } = new Guid("26FF0D4D-8FA8-46C6-A021-079E669E7EC6");

        public Task SaveSettingsAsync()
        {
            ((SerilogUCSettings)DataProviderSettings)?.SaveSettings();
            return Task.CompletedTask;
        }
    }
}
