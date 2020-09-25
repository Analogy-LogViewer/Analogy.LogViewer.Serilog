using Analogy.Interfaces;
using Analogy.LogViewer.Serilog.DataTypes;
using Analogy.LogViewer.Serilog.Managers;
using Newtonsoft.Json;
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
        private CompactJsonFormatParser CompactFormatPerLineParser { get; }
        private JsonFormatterParser JsonPerLineParser { get; }
        private JsonFileParser CompactJsonFileParser { get; }
        private JsonFileParser JsonFileParser { get; }

        public bool UseCustomColors { get; set; } = false;
        public IEnumerable<(string originalHeader, string replacementHeader)> GetReplacementHeaders()
            => Array.Empty<(string, string)>();

        public (Color backgroundColor, Color foregroundColor) GetColorForMessage(IAnalogyLogMessage logMessage)
            => (Color.Empty, Color.Empty);
        public OfflineDataProvider()
        {
            CompactFormatPerLineParser = new CompactJsonFormatParser();
            JsonPerLineParser = new JsonFormatterParser();
            JsonFileParser = new JsonFileParser(new JsonFormatMessageFields());
            CompactJsonFileParser = new JsonFileParser(new CompactJsonFormatMessageFields());

        }
        public async Task<IEnumerable<AnalogyLogMessage>> Process(string fileName, CancellationToken token, ILogMessageCreatedHandler messagesHandler)
        {
            if (CanOpenFile(fileName))
            {
                if (UserSettingsManager.UserSettings.Settings.Format == FileFormat.Unknown)
                {
                    UserSettingsManager.UserSettings.Settings.Format = TryDetectFormat(fileName);
                }
                switch (UserSettingsManager.UserSettings.Settings.Format)
                {
                    case FileFormat.CompactJsonFormatPerLine:
                        return await CompactFormatPerLineParser.Process(fileName, token, messagesHandler);
                    case FileFormat.CompactJsonFormatPerFile:
                        return await CompactJsonFileParser.Process(fileName, token, messagesHandler);
                    case FileFormat.JsonFormatFile:
                        return await JsonFileParser.Process(fileName, token, messagesHandler);
                    case FileFormat.JsonFormatPerLine:
                        return await JsonPerLineParser.Process(fileName, token, messagesHandler);
                }
            }
            LogManager.Instance.LogError($"Unsupported File {fileName}", nameof(OfflineDataProvider));
            return new List<AnalogyLogMessage>(0);
        }

        public static FileFormat TryDetectFormat(string fileName)
        {
            var format = TryParseAsFile(fileName);
            if (format == FileFormat.Unknown)
                format = TryParsePerLine(fileName);
            return format;
        }

        private static FileFormat TryParsePerLine(string fileName)
        {
            try
            {
                var jsonData = File.ReadLines(fileName).First();
                IMessageFields fields = new JsonFormatMessageFields();
                if (jsonData.Contains(fields.Timestamp) && jsonData.Contains(fields.MessageTemplate))
                    return FileFormat.JsonFormatPerLine;
                fields = new CompactJsonFormatMessageFields();
                if (jsonData.Contains(fields.Timestamp) && jsonData.Contains(fields.MessageTemplate))
                    return FileFormat.CompactJsonFormatPerLine;
                return FileFormat.Unknown;
            }
            catch (Exception)
            {
                return FileFormat.Unknown;
            }
        }

        private static FileFormat TryParseAsFile(string fileName)
        {
            try
            {
                var jsonData = File.ReadAllText(fileName);
                var jsonObject = JsonConvert.DeserializeObject(jsonData);
                IMessageFields fields = new JsonFormatMessageFields();
                if (jsonData.Contains(fields.Timestamp) && jsonData.Contains(fields.MessageTemplate))
                    return FileFormat.JsonFormatFile;
                fields = new CompactJsonFormatMessageFields();
                if (jsonData.Contains(fields.Timestamp) && jsonData.Contains(fields.MessageTemplate))
                    return FileFormat.CompactJsonFormatPerFile;
                return FileFormat.Unknown;
            }
            catch (Exception)
            {
                return FileFormat.Unknown;
            }
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
