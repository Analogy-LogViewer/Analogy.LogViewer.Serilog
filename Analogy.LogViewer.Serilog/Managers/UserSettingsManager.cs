using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Analogy.LogViewer.Serilog.Managers
{
    public class UserSettingsManager
    {

        private static readonly Lazy<UserSettingsManager> _instance =
            new Lazy<UserSettingsManager>(() => new UserSettingsManager());
        public static UserSettingsManager UserSettings { get; set; } = _instance.Value;
        public string SerilogFileSetting { get; private set; } = "AnalogySerilogSettings.json";
        public SerilogSettings LogParserSettings { get; set; }


        public UserSettingsManager()
        {
            if (File.Exists(SerilogFileSetting))
            {
                try
                {
                    string data = File.ReadAllText(SerilogFileSetting);
                    LogParserSettings = JsonConvert.DeserializeObject<SerilogSettings>(data);
                }
                catch (Exception ex)
                {
                    LogManager.Instance.LogException(ex, "Analogy Serilog Parser", "Error loading user setting file");
                    LogParserSettings = new SerilogSettings();
                   
                }
            }
            else
            {
                LogParserSettings = new SerilogSettings();
            }

        }

        public void Save()
        {
            try
            {
                File.WriteAllText(SerilogFileSetting, JsonConvert.SerializeObject(LogParserSettings));
            }
            catch (Exception e)
            {
                LogManager.Instance.LogException(e, "Analogy Serilog Parser", "Error saving settings: " + e.Message);
            }


        }
    }
}
