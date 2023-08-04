using Analogy.Interfaces;
using Analogy.LogViewer.Serilog.DataTypes;
using Analogy.LogViewer.Serilog.Managers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Serilog.IAnalogy
{
    public class OfflineDataProvider : Analogy.LogViewer.Template.OfflineDataProvider
    {
        public override Guid Id { get; set; } = new Guid("D89318C6-306A-48D9-90A0-7C2C49EFDA82");
        public override Image LargeImage { get; set; }
        public override Image SmallImage { get; set; }
        public override string OptionalTitle { get; set; } = "Serilog offline reader";
        public override bool CanSaveToLogFile { get; set; }
        public override string FileOpenDialogFilters { get; set; } = UserSettingsManager.UserSettings.Settings.FileOpenDialogFilters;
        public override string FileSaveDialogFilters { get; set; } = string.Empty;
        public override IEnumerable<string> SupportFormats { get; set; } = UserSettingsManager.UserSettings.Settings.SupportFormats;
        public override bool DisableFilePoolingOption { get; set; }

        public override string InitialFolderFullPath =>
            (!string.IsNullOrEmpty(UserSettingsManager.UserSettings.Settings.Directory) &&
             Directory.Exists(UserSettingsManager.UserSettings.Settings.Directory))
                ? UserSettingsManager.UserSettings.Settings.Directory
                : Environment.CurrentDirectory;
        private CompactJsonFormatParser CompactFormatPerLineParser { get; }
        private JsonFormatterParser JsonPerLineParser { get; }
        private JsonFileParser CompactJsonFileParser { get; }
        private JsonFileParser JsonFileParser { get; }

        public override bool UseCustomColors { get; set; }
        public override IEnumerable<(string originalHeader, string replacementHeader)> GetReplacementHeaders()
            => Array.Empty<(string, string)>();

        public override (Color backgroundColor, Color foregroundColor) GetColorForMessage(IAnalogyLogMessage logMessage)
            => (Color.Empty, Color.Empty);
        public OfflineDataProvider()
        {
            CompactFormatPerLineParser = new CompactJsonFormatParser();
            CompactJsonFileParser = new JsonFileParser(new CompactJsonFormatMessageFields());

            JsonPerLineParser = new JsonFormatterParser(new JsonFormatMessageFields());
            JsonFileParser = new JsonFileParser(new JsonFormatMessageFields());

        }
        public override Task InitializeDataProvider(ILogger logger)
        {
            return base.InitializeDataProvider(logger);
        }

        public override async Task<IEnumerable<IAnalogyLogMessage>> Process(string fileName, CancellationToken token, ILogMessageCreatedHandler messagesHandler)
        {
            if (CanOpenFile(fileName))
            {
                if (UserSettingsManager.UserSettings.Settings.FileFormatDetection == FileFormatDetection.Automatic ||
                    UserSettingsManager.UserSettings.Settings.Format == FileFormat.Unknown)
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
                    case FileFormat.Unknown:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"Invalid format {UserSettingsManager.UserSettings.Settings.Format}");
                }
            }
            Template.Managers.LogManager.Instance?.LogError($"Unsupported File {fileName}", nameof(OfflineDataProvider));
            return new List<AnalogyLogMessage>(0);
        }

        public override bool CanOpenFile(string fileName)
        {
            foreach (string pattern in UserSettingsManager.UserSettings.Settings.SupportFormats)
            {
                if (CommonUtilities.Files.FilesPatternMatcher.StrictMatchPattern(pattern, fileName))
                {
                    return true;
                }
            }
            return false;
        }

        protected override List<FileInfo> GetSupportedFilesInternal(DirectoryInfo dirInfo, bool recursive)
        {
            List<FileInfo> files = new List<FileInfo>();
            foreach (string pattern in UserSettingsManager.UserSettings.Settings.SupportFormats)
            {
                files.AddRange(dirInfo.GetFiles(pattern).ToList());
            }

            if (!recursive)
            {
                return files;
            }

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

        public static FileFormat TryDetectFormat(string fileName)
        {
            string jsonData = string.Empty;
            if (fileName.EndsWith(".gz", StringComparison.InvariantCultureIgnoreCase))
            {
                using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var gzStream = new GZipStream(fileStream, CompressionMode.Decompress))
                    {
                        using (var streamReader = new StreamReader(gzStream, encoding: Encoding.UTF8))
                        {
                            jsonData = streamReader.ReadToEnd();
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(jsonData))
            {
                jsonData = SafeReadAllLines(fileName);
            }

            var format = TryParseAsFile(jsonData);
            if (format == FileFormat.Unknown)
            {
                format = TryParsePerLine(jsonData);
            }

            return format;
        }
        private static FileFormat TryParsePerLine(string jsonData)
        {
            try
            {
                IMessageFields fields = new JsonFormatMessageFields();
                if (jsonData.Contains(fields.Timestamp) && jsonData.Contains(fields.MessageTemplate))
                {
                    return FileFormat.JsonFormatPerLine;
                }

                fields = new CompactJsonFormatMessageFields();
                if (jsonData.Contains(fields.Timestamp) && jsonData.Contains(fields.MessageTemplate))
                {
                    return FileFormat.CompactJsonFormatPerLine;
                }

                return FileFormat.Unknown;
            }
            catch (Exception)
            {
                return FileFormat.Unknown;
            }
        }

        private static FileFormat TryParseAsFile(string jsonData)
        {
            try
            {
                var jsonObject = JsonConvert.DeserializeObject(jsonData);
                IMessageFields fields = new JsonFormatMessageFields();
                if (jsonData.Contains(fields.Timestamp) && jsonData.Contains(fields.MessageTemplate))
                {
                    return FileFormat.JsonFormatFile;
                }

                fields = new CompactJsonFormatMessageFields();
                if (jsonData.Contains(fields.Timestamp) && jsonData.Contains(fields.MessageTemplate))
                {
                    return FileFormat.CompactJsonFormatPerFile;
                }

                return FileFormat.Unknown;
            }
            catch (Exception)
            {
                return FileFormat.Unknown;
            }
        }

        private static string SafeReadAllLines(string path)
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(stream))
            {
                string data = sr.ReadToEnd();
                return data;
            }
        }
    }
}
