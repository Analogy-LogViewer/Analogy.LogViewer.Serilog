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
            this.lblLayout = new System.Windows.Forms.Label();
            this.txtNLogExtension = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnImport = new System.Windows.Forms.Button();
            this.txtbDirectory = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOpenFolder = new System.Windows.Forms.Button();
            this.rbtnCLEF = new System.Windows.Forms.RadioButton();
            this.rbJson = new System.Windows.Forms.RadioButton();
            this.rbSimpleTextFile = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(592, 143);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(114, 40);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save Settings";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnExportSettings
            // 
            this.btnExportSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportSettings.Location = new System.Drawing.Point(473, 143);
            this.btnExportSettings.Name = "btnExportSettings";
            this.btnExportSettings.Size = new System.Drawing.Size(114, 40);
            this.btnExportSettings.TabIndex = 2;
            this.btnExportSettings.Text = "Export Settings";
            this.btnExportSettings.UseVisualStyleBackColor = true;
            this.btnExportSettings.Click += new System.EventHandler(this.btnExportSettings_Click);
            // 
            // lblLayout
            // 
            this.lblLayout.AutoSize = true;
            this.lblLayout.Location = new System.Drawing.Point(3, 5);
            this.lblLayout.Name = "lblLayout";
            this.lblLayout.Size = new System.Drawing.Size(57, 17);
            this.lblLayout.TabIndex = 3;
            this.lblLayout.Text = "Format:";
            // 
            // txtNLogExtension
            // 
            this.txtNLogExtension.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNLogExtension.Location = new System.Drawing.Point(169, 111);
            this.txtNLogExtension.Name = "txtNLogExtension";
            this.txtNLogExtension.Size = new System.Drawing.Size(537, 23);
            this.txtNLogExtension.TabIndex = 9;
            this.txtNLogExtension.Text = "*.Clef";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 113);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(139, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "Serilog File Extension:";
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Location = new System.Drawing.Point(592, 3);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(114, 25);
            this.btnImport.TabIndex = 10;
            this.btnImport.Text = "Import settings";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // txtbDirectory
            // 
            this.txtbDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtbDirectory.Location = new System.Drawing.Point(169, 84);
            this.txtbDirectory.Name = "txtbDirectory";
            this.txtbDirectory.Size = new System.Drawing.Size(502, 23);
            this.txtbDirectory.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 17);
            this.label3.TabIndex = 11;
            this.label3.Text = "Logs Location:";
            // 
            // btnOpenFolder
            // 
            this.btnOpenFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenFolder.Location = new System.Drawing.Point(677, 82);
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.btnOpenFolder.Size = new System.Drawing.Size(28, 25);
            this.btnOpenFolder.TabIndex = 13;
            this.btnOpenFolder.Text = "..";
            this.btnOpenFolder.UseVisualStyleBackColor = true;
            this.btnOpenFolder.Click += new System.EventHandler(this.btnOpenFolder_Click);
            // 
            // rbtnCLEF
            // 
            this.rbtnCLEF.AutoSize = true;
            this.rbtnCLEF.Checked = true;
            this.rbtnCLEF.Location = new System.Drawing.Point(75, 3);
            this.rbtnCLEF.Name = "rbtnCLEF";
            this.rbtnCLEF.Size = new System.Drawing.Size(219, 21);
            this.rbtnCLEF.TabIndex = 14;
            this.rbtnCLEF.TabStop = true;
            this.rbtnCLEF.Text = "Compact Log Event File (CLEF)";
            this.rbtnCLEF.UseVisualStyleBackColor = true;
            // 
            // rbJson
            // 
            this.rbJson.AutoSize = true;
            this.rbJson.Enabled = false;
            this.rbJson.Location = new System.Drawing.Point(75, 29);
            this.rbJson.Name = "rbJson";
            this.rbJson.Size = new System.Drawing.Size(146, 21);
            this.rbJson.TabIndex = 15;
            this.rbJson.Text = "Json Log Event File";
            this.rbJson.UseVisualStyleBackColor = true;
            // 
            // rbSimpleTextFile
            // 
            this.rbSimpleTextFile.AutoSize = true;
            this.rbSimpleTextFile.Enabled = false;
            this.rbSimpleTextFile.Location = new System.Drawing.Point(75, 56);
            this.rbSimpleTextFile.Name = "rbSimpleTextFile";
            this.rbSimpleTextFile.Size = new System.Drawing.Size(266, 21);
            this.rbSimpleTextFile.TabIndex = 16;
            this.rbSimpleTextFile.Text = "Single simple line (Use Plain text parser)";
            this.rbSimpleTextFile.UseVisualStyleBackColor = true;
            // 
            // SerilogUCSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rbSimpleTextFile);
            this.Controls.Add(this.rbJson);
            this.Controls.Add(this.rbtnCLEF);
            this.Controls.Add(this.btnOpenFolder);
            this.Controls.Add(this.txtbDirectory);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.txtNLogExtension);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblLayout);
            this.Controls.Add(this.btnExportSettings);
            this.Controls.Add(this.btnSave);
            this.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SerilogUCSettings";
            this.Size = new System.Drawing.Size(709, 186);
            this.Load += new System.EventHandler(this.NLogSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnExportSettings;
        private System.Windows.Forms.Label lblLayout;
        private System.Windows.Forms.TextBox txtNLogExtension;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.TextBox txtbDirectory;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnOpenFolder;
        private System.Windows.Forms.RadioButton rbtnCLEF;
        private System.Windows.Forms.RadioButton rbJson;
        private System.Windows.Forms.RadioButton rbSimpleTextFile;
    }
}
