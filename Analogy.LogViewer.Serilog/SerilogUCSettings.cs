using Analogy.Interfaces;
using Analogy.LogViewer.Serilog.Managers;
using Analogy.LogViewer.Serilog.Regex;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Analogy.LogViewer.Serilog
{
    public partial class SerilogUCSettings : UserControl
    {
        private SerilogSettings LogParsersSettings => UserSettingsManager.UserSettings.LogParserSettings;
        public SerilogUCSettings()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }
        public void SaveSettings()
        {
#if NETCOREAPP3_1
            LogParsersSettings.SupportFormats = txtNLogExtension.Text.Split(";",StringSplitOptions.RemoveEmptyEntries).ToList();
#endif
#if !NETCOREAPP3_1
            LogParsersSettings.SupportFormats = txtNLogExtension.Text.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();
#endif
            LogParsersSettings.Directory = txtbDirectory.Text;
            UserSettingsManager.UserSettings.Save();
        }

        private void btnExportSettings_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Analogy NLog Settings (*.nlogsettings)|*.nlogsettings";
            saveFileDialog.Title = @"Export NLog settings";

            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                SaveSettings();
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, JsonConvert.SerializeObject(LogParsersSettings));
                    MessageBox.Show("File Saved", @"Export settings", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Export: " + ex.Message, @"Error Saving file", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }

            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Analogy Serilog Settings (*.Json)|*.json";
            openFileDialog1.Title = @"Import Serilog settings";
            openFileDialog1.Multiselect = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var json = File.ReadAllText(openFileDialog1.FileName);
                    SerilogSettings settings = JsonConvert.DeserializeObject<SerilogSettings>(json);
                    LoadSettings(settings);
                    MessageBox.Show("File Imported. Save settings if desired", @"Import settings", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Import: " + ex.Message, @"Error Import file", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void LoadSettings(SerilogSettings logSettings)
        {
            txtLogsLocation.Text = Settings.LogsLocation;
            txtbOpenFileFilters.Text = Settings.FileOpenDialogFilters;
            txtbSupportedFiles.Text = string.Join(";", Settings.SupportFormats.ToList());
            lstbRegularExpressions.Items.AddRange(Settings.RegexPatterns.ToArray());
            txtbDateTimeFormat.Text = Settings.RegexPatterns.First().DateTimeFormat;
            txtNLogExtension.Text = string.Join(";", logSettings.SupportFormats);
            rbtnCLEF.Checked = true;
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    txtbDirectory.Text = fbd.SelectedPath;
                }
            }
        }

        private void NLogSettings_Load(object sender, EventArgs e)
        {
            LoadSettings(UserSettingsManager.UserSettings.LogParserSettings);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtbRegEx.Text)) return;
            var rp = new RegexPattern(txtbRegEx.Text, txtbDateTimeFormat.Text, txtbGuidFormat.Text);
            lstbRegularExpressions.Items.Add(rp);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstbRegularExpressions.SelectedItem is RegexPattern regexPattern)
            {
                lstbRegularExpressions.Items.Remove(lstbRegularExpressions.SelectedItem);
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            RegexPattern p = new RegexPattern(txtbRegEx.Text, txtbDateTimeFormat.Text, "");
            bool valid = RegexParser.CheckRegex(txtbTest.Text, p, out AnalogyLogMessage m);
            if (valid)
            {
                lblResult.Text = "Valid Regular Expression";
                lblResult.BackColor = Color.GreenYellow;
                lblResultMessage.Text = m.ToString();
            }
            else
            {
                lblResult.Text = "Non Valid Regular Expression";
                lblResult.BackColor = Color.OrangeRed;
            }
        }
    }
}
