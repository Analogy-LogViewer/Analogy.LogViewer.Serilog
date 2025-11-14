using Analogy.Interfaces;
using Analogy.LogViewer.Template;
using Microsoft.Extensions.Logging;
using System;
using System.Drawing;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Serilog.IAnalogy
{
    public class SerilogUserSettingsFactory : TemplateUserSettingsFactory
    {
        public override string Title { get; set; } = "Serilog User Settings";
        public override Guid FactoryId { get; set; } = SerilogPrimaryFactory.Id;
        public override Guid Id { get; set; } = new Guid("26FF0D4D-8FA8-46C6-A021-079E669E7EC6");

        public override void CreateUserControl(ILogger logger)
        {
        }

        public override Task SaveSettingsAsync()
        {
            return Task.CompletedTask;
        }
    }
}