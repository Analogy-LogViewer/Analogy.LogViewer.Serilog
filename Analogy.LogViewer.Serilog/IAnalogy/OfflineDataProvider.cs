using Analogy.Interfaces;
using Analogy.LogViewer.Serilog.Managers;
using Analogy.LogViewer.Serilog.Regex;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Serilog.IAnalogy
{
    public class OfflineDataProvider : IAnalogyOfflineDataProvider
    {
        public Guid ID { get; } = new Guid("D89318C6-306A-48D9-90A0-7C2C49EFDA82");
        public string OptionalTitle { get; } = "Serilog offline reader";
        public bool CanSaveToLogFile { get; } = false;
        public string FileOpenDialogFilters { get; } = "Serilog CLEF log files|*.clef";
        public string FileSaveDialogFilters { get; } = string.Empty;
        public IEnumerable<string> SupportFormats { get; } = new[] { "*.clef" };
        public bool DisableFilePoolingOption { get; } = false;
        public string InitialFolderFullPath { get; } = Environment.CurrentDirectory;
        private ClefParser ClefParser { get; }
        //private JsonParser JsonParser { get; }
        public bool UseCustomColors { get; set; } = false;
        public IEnumerable<(string originalHeader, string replacementHeader)> GetReplacementHeaders()
            => Array.Empty<(string, string)>();

        public (Color backgroundColor, Color foregroundColor) GetColorForMessage(IAnalogyLogMessage logMessage)
            => (Color.Empty, Color.Empty);
        public OfflineDataProvider()
        {
            ClefParser = new ClefParser();

        }
        public async Task<IEnumerable<AnalogyLogMessage>> Process(string fileName, CancellationToken token, ILogMessageCreatedHandler messagesHandler)
        {
            if (CanOpenFile(fileName))
            {
                if (fileName.EndsWith(".clef"))
                    return await ClefParser.Process(fileName, token, messagesHandler);
                if (fileName.EndsWith(".log"))
                    return await new RegexParser(UserSettingsManager.UserSettings.Settings.RegexPatterns, true,
                        LogManager.Instance).ParseLog(fileName, token, messagesHandler);
            }
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
            return fileName.EndsWith(".Clef", StringComparison.InvariantCultureIgnoreCase) ||
                   fileName.EndsWith(".log", StringComparison.InvariantCultureIgnoreCase);
        }

        public bool CanOpenAllFiles(IEnumerable<string> fileNames) => fileNames.All(CanOpenFile);


        public Task InitializeDataProviderAsync(IAnalogyLogger logger)
        {
            LogManager.Instance.SetLogger(logger);
            return Task.CompletedTask;

        }

        public void MessageOpened(AnalogyLogMessage message)
        {
            //nop
        }

        public static List<FileInfo> GetSupportedFilesInternal(DirectoryInfo dirInfo, bool recursive)
        {
            List<FileInfo> files = dirInfo.GetFiles("*.clef").Concat(dirInfo.GetFiles("*.log")).ToList();
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
