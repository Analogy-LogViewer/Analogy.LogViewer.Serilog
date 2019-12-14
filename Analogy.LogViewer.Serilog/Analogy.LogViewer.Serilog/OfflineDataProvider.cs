using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Analogy.Interfaces;

namespace Analogy.LogViewer.Serilog
{
    public class OfflineDataProvider : IAnalogyOfflineDataProvider
    {

        public Guid ID { get; } = new Guid("D89318C6-306A-48D9-90A0-7C2C49EFDA82");
        public string OptionalTitle { get; } = "Serilog offline reader";
        public bool CanSaveToLogFile { get; } = false;
        public string FileOpenDialogFilters { get; } = "Serilog CLEF log files|*.clef";
        public string FileSaveDialogFilters { get; } = string.Empty;
        public IEnumerable<string> SupportFormats { get; } = new[] { "*.clef" };
        public string InitialFolderFullPath { get; } = Environment.CurrentDirectory;
        private ClefParser ClefParser { get; }

        public OfflineDataProvider()
        {
            ClefParser = new ClefParser();
        }
        public async Task<IEnumerable<AnalogyLogMessage>> Process(string fileName, CancellationToken token, ILogMessageCreatedHandler messagesHandler)
        {
            if (CanOpenFile(fileName))
                return await ClefParser.Process(fileName, token, messagesHandler);
            return new List<AnalogyLogMessage>(0);
        }

        public IEnumerable<FileInfo> GetSupportedFiles(DirectoryInfo dirInfo, bool recursiveLoad)
            => GetSupportedFilesInternal(dirInfo, recursiveLoad);

        public Task SaveAsync(List<AnalogyLogMessage> messages, string fileName)
        {
            throw new NotImplementedException();
        }

        public bool CanOpenFile(string fileName)
        {
            return fileName.EndsWith(".Clef", StringComparison.InvariantCultureIgnoreCase);
        }

        public bool CanOpenAllFiles(IEnumerable<string> fileNames)=> fileNames.All(CanOpenFile);

        public Task InitializeDataProviderAsync(IAnalogyLogger logger)
        {
            return Task.CompletedTask;
            //nop
        }

        public void MessageOpened(AnalogyLogMessage message)
        {
            //nop
        }

        public static List<FileInfo> GetSupportedFilesInternal(DirectoryInfo dirInfo, bool recursive)
        {
            List<FileInfo> files = dirInfo.GetFiles("*.clef").ToList();
            if (!recursive)
                return files;
            try
            {
                foreach (DirectoryInfo dir in dirInfo.GetDirectories())
                {
                    files.AddRange(GetSupportedFilesInternal(dir, true));
                }
            }
            catch (Exception)
            {
                return files;
            }

            return files;
        }
    }
}
