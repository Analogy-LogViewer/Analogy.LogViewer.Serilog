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
        public string SerilogFileSetting { get; private set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Analogy.LogViewer", "AnalogySerilogSettings.json");
        public SerilogSettings Settings { get; set; }


        public UserSettingsManager()
        {
            if (File.Exists(SerilogFileSetting))
            {
                try
                {
                    var settings = new JsonSerializerSettings
                    {
                        ObjectCreationHandling = ObjectCreationHandling.Replace
                    };
                    string data = File.ReadAllText(SerilogFileSetting);
                    Settings = JsonConvert.DeserializeObject<SerilogSettings>(data, settings);
                }
                catch (Exception ex)
                {
                    LogManager.Instance.LogException("Error loading user setting file",ex, "Analogy Serilog Parser");
                    Settings = new SerilogSettings();

                }
            }
            else
            {
                Settings = new SerilogSettings();
            }

        }

        public void Save()
        {
            try
            {
                File.WriteAllText(SerilogFileSetting, JsonConvert.SerializeObject(Settings));
            }
            catch (Exception e)
            {
                LogManager.Instance.LogException("Error saving settings: " + e.Message,e, "Analogy Serilog Parser" );
            }


        }
    }
}
