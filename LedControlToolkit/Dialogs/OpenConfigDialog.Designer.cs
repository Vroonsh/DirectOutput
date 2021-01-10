namespace LedControlToolkit
{
    partial class OpenConfigDialog
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
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpenConfigDialog));
            this.OKButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.OpenGlobalConfigFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.GlobalConfigFileSelectButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.LedControlFileSelectButton = new System.Windows.Forms.Button();
            this.OpenLedControlIniFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.GlobalConfigFilenameComboBox = new System.Windows.Forms.ComboBox();
            this.LedControlIniFilenameComboBox = new System.Windows.Forms.ComboBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.dofConfigToolSetupControl1 = new DofConfigToolWrapper.DofConfigToolSetupControl();
            this.directoryEntry1 = new System.DirectoryServices.DirectoryEntry();
            this.directOutputPreviewControl1 = new DirectOutputControls.DirectOutputPreviewControl();
            this.directOutputViewSetupControl1 = new DirectOutputControls.DirectOutputViewSetupControl();
            this.SuspendLayout();
            // 
            // OKButton
            // 
            this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKButton.Location = new System.Drawing.Point(110, 64);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(159, 23);
            this.OKButton.TabIndex = 0;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(1367, 65);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(150, 23);
            this.CancelButton.TabIndex = 1;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // OpenGlobalConfigFileDialog
            // 
            this.OpenGlobalConfigFileDialog.DefaultExt = "xml";
            this.OpenGlobalConfigFileDialog.Filter = "XML files (*.xml)|*.xml|All files|*.*";
            this.OpenGlobalConfigFileDialog.Title = "Select a global config file";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Global config file:";
            // 
            // GlobalConfigFileSelectButton
            // 
            this.GlobalConfigFileSelectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GlobalConfigFileSelectButton.Location = new System.Drawing.Point(1472, 5);
            this.GlobalConfigFileSelectButton.Name = "GlobalConfigFileSelectButton";
            this.GlobalConfigFileSelectButton.Size = new System.Drawing.Size(45, 23);
            this.GlobalConfigFileSelectButton.TabIndex = 4;
            this.GlobalConfigFileSelectButton.Text = "Select";
            this.GlobalConfigFileSelectButton.UseVisualStyleBackColor = true;
            this.GlobalConfigFileSelectButton.Click += new System.EventHandler(this.GlobalConfigFileSelectButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Led Control Ini file :";
            // 
            // LedControlFileSelectButton
            // 
            this.LedControlFileSelectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LedControlFileSelectButton.Location = new System.Drawing.Point(1472, 36);
            this.LedControlFileSelectButton.Name = "LedControlFileSelectButton";
            this.LedControlFileSelectButton.Size = new System.Drawing.Size(45, 23);
            this.LedControlFileSelectButton.TabIndex = 8;
            this.LedControlFileSelectButton.Text = "Select";
            this.LedControlFileSelectButton.UseVisualStyleBackColor = true;
            this.LedControlFileSelectButton.Click += new System.EventHandler(this.TableFileSelectButton_Click);
            // 
            // OpenLedControlIniFileDialog
            // 
            this.OpenLedControlIniFileDialog.DefaultExt = "ini";
            this.OpenLedControlIniFileDialog.Filter = "Led Control ini File (*.ini)|*.ini|All files|*.*";
            this.OpenLedControlIniFileDialog.Title = "Select a Led control ini file";
            // 
            // GlobalConfigFilenameComboBox
            // 
            this.GlobalConfigFilenameComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.GlobalConfigFilenameComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.GlobalConfigFilenameComboBox.FormattingEnabled = true;
            this.GlobalConfigFilenameComboBox.Location = new System.Drawing.Point(110, 9);
            this.GlobalConfigFilenameComboBox.MaxDropDownItems = 16;
            this.GlobalConfigFilenameComboBox.Name = "GlobalConfigFilenameComboBox";
            this.GlobalConfigFilenameComboBox.Size = new System.Drawing.Size(520, 21);
            this.GlobalConfigFilenameComboBox.TabIndex = 12;
            // 
            // LedControlIniFilenameComboBox
            // 
            this.LedControlIniFilenameComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.LedControlIniFilenameComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.LedControlIniFilenameComboBox.FormattingEnabled = true;
            this.LedControlIniFilenameComboBox.Location = new System.Drawing.Point(110, 37);
            this.LedControlIniFilenameComboBox.Name = "LedControlIniFilenameComboBox";
            this.LedControlIniFilenameComboBox.Size = new System.Drawing.Size(520, 21);
            this.LedControlIniFilenameComboBox.TabIndex = 13;
            // 
            // dofConfigToolSetupControl1
            // 
            this.dofConfigToolSetupControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dofConfigToolSetupControl1.Location = new System.Drawing.Point(15, 103);
            this.dofConfigToolSetupControl1.Name = "dofConfigToolSetupControl1";
            this.dofConfigToolSetupControl1.Size = new System.Drawing.Size(592, 725);
            this.dofConfigToolSetupControl1.TabIndex = 14;
            // 
            // directOutputPreviewControl1
            // 
            this.directOutputPreviewControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directOutputPreviewControl1.AreaDisplayColor = System.Drawing.Color.Green;
            this.directOutputPreviewControl1.BackgroundColor = System.Drawing.Color.MidnightBlue;
            this.directOutputPreviewControl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.directOutputPreviewControl1.Location = new System.Drawing.Point(1203, 103);
            this.directOutputPreviewControl1.Name = "directOutputPreviewControl1";
            this.directOutputPreviewControl1.Size = new System.Drawing.Size(363, 725);
            this.directOutputPreviewControl1.TabIndex = 15;
            // 
            // directOutputViewSetupControl1
            // 
            this.directOutputViewSetupControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.directOutputViewSetupControl1.DirectOutputViewSetup = null;
            this.directOutputViewSetupControl1.Location = new System.Drawing.Point(613, 103);
            this.directOutputViewSetupControl1.Name = "directOutputViewSetupControl1";
            this.directOutputViewSetupControl1.Size = new System.Drawing.Size(584, 725);
            this.directOutputViewSetupControl1.TabIndex = 16;
            // 
            // OpenConfigDialog
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1578, 840);
            this.Controls.Add(this.directOutputViewSetupControl1);
            this.Controls.Add(this.directOutputPreviewControl1);
            this.Controls.Add(this.dofConfigToolSetupControl1);
            this.Controls.Add(this.LedControlIniFilenameComboBox);
            this.Controls.Add(this.GlobalConfigFilenameComboBox);
            this.Controls.Add(this.LedControlFileSelectButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.GlobalConfigFileSelectButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OKButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "OpenConfigDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "LedControl Toolkit Open Configuration";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OKButton;
        private new System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.OpenFileDialog OpenGlobalConfigFileDialog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button GlobalConfigFileSelectButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button LedControlFileSelectButton;
        private System.Windows.Forms.OpenFileDialog OpenLedControlIniFileDialog;
        private System.Windows.Forms.ComboBox GlobalConfigFilenameComboBox;
        private System.Windows.Forms.ComboBox LedControlIniFilenameComboBox;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private DofConfigToolWrapper.DofConfigToolSetupControl dofConfigToolSetupControl1;
        private System.DirectoryServices.DirectoryEntry directoryEntry1;
        private DirectOutputControls.DirectOutputPreviewControl directOutputPreviewControl1;
        private DirectOutputControls.DirectOutputViewSetupControl directOutputViewSetupControl1;
    }
}