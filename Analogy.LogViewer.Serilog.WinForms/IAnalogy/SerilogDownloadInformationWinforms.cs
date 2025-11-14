using Analogy.LogViewer.Serilog.IAnalogy;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Versioning;

namespace Analogy.LogViewer.Serilog.WinForms
{
    public class SerilogDownloadInformationWinforms : Analogy.LogViewer.Template.AnalogyDownloadInformation
    {
        protected override string RepositoryURL { get; set; } = "https://api.github.com/repos/Analogy-LogViewer/Analogy.LogViewer.Serilog";
        public override TargetFrameworkAttribute CurrentFrameworkAttribute { get; set; } = (TargetFrameworkAttribute)Assembly.GetExecutingAssembly().GetCustomAttribute(typeof(TargetFrameworkAttribute));

        public override Guid FactoryId { get; set; } = SerilogPrimaryFactory.Id;
        public override string Name { get; set; } = "Serilog Data Provider";

        private string? _installedVersionNumber;
        public override string InstalledVersionNumber
        {
            get
            {
                if (_installedVersionNumber != null)
                {
                    return _installedVersionNumber;
                }
                Assembly assembly = Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                _installedVersionNumber = fvi.FileVersion;
                return _installedVersionNumber;
            }
        }
    }
}