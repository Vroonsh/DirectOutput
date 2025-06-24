using DirectOutputControls;
using DofConfigToolWrapper;

namespace DirectOutputToolkit
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
            this.OpenDofConfigSetupFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.DofConfigSetupFileSelectButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.DofViewSetupSelectButton = new System.Windows.Forms.Button();
            this.OpenDofViewSetupFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.DofConfigSetupFilenameComboBox = new System.Windows.Forms.ComboBox();
            this.DofViewSetupFilenameComboBox = new System.Windows.Forms.ComboBox();
            this.buttonDofConfigSetupEdit = new System.Windows.Forms.Button();
            this.buttonDofViewSetupEdit = new System.Windows.Forms.Button();
            this.checkBoxForceDownload = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ConnectionMethodComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // OKButton
            // 
            this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKButton.Location = new System.Drawing.Point(580, 96);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(159, 23);
            this.OKButton.TabIndex = 0;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(744, 96);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(150, 23);
            this.CancelButton.TabIndex = 1;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // OpenDofConfigSetupFileDialog
            // 
            this.OpenDofConfigSetupFileDialog.Title = "Select a DofConfig setup file";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "DofConfig setup file";
            // 
            // DofConfigSetupFileSelectButton
            // 
            this.DofConfigSetupFileSelectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DofConfigSetupFileSelectButton.Location = new System.Drawing.Point(798, 7);
            this.DofConfigSetupFileSelectButton.Name = "DofConfigSetupFileSelectButton";
            this.DofConfigSetupFileSelectButton.Size = new System.Drawing.Size(45, 23);
            this.DofConfigSetupFileSelectButton.TabIndex = 4;
            this.DofConfigSetupFileSelectButton.Text = "Select";
            this.DofConfigSetupFileSelectButton.UseVisualStyleBackColor = true;
            this.DofConfigSetupFileSelectButton.Click += new System.EventHandler(this.DofConfigSetupFileSelectButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(175, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "DirectOutput Toolkit view setup  file";
            // 
            // DofViewSetupSelectButton
            // 
            this.DofViewSetupSelectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DofViewSetupSelectButton.Location = new System.Drawing.Point(798, 64);
            this.DofViewSetupSelectButton.Name = "DofViewSetupSelectButton";
            this.DofViewSetupSelectButton.Size = new System.Drawing.Size(45, 23);
            this.DofViewSetupSelectButton.TabIndex = 8;
            this.DofViewSetupSelectButton.Text = "Select";
            this.DofViewSetupSelectButton.UseVisualStyleBackColor = true;
            this.DofViewSetupSelectButton.Click += new System.EventHandler(this.DofViewSetupFileSelectButton_Click);
            // 
            // OpenDofViewSetupFileDialog
            // 
            this.OpenDofViewSetupFileDialog.Title = "Select a Led control ini file";
            // 
            // DofConfigSetupFilenameComboBox
            // 
            this.DofConfigSetupFilenameComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DofConfigSetupFilenameComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.DofConfigSetupFilenameComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.DofConfigSetupFilenameComboBox.FormattingEnabled = true;
            this.DofConfigSetupFilenameComboBox.Location = new System.Drawing.Point(120, 9);
            this.DofConfigSetupFilenameComboBox.MaxDropDownItems = 16;
            this.DofConfigSetupFilenameComboBox.Name = "DofConfigSetupFilenameComboBox";
            this.DofConfigSetupFilenameComboBox.Size = new System.Drawing.Size(672, 21);
            this.DofConfigSetupFilenameComboBox.TabIndex = 12;
            // 
            // DofViewSetupFilenameComboBox
            // 
            this.DofViewSetupFilenameComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DofViewSetupFilenameComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.DofViewSetupFilenameComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.DofViewSetupFilenameComboBox.FormattingEnabled = true;
            this.DofViewSetupFilenameComboBox.Location = new System.Drawing.Point(193, 66);
            this.DofViewSetupFilenameComboBox.Name = "DofViewSetupFilenameComboBox";
            this.DofViewSetupFilenameComboBox.Size = new System.Drawing.Size(599, 21);
            this.DofViewSetupFilenameComboBox.TabIndex = 13;
            // 
            // buttonDofConfigSetupEdit
            // 
            this.buttonDofConfigSetupEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDofConfigSetupEdit.Location = new System.Drawing.Point(849, 7);
            this.buttonDofConfigSetupEdit.Name = "buttonDofConfigSetupEdit";
            this.buttonDofConfigSetupEdit.Size = new System.Drawing.Size(45, 23);
            this.buttonDofConfigSetupEdit.TabIndex = 14;
            this.buttonDofConfigSetupEdit.Text = "Edit";
            this.buttonDofConfigSetupEdit.UseVisualStyleBackColor = true;
            this.buttonDofConfigSetupEdit.Click += new System.EventHandler(this.buttonDofConfigSetupEdit_Click);
            // 
            // buttonDofViewSetupEdit
            // 
            this.buttonDofViewSetupEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDofViewSetupEdit.Location = new System.Drawing.Point(849, 64);
            this.buttonDofViewSetupEdit.Name = "buttonDofViewSetupEdit";
            this.buttonDofViewSetupEdit.Size = new System.Drawing.Size(45, 23);
            this.buttonDofViewSetupEdit.TabIndex = 15;
            this.buttonDofViewSetupEdit.Text = "Edit";
            this.buttonDofViewSetupEdit.UseVisualStyleBackColor = true;
            this.buttonDofViewSetupEdit.Click += new System.EventHandler(this.buttonDofViewSetupEdit_Click);
            // 
            // checkBoxForceDownload
            // 
            this.checkBoxForceDownload.AutoSize = true;
            this.checkBoxForceDownload.Location = new System.Drawing.Point(335, 36);
            this.checkBoxForceDownload.Name = "checkBoxForceDownload";
            this.checkBoxForceDownload.Size = new System.Drawing.Size(155, 17);
            this.checkBoxForceDownload.TabIndex = 16;
            this.checkBoxForceDownload.Text = "Force Dofconfigtool update";
            this.checkBoxForceDownload.UseVisualStyleBackColor = true;
            this.checkBoxForceDownload.CheckedChanged += new System.EventHandler(this.checkBoxForceDownload_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Connection method";
            // 
            // ConnectionMethodComboBox
            // 
            this.ConnectionMethodComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ConnectionMethodComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ConnectionMethodComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ConnectionMethodComboBox.FormattingEnabled = true;
            this.ConnectionMethodComboBox.Location = new System.Drawing.Point(120, 34);
            this.ConnectionMethodComboBox.MaxDropDownItems = 16;
            this.ConnectionMethodComboBox.Name = "ConnectionMethodComboBox";
            this.ConnectionMethodComboBox.Size = new System.Drawing.Size(200, 21);
            this.ConnectionMethodComboBox.TabIndex = 18;
            this.ConnectionMethodComboBox.SelectedIndexChanged += new System.EventHandler(this.ConnectionMethodComboBox_SelectedIndexChanged);
            // 
            // OpenConfigDialog
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(906, 133);
            this.Controls.Add(this.ConnectionMethodComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkBoxForceDownload);
            this.Controls.Add(this.buttonDofViewSetupEdit);
            this.Controls.Add(this.buttonDofConfigSetupEdit);
            this.Controls.Add(this.DofViewSetupFilenameComboBox);
            this.Controls.Add(this.DofConfigSetupFilenameComboBox);
            this.Controls.Add(this.DofViewSetupSelectButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.DofConfigSetupFileSelectButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OKButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(16, 140);
            this.Name = "OpenConfigDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DirectOutput Toolkit Open Configuration";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OKButton;
        private new System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.OpenFileDialog OpenDofConfigSetupFileDialog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button DofConfigSetupFileSelectButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button DofViewSetupSelectButton;
        private System.Windows.Forms.OpenFileDialog OpenDofViewSetupFileDialog;
        private System.Windows.Forms.ComboBox DofConfigSetupFilenameComboBox;
        private System.Windows.Forms.ComboBox DofViewSetupFilenameComboBox;
        private System.Windows.Forms.Button buttonDofConfigSetupEdit;
        private System.Windows.Forms.Button buttonDofViewSetupEdit;
        private System.Windows.Forms.CheckBox checkBoxForceDownload;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ConnectionMethodComboBox;
    }
}