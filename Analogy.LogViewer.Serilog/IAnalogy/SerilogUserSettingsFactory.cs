using Analogy.Interfaces;
using Analogy.LogViewer.Serilog.Properties;
using Analogy.LogViewer.Template.WinForms;
using Microsoft.Extensions.Logging;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Analogy.LogViewer.Serilog.IAnalogy
{
    public class SerilogUserSettingsFactory : TemplateUserSettingsFactoryWinForms
    {
        public override string Title { get; set; } = "Serilog User Settings";
        public override UserControl DataProviderSettings { get; set; }
        public override Image SmallImage { get; set; } = Resources.AnalogySerilog16x16;
        public override Image LargeImage { get; set; } = Resources.AnalogySerilog32x32;
        public override Guid FactoryId { get; set; } = SerilogPrimaryFactory.Id;
        public override Guid Id { get; set; } = new Guid("26FF0D4D-8FA8-46C6-A021-079E669E7EC6");

        public override void CreateUserControl(ILogger logger)
        {
            DataProviderSettings = new SerilogUCSettings();
        }

        public override Task SaveSettingsAsync()
        {
            ((SerilogUCSettings)DataProviderSettings)?.SaveSettings();
            return Task.CompletedTask;
        }
    }
}