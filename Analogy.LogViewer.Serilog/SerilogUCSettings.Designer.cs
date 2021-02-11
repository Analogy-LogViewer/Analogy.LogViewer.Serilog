namespace Analogy.LogViewer.Serilog
{
    partial class SerilogUCSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSave = new System.Windows.Forms.Button();
            this.btnExportSettings = new System.Windows.Forms.Button();
            this.txtbSupportedFiles = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnImport = new System.Windows.Forms.Button();
            this.txtbDirectory = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOpenFolder = new System.Windows.Forms.Button();
            this.rbtnCLEF = new System.Windows.Forms.RadioButton();
            this.rbtnJsonPerLine = new System.Windows.Forms.RadioButton();
            this.txtbOpenFileFilters = new System.Windows.Forms.TextBox();
            this.lblOpenfilesFilters = new System.Windows.Forms.Label();
            this.btnTestFilter = new System.Windows.Forms.Button();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.rbDetectionModeManual = new System.Windows.Forms.RadioButton();
            this.rbDetectionModeAutomatic = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbtnReset = new System.Windows.Forms.RadioButton();
            this.rbtnJsonFile = new System.Windows.Forms.RadioButton();
            this.rtxtExample = new System.Windows.Forms.RichTextBox();
            this.rbtnCompactJsonFile = new System.Windows.Forms.RadioButton();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.txtbIgnoreColumn = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnDeleteIgnoreColumn = new System.Windows.Forms.Button();
            this.btnIgnoreColumn = new System.Windows.Forms.Button();
            this.lstbIgnoreColumn = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbtnPerUser = new System.Windows.Forms.RadioButton();
            this.rbtnApplicationFolder = new System.Windows.Forms.RadioButton();
            this.tabControlMain.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(579, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(133, 47);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save Settings";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnExportSettings
            // 
            this.btnExportSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportSettings.Location = new System.Drawing.Point(440, 6);
            this.btnExportSettings.Name = "btnExportSettings";
            this.btnExportSettings.Size = new System.Drawing.Size(133, 47);
            this.btnExportSettings.TabIndex = 2;
            this.btnExportSettings.Text = "Export Settings";
            this.btnExportSettings.UseVisualStyleBackColor = true;
            this.btnExportSettings.Click += new System.EventHandler(this.btnExportSettings_Click);
            // 
            // txtbSupportedFiles
            // 
            this.txtbSupportedFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtbSupportedFiles.Location = new System.Drawing.Point(276, 67);
            this.txtbSupportedFiles.Name = "txtbSupportedFiles";
            this.txtbSupportedFiles.Size = new System.Drawing.Size(437, 26);
            this.txtbSupportedFiles.TabIndex = 9;
            this.txtbSupportedFiles.Text = "*.Clef";
            // 
            // label2
            // 
            this.label2.AutoEllipsis = true;
            this.label2.Location = new System.Drawing.Point(9, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(261, 50);
            this.label2.TabIndex = 8;
            this.label2.Text = "Supported Log Formats (use ; as separator):";
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Location = new System.Drawing.Point(297, 6);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(137, 47);
            this.btnImport.TabIndex = 10;
            this.btnImport.Text = "Import settings";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // txtbDirectory
            // 
            this.txtbDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtbDirectory.Location = new System.Drawing.Point(276, 3);
            this.txtbDirectory.Name = "txtbDirectory";
            this.txtbDirectory.Size = new System.Drawing.Size(376, 26);
            this.txtbDirectory.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(152, 18);
            this.label3.TabIndex = 11;
            this.label3.Text = "Default Logs Location:";
            // 
            // btnOpenFolder
            // 
            this.btnOpenFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenFolder.Location = new System.Drawing.Point(658, 3);
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.btnOpenFolder.Size = new System.Drawing.Size(55, 26);
            this.btnOpenFolder.TabIndex = 13;
            this.btnOpenFolder.Text = "...";
            this.btnOpenFolder.UseVisualStyleBackColor = true;
            this.btnOpenFolder.Click += new System.EventHandler(this.btnOpenFolder_Click);
            // 
            // rbtnCLEF
            // 
            this.rbtnCLEF.AutoSize = true;
            this.rbtnCLEF.Checked = true;
            this.rbtnCLEF.Location = new System.Drawing.Point(6, 25);
            this.rbtnCLEF.Name = "rbtnCLEF";
            this.rbtnCLEF.Size = new System.Drawing.Size(219, 22);
            this.rbtnCLEF.TabIndex = 14;
            this.rbtnCLEF.TabStop = true;
            this.rbtnCLEF.Text = "Compact Format Per line/row";
            this.rbtnCLEF.UseVisualStyleBackColor = true;
            // 
            // rbtnJsonPerLine
            // 
            this.rbtnJsonPerLine.AutoSize = true;
            this.rbtnJsonPerLine.Location = new System.Drawing.Point(6, 50);
            this.rbtnJsonPerLine.Name = "rbtnJsonPerLine";
            this.rbtnJsonPerLine.Size = new System.Drawing.Size(250, 22);
            this.rbtnJsonPerLine.TabIndex = 15;
            this.rbtnJsonPerLine.Text = "Standard Json format per line/row";
            this.rbtnJsonPerLine.UseVisualStyleBackColor = true;
            // 
            // txtbOpenFileFilters
            // 
            this.txtbOpenFileFilters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtbOpenFileFilters.Location = new System.Drawing.Point(276, 35);
            this.txtbOpenFileFilters.Name = "txtbOpenFileFilters";
            this.txtbOpenFileFilters.Size = new System.Drawing.Size(376, 26);
            this.txtbOpenFileFilters.TabIndex = 47;
            // 
            // lblOpenfilesFilters
            // 
            this.lblOpenfilesFilters.AutoEllipsis = true;
            this.lblOpenfilesFilters.Location = new System.Drawing.Point(13, 35);
            this.lblOpenfilesFilters.Name = "lblOpenfilesFilters";
            this.lblOpenfilesFilters.Size = new System.Drawing.Size(148, 22);
            this.lblOpenfilesFilters.TabIndex = 46;
            this.lblOpenfilesFilters.Text = "Open file Filter:";
            // 
            // btnTestFilter
            // 
            this.btnTestFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestFilter.Location = new System.Drawing.Point(658, 35);
            this.btnTestFilter.Name = "btnTestFilter";
            this.btnTestFilter.Size = new System.Drawing.Size(55, 25);
            this.btnTestFilter.TabIndex = 48;
            this.btnTestFilter.Text = "Test";
            this.btnTestFilter.UseVisualStyleBackColor = true;
            this.btnTestFilter.Click += new System.EventHandler(this.btnTestFilter_Click);
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPage2);
            this.tabControlMain.Controls.Add(this.tabPage1);
            this.tabControlMain.Controls.Add(this.tabPage3);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(725, 584);
            this.tabControlMain.TabIndex = 49;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.rbDetectionModeManual);
            this.tabPage1.Controls.Add(this.rbDetectionModeAutomatic);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.btnTestFilter);
            this.tabPage1.Controls.Add(this.txtbOpenFileFilters);
            this.tabPage1.Controls.Add(this.btnOpenFolder);
            this.tabPage1.Controls.Add(this.lblOpenfilesFilters);
            this.tabPage1.Controls.Add(this.txtbDirectory);
            this.tabPage1.Controls.Add(this.txtbSupportedFiles);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Location = new System.Drawing.Point(4, 27);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(717, 553);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Parsing Settings";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // rbDetectionModeManual
            // 
            this.rbDetectionModeManual.AutoSize = true;
            this.rbDetectionModeManual.Location = new System.Drawing.Point(447, 120);
            this.rbDetectionModeManual.Name = "rbDetectionModeManual";
            this.rbDetectionModeManual.Size = new System.Drawing.Size(75, 22);
            this.rbDetectionModeManual.TabIndex = 53;
            this.rbDetectionModeManual.Text = "Manual";
            this.rbDetectionModeManual.UseVisualStyleBackColor = true;
            // 
            // rbDetectionModeAutomatic
            // 
            this.rbDetectionModeAutomatic.AutoSize = true;
            this.rbDetectionModeAutomatic.Checked = true;
            this.rbDetectionModeAutomatic.Location = new System.Drawing.Point(276, 120);
            this.rbDetectionModeAutomatic.Name = "rbDetectionModeAutomatic";
            this.rbDetectionModeAutomatic.Size = new System.Drawing.Size(94, 22);
            this.rbDetectionModeAutomatic.TabIndex = 52;
            this.rbDetectionModeAutomatic.TabStop = true;
            this.rbDetectionModeAutomatic.Text = "Automatic";
            this.rbDetectionModeAutomatic.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoEllipsis = true;
            this.label1.Location = new System.Drawing.Point(13, 120);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 22);
            this.label1.TabIndex = 51;
            this.label1.Text = "File Detection Mode:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.rbtnReset);
            this.groupBox1.Controls.Add(this.rbtnJsonFile);
            this.groupBox1.Controls.Add(this.rtxtExample);
            this.groupBox1.Controls.Add(this.rbtnCLEF);
            this.groupBox1.Controls.Add(this.rbtnCompactJsonFile);
            this.groupBox1.Controls.Add(this.rbtnJsonPerLine);
            this.groupBox1.Location = new System.Drawing.Point(13, 148);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(695, 399);
            this.groupBox1.TabIndex = 50;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Format:";
            // 
            // rbtnReset
            // 
            this.rbtnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbtnReset.AutoSize = true;
            this.rbtnReset.Location = new System.Drawing.Point(523, 25);
            this.rbtnReset.Name = "rbtnReset";
            this.rbtnReset.Size = new System.Drawing.Size(148, 22);
            this.rbtnReset.TabIndex = 52;
            this.rbtnReset.Text = "Reset to Unknown";
            this.rbtnReset.UseVisualStyleBackColor = true;
            // 
            // rbtnJsonFile
            // 
            this.rbtnJsonFile.AutoSize = true;
            this.rbtnJsonFile.Location = new System.Drawing.Point(6, 106);
            this.rbtnJsonFile.Name = "rbtnJsonFile";
            this.rbtnJsonFile.Size = new System.Drawing.Size(214, 22);
            this.rbtnJsonFile.TabIndex = 51;
            this.rbtnJsonFile.Text = "Standard format full Json file";
            this.rbtnJsonFile.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.rbtnJsonFile.UseVisualStyleBackColor = true;
            // 
            // rtxtExample
            // 
            this.rtxtExample.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtxtExample.Location = new System.Drawing.Point(6, 147);
            this.rtxtExample.Name = "rtxtExample";
            this.rtxtExample.Size = new System.Drawing.Size(683, 245);
            this.rtxtExample.TabIndex = 50;
            this.rtxtExample.Text = "";
            // 
            // rbtnCompactJsonFile
            // 
            this.rbtnCompactJsonFile.AutoSize = true;
            this.rbtnCompactJsonFile.Location = new System.Drawing.Point(6, 78);
            this.rbtnCompactJsonFile.Name = "rbtnCompactJsonFile";
            this.rbtnCompactJsonFile.Size = new System.Drawing.Size(214, 22);
            this.rbtnCompactJsonFile.TabIndex = 49;
            this.rbtnCompactJsonFile.Text = "Compact format full Json file";
            this.rbtnCompactJsonFile.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.rbtnCompactJsonFile.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.txtbIgnoreColumn);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Controls.Add(this.btnDeleteIgnoreColumn);
            this.tabPage3.Controls.Add(this.btnIgnoreColumn);
            this.tabPage3.Controls.Add(this.lstbIgnoreColumn);
            this.tabPage3.Location = new System.Drawing.Point(4, 27);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(717, 553);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "Dynamic Columns";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // txtbIgnoreColumn
            // 
            this.txtbIgnoreColumn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtbIgnoreColumn.Location = new System.Drawing.Point(6, 46);
            this.txtbIgnoreColumn.Name = "txtbIgnoreColumn";
            this.txtbIgnoreColumn.Size = new System.Drawing.Size(649, 26);
            this.txtbIgnoreColumn.TabIndex = 63;
            // 
            // label4
            // 
            this.label4.AccessibleDescription = "";
            this.label4.AutoEllipsis = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(3, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(711, 24);
            this.label4.TabIndex = 62;
            this.label4.Text = "Dynamic columns: Ignore the following entries in the dynamic columns generation:";
            // 
            // btnDeleteIgnoreColumn
            // 
            this.btnDeleteIgnoreColumn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteIgnoreColumn.Location = new System.Drawing.Point(615, 133);
            this.btnDeleteIgnoreColumn.Name = "btnDeleteIgnoreColumn";
            this.btnDeleteIgnoreColumn.Size = new System.Drawing.Size(93, 25);
            this.btnDeleteIgnoreColumn.TabIndex = 61;
            this.btnDeleteIgnoreColumn.Text = "Delete";
            this.btnDeleteIgnoreColumn.UseVisualStyleBackColor = true;
            this.btnDeleteIgnoreColumn.Click += new System.EventHandler(this.btnDeleteIgnoreColumn_Click);
            // 
            // btnIgnoreColumn
            // 
            this.btnIgnoreColumn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIgnoreColumn.Location = new System.Drawing.Point(661, 46);
            this.btnIgnoreColumn.Name = "btnIgnoreColumn";
            this.btnIgnoreColumn.Size = new System.Drawing.Size(47, 25);
            this.btnIgnoreColumn.TabIndex = 60;
            this.btnIgnoreColumn.Text = "Add";
            this.btnIgnoreColumn.UseVisualStyleBackColor = true;
            this.btnIgnoreColumn.Click += new System.EventHandler(this.btnIgnoreColumn_Click);
            // 
            // lstbIgnoreColumn
            // 
            this.lstbIgnoreColumn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstbIgnoreColumn.FormattingEnabled = true;
            this.lstbIgnoreColumn.ItemHeight = 18;
            this.lstbIgnoreColumn.Location = new System.Drawing.Point(6, 78);
            this.lstbIgnoreColumn.Name = "lstbIgnoreColumn";
            this.lstbIgnoreColumn.Size = new System.Drawing.Size(603, 472);
            this.lstbIgnoreColumn.TabIndex = 59;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnImport);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnExportSettings);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 584);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(725, 56);
            this.panel1.TabIndex = 50;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 27);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(717, 553);
            this.tabPage2.TabIndex = 2;
            this.tabPage2.Text = "General Settings";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbtnApplicationFolder);
            this.groupBox2.Controls.Add(this.rbtnPerUser);
            this.groupBox2.Location = new System.Drawing.Point(14, 22);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(683, 94);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Storage and Location";
            // 
            // rbtnPerUser
            // 
            this.rbtnPerUser.AutoSize = true;
            this.rbtnPerUser.Location = new System.Drawing.Point(14, 22);
            this.rbtnPerUser.Name = "rbtnPerUser";
            this.rbtnPerUser.Size = new System.Drawing.Size(524, 22);
            this.rbtnPerUser.TabIndex = 0;
            this.rbtnPerUser.TabStop = true;
            this.rbtnPerUser.Text = "Per User: Store settings in: %userprofile%\\appdata\\local\\Analogy.LogViewer";
            this.rbtnPerUser.UseVisualStyleBackColor = true;
            // 
            // rbtnApplicationFolder
            // 
            this.rbtnApplicationFolder.AutoSize = true;
            this.rbtnApplicationFolder.Location = new System.Drawing.Point(14, 54);
            this.rbtnApplicationFolder.Name = "rbtnApplicationFolder";
            this.rbtnApplicationFolder.Size = new System.Drawing.Size(540, 22);
            this.rbtnApplicationFolder.TabIndex = 1;
            this.rbtnApplicationFolder.TabStop = true;
            this.rbtnApplicationFolder.Text = "Portable: Store settings in the Application Folder (May need folder permissions)";
            this.rbtnApplicationFolder.UseVisualStyleBackColor = true;
            // 
            // SerilogUCSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Tahoma", 9F);
            this.Name = "SerilogUCSettings";
            this.Size = new System.Drawing.Size(725, 640);
            this.Load += new System.EventHandler(this.NLogSettings_Load);
            this.tabControlMain.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnExportSettings;
        private System.Windows.Forms.TextBox txtbSupportedFiles;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.TextBox txtbDirectory;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnOpenFolder;
        private System.Windows.Forms.RadioButton rbtnCLEF;
        private System.Windows.Forms.RadioButton rbtnJsonPerLine;
        private System.Windows.Forms.TextBox txtbOpenFileFilters;
        private System.Windows.Forms.Label lblOpenfilesFilters;
        private System.Windows.Forms.Button btnTestFilter;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtbIgnoreColumn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnDeleteIgnoreColumn;
        private System.Windows.Forms.Button btnIgnoreColumn;
        private System.Windows.Forms.ListBox lstbIgnoreColumn;
        private System.Windows.Forms.RadioButton rbtnCompactJsonFile;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox rtxtExample;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbDetectionModeAutomatic;
        private System.Windows.Forms.RadioButton rbDetectionModeManual;
        private System.Windows.Forms.RadioButton rbtnJsonFile;
        private System.Windows.Forms.RadioButton rbtnReset;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbtnPerUser;
        private System.Windows.Forms.RadioButton rbtnApplicationFolder;
    }
}
