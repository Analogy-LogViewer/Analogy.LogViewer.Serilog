using Analogy.LogViewer.Serilog.Managers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Analogy.LogViewer.Serilog
{
    public partial class SerilogUCSettings : UserControl
    {
        private SerilogSettings Settings => UserSettingsManager.UserSettings.Settings;
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
            Settings.SupportFormats = txtbSupportedFiles.Text.Split(";", StringSplitOptions.RemoveEmptyEntries).ToList();
#endif
#if !NETCOREAPP3_1
            Settings.SupportFormats = txtbSupportedFiles.Text.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();
#endif
            Settings.Directory = txtbDirectory.Text;
            Settings.FileOpenDialogFilters = txtbOpenFileFilters.Text;
            Settings.SupportFormats = txtbSupportedFiles.Text.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            Settings.IgnoredAttributes = lstbIgnoreColumn.Items.Count > 0 ? lstbIgnoreColumn.Items.Cast<string>().ToList() : new List<string>();
            if (rbtnCLEF.Checked)
                Settings.Format = SerilogFileFormat.CLEF;
            else if (rbJson.Checked)
                Settings.Format = SerilogFileFormat.JSONPerLine;
            else if (rbJsonFile.Checked)
                Settings.Format = SerilogFileFormat.JSONFile;
            UserSettingsManager.UserSettings.Save();
        }

        private void btnExportSettings_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Analogy Serilog Settings (*.serilogsettings)|*.serilogsettings";
            saveFileDialog.Title = @"Export settings";

            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                SaveSettings();
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, JsonConvert.SerializeObject(Settings));
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
            txtbDirectory.Text = logSettings.Directory;
            txtbOpenFileFilters.Text = logSettings.FileOpenDialogFilters;
            txtbSupportedFiles.Text = string.Join(";", logSettings.SupportFormats.ToList());
            lstbIgnoreColumn.Items.Clear();
            lstbIgnoreColumn.Items.AddRange(logSettings.IgnoredAttributes.ToArray());
            rbtnCLEF.Checked = logSettings.Format == SerilogFileFormat.CLEF;
            rbJson.Checked = logSettings.Format == SerilogFileFormat.JSONPerLine;
            rbJsonFile.Checked = logSettings.Format == SerilogFileFormat.JSONFile;
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
            LoadSettings(UserSettingsManager.UserSettings.Settings);
        }

        private void btnTestFilter_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog
                {
                    Filter = txtbOpenFileFilters.Text,
                    Title = @"Test Open Files",
                    Multiselect = true
                };
                openFileDialog1.ShowDialog(this);
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Incorrect filter: {exception.Message}", "Invalid filter text", MessageBoxButtons.OK);
            }
        }

        private void btnIgnoreColumn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtbIgnoreColumn.Text)) return;
            lstbIgnoreColumn.Items.Add(txtbIgnoreColumn.Text);
        }

        private void btnDeleteIgnoreColumn_Click(object sender, EventArgs e)
        {
            if (lstbIgnoreColumn.SelectedItem is string ignore)
            {
                lstbIgnoreColumn.Items.Remove(lstbIgnoreColumn.SelectedItem);
            }
        }
    }
}
