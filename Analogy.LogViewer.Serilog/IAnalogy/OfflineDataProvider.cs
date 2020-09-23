using Analogy.Interfaces;
using Analogy.LogViewer.Serilog.DataTypes;
using Analogy.LogViewer.Serilog.Managers;
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
        public Guid Id { get; set; } = new Guid("D89318C6-306A-48D9-90A0-7C2C49EFDA82");
        public Image LargeImage { get; set; } = null;
        public Image SmallImage { get; set; } = null;

        public string OptionalTitle { get; set; } = "Serilog offline reader";
        public bool CanSaveToLogFile { get; } = false;
        public string FileOpenDialogFilters => UserSettingsManager.UserSettings.Settings.FileOpenDialogFilters;
        public string FileSaveDialogFilters { get; } = string.Empty;
        public IEnumerable<string> SupportFormats => UserSettingsManager.UserSettings.Settings.SupportFormats;
        public bool DisableFilePoolingOption { get; } = false;

        public string InitialFolderFullPath =>
            (!string.IsNullOrEmpty(UserSettingsManager.UserSettings.Settings.Directory) &&
             Directory.Exists(UserSettingsManager.UserSettings.Settings.Directory))
                ? UserSettingsManager.UserSettings.Settings.Directory
                : Environment.CurrentDirectory;
        private CompactJsonFormatParser ClefParser { get; }
        private JsonFormatterParser JsonParser { get; }
        private JsonFileParser JsonFileParser { get; }
        private JsonFormatterParser JsonFormatterParser { get; }

        public bool UseCustomColors { get; set; } = false;
        public IEnumerable<(string originalHeader, string replacementHeader)> GetReplacementHeaders()
            => Array.Empty<(string, string)>();

        public (Color backgroundColor, Color foregroundColor) GetColorForMessage(IAnalogyLogMessage logMessage)
            => (Color.Empty, Color.Empty);
        public OfflineDataProvider()
        {
            ClefParser = new CompactJsonFormatParser();
            JsonParser = new JsonFormatterParser();
            JsonFileParser = new JsonFileParser(new CompactJsonFormatMessageFields());
            JsonFormatterParser = new JsonFormatterParser();

        }
        public async Task<IEnumerable<AnalogyLogMessage>> Process(string fileName, CancellationToken token, ILogMessageCreatedHandler messagesHandler)
        {
            if (CanOpenFile(fileName))
            {
                switch (UserSettingsManager.UserSettings.Settings.Format)
                {
                    case SerilogFileFormat.CLEF:
                        return await ClefParser.Process(fileName, token, messagesHandler);
                    case SerilogFileFormat.JSONFile:
                        return await JsonFileParser.Process(fileName, token, messagesHandler);
                    case SerilogFileFormat.JSONPerLine:
                        return await JsonFormatterParser.Process(fileName, token, messagesHandler);
                }
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
            foreach (string pattern in UserSettingsManager.UserSettings.Settings.SupportFormats)
            {
                if (CommonUtilities.Files.FilesPatternMatcher.StrictMatchPattern(pattern, fileName))
                    return true;
            }
            return false;
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
            List<FileInfo> files = new List<FileInfo>();
            foreach (string pattern in UserSettingsManager.UserSettings.Settings.SupportFormats)
            {
                files.AddRange(dirInfo.GetFiles(pattern).ToList());
            }

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
