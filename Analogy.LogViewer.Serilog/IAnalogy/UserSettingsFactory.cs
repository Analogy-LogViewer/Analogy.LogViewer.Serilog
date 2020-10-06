using Analogy.LogViewer.Serilog.Properties;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Analogy.LogViewer.Serilog.IAnalogy
{
    public class UserSettingsFactory : Analogy.LogViewer.Template.UserSettingsFactory
    {
        public override string Title { get; set; } = "Serilog User Settings";
        public override UserControl DataProviderSettings { get; set; } = new SerilogUCSettings();
        public override Image SmallImage { get; set; } = Resources.AnalogySerilog16x16;
        public override Image LargeImage { get; set; } = Resources.AnalogySerilog32x32;
        public override Guid FactoryId { get; set; } = PrimaryFactory.Id;
        public override Guid Id { get; set; } = new Guid("26FF0D4D-8FA8-46C6-A021-079E669E7EC6");

        public override Task SaveSettingsAsync()
        {
            ((SerilogUCSettings)DataProviderSettings)?.SaveSettings();
            return Task.CompletedTask;
        }
    }
}
