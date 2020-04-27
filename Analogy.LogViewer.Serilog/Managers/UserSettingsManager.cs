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
        public string SerilogFileSetting { get; private set; } = "AnalogySerilogSettings.json";
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
                    LogManager.Instance.LogException(ex, "Analogy Serilog Parser", "Error loading user setting file");
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
                LogManager.Instance.LogException(e, "Analogy Serilog Parser", "Error saving settings: " + e.Message);
            }


        }
    }
}
