using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Analogy.DataProviders.Extensions;
using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using Newtonsoft.Json;

namespace Analogy.LogViewer.NLogProvider
{
    public partial class NLogSettings : UserControl
    {
        private ILogParserSettings LogParsersSettings => UserSettingsManager.UserSettings.LogParserSettings;
        public NLogSettings()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveMapping();
        }
        private void SaveMapping()
        {
            LogParsersSettings.Configure(txtNLogLayout.Text, txtNLogSeperator.Text,
                new List<string> { txtNLogExtension.Text }, analogyColumnsMatcherUC1.Mapping);
            LogParsersSettings.Directory = txtbNLogDirectory.Text;
        }

        private void btnExportSettings_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Analogy NLog Settings (*.nlogsettings)|*.nlogsettings";
            saveFileDialog.Title = @"Export NLog settings";

            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                SaveMapping();
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

        private void btnLoadLayout_Click(object sender, EventArgs e)
        {
            CheckNLogLayout();
        }

        private void CheckNLogLayout()
        {
            try
            {
                if (string.IsNullOrEmpty(txtNLogSeperator.Text)) return;
                var items = txtNLogLayout.Text
                    .Split(txtNLogSeperator.Text.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                    .ToArray();
                analogyColumnsMatcherUC1.SetColumns(items);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error parsing input: " + exception.Message, "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Analogy NLog Settings (*.nlogsettings)|*.nlogsettings";
            openFileDialog1.Title = @"Import NLog settings";
            openFileDialog1.Multiselect = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var json = File.ReadAllText(openFileDialog1.FileName);
                    LogParserSettings nlog = JsonConvert.DeserializeObject<LogParserSettings>(json);
                    LoadNLogSettings(nlog);
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
        private void LoadNLogSettings(ILogParserSettings nLogParserSettings)
        {
            if (nLogParserSettings.IsConfigured)
            {
                txtNLogSeperator.Text = nLogParserSettings.Splitter;
                txtNLogLayout.Text = nLogParserSettings.Layout;
                txtNLogExtension.Text = string.Join(";", nLogParserSettings.SupportedFilesExtensions);

                analogyColumnsMatcherUC1.LoadMapping(nLogParserSettings);
                CheckNLogLayout();
            }
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    txtbNLogDirectory.Text = fbd.SelectedPath;

                }
            }
        }

        private void NLogSettings_Load(object sender, EventArgs e)
        {
            LoadNLogSettings(UserSettingsManager.UserSettings.LogParserSettings);
        }
    }
}
