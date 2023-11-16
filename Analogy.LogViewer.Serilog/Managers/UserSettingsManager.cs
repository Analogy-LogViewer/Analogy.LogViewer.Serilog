﻿using Analogy.LogViewer.Template.Managers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Analogy.LogViewer.Serilog.Managers
{
    public class UserSettingsManager
    {
        private static readonly Lazy<UserSettingsManager> _instance =
            new Lazy<UserSettingsManager>(() => new UserSettingsManager());
        public static UserSettingsManager UserSettings { get; set; } = _instance.Value;
        private string LocalSettingFileName { get; } = "AnalogySerilogSettings.json";

        public string SerilogPerUserFileSetting => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Analogy.LogViewer", LocalSettingFileName);
        public SerilogSettings Settings { get; set; }

        public UserSettingsManager()
        {
            //check if local file exist:
            var loaded = LoadFileSettings(LocalSettingFileName, true);
            if (!loaded)
            {
                LoadFileSettings(SerilogPerUserFileSetting, false);
            }
        }

        private bool LoadFileSettings(string localSettingFileName, bool optional)
        {
            if (File.Exists(localSettingFileName))
            {
                try
                {
                    var settings = new JsonSerializerSettings
                    {
                        ObjectCreationHandling = ObjectCreationHandling.Replace,
                    };
                    string data = File.ReadAllText(localSettingFileName);
                    Settings = JsonConvert.DeserializeObject<SerilogSettings>(data, settings);
                    if (string.IsNullOrEmpty(Settings.FileOpenDialogFilters))
                    {
                        Settings.FileOpenDialogFilters = "All Supported formats (*.Clef;*.log;*.gz)|*.clef;*.log;*.gz;*.zip|Clef format (*.clef)|*.clef|Plain log text file (*.log)|*.log|GZIP file (*.gz)|*.gz|ZIP file (*.zip)|*.zip";
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    LogManager.Instance?.LogWarning($"Error loading user setting file: {ex.Message}", "Analogy Serilog Parser");
                    Settings = new SerilogSettings();
                    if (string.IsNullOrEmpty(Settings.FileOpenDialogFilters))
                    {
                        Settings.FileOpenDialogFilters = "All Supported formats (*.Clef;*.log;*.gz)|*.clef;*.log;*.gz;*.zip|Clef format (*.clef)|*.clef|Plain log text file (*.log)|*.log|GZIP file (*.gz)|*.gz|ZIP file (*.zip)|*.zip";
                    }
                    return true;
                }
            }
            else
            {
                if (!optional)
                {
                    Settings = new SerilogSettings();
                    if (string.IsNullOrEmpty(Settings.FileOpenDialogFilters))
                    {
                        Settings.FileOpenDialogFilters = "All Supported formats (*.Clef;*.log;*.gz)|*.clef;*.log;*.gz;*.zip|Clef format (*.clef)|*.clef|Plain log text file (*.log)|*.log|GZIP file (*.gz)|*.gz|ZIP file (*.zip)|*.zip";
                    }
                    return false;
                }
            }

            return false;
        }

        public void Save()
        {
            try
            {
                if (Settings.UseApplicationFolderForSettings)
                {
                    File.WriteAllText(LocalSettingFileName, JsonConvert.SerializeObject(Settings));
                }
                else
                {
                    if (File.Exists(LocalSettingFileName))
                    {
                        try
                        {
                            File.Delete(LocalSettingFileName);
                        }
                        catch (Exception e)
                        {
                            LogManager.Instance.LogError($"Error deleting local file: {e.Message}");
                        }
                    }
                    File.WriteAllText(SerilogPerUserFileSetting, JsonConvert.SerializeObject(Settings));
                }
            }
            catch (Exception e)
            {
                LogManager.Instance.LogError(e, "Error saving settings: " + e.Message);
            }
        }
    }
}