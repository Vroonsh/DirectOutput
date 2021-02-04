namespace DirectOutputToolkit
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownMatrixMinDuration = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDownMinDuration = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxDofAutoUpdate = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonSaveSettings = new System.Windows.Forms.Button();
            this.checkBoxAutoSave = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMatrixMinDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinDuration)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(147, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Minimal Matrix Effect Duration";
            // 
            // numericUpDownMatrixMinDuration
            // 
            this.numericUpDownMatrixMinDuration.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownMatrixMinDuration.Location = new System.Drawing.Point(302, 45);
            this.numericUpDownMatrixMinDuration.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownMatrixMinDuration.Name = "numericUpDownMatrixMinDuration";
            this.numericUpDownMatrixMinDuration.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownMatrixMinDuration.TabIndex = 16;
            this.numericUpDownMatrixMinDuration.ValueChanged += new System.EventHandler(this.numericUpDownMatrixMinDuration_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Minimal Effect Duration";
            // 
            // numericUpDownMinDuration
            // 
            this.numericUpDownMinDuration.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownMinDuration.Location = new System.Drawing.Point(302, 19);
            this.numericUpDownMinDuration.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownMinDuration.Name = "numericUpDownMinDuration";
            this.numericUpDownMinDuration.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownMinDuration.TabIndex = 14;
            this.numericUpDownMinDuration.ValueChanged += new System.EventHandler(this.numericUpDownMinDuration_ValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.checkBoxDofAutoUpdate);
            this.groupBox2.Location = new System.Drawing.Point(15, 92);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(428, 52);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "DofConfigTool";
            // 
            // checkBoxDofAutoUpdate
            // 
            this.checkBoxDofAutoUpdate.AutoSize = true;
            this.checkBoxDofAutoUpdate.Location = new System.Drawing.Point(7, 20);
            this.checkBoxDofAutoUpdate.Name = "checkBoxDofAutoUpdate";
            this.checkBoxDofAutoUpdate.Size = new System.Drawing.Size(86, 17);
            this.checkBoxDofAutoUpdate.TabIndex = 0;
            this.checkBoxDofAutoUpdate.Text = "Auto Update";
            this.checkBoxDofAutoUpdate.UseVisualStyleBackColor = true;
            this.checkBoxDofAutoUpdate.CheckedChanged += new System.EventHandler(this.checkBoxDofAutoUpdate_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.numericUpDownMinDuration);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.numericUpDownMatrixMinDuration);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(15, 13);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(428, 73);
            this.groupBox3.TabIndex = 23;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Pinball";
            // 
            // buttonSaveSettings
            // 
            this.buttonSaveSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveSettings.Location = new System.Drawing.Point(368, 184);
            this.buttonSaveSettings.Name = "buttonSaveSettings";
            this.buttonSaveSettings.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveSettings.TabIndex = 24;
            this.buttonSaveSettings.Text = "Save";
            this.buttonSaveSettings.UseVisualStyleBackColor = true;
            this.buttonSaveSettings.Click += new System.EventHandler(this.buttonSaveSettings_Click);
            // 
            // checkBoxAutoSave
            // 
            this.checkBoxAutoSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxAutoSave.AutoSize = true;
            this.checkBoxAutoSave.Location = new System.Drawing.Point(15, 188);
            this.checkBoxAutoSave.Name = "checkBoxAutoSave";
            this.checkBoxAutoSave.Size = new System.Drawing.Size(171, 17);
            this.checkBoxAutoSave.TabIndex = 25;
            this.checkBoxAutoSave.Text = "Auto save when leaving toolkit";
            this.checkBoxAutoSave.UseVisualStyleBackColor = true;
            this.checkBoxAutoSave.CheckedChanged += new System.EventHandler(this.checkBoxAutoSave_CheckedChanged);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 219);
            this.Controls.Add(this.checkBoxAutoSave);
            this.Controls.Add(this.buttonSaveSettings);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DirectOutput Toolkit Settings";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMatrixMinDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinDuration)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownMatrixMinDuration;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDownMinDuration;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBoxDofAutoUpdate;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button buttonSaveSettings;
        private System.Windows.Forms.CheckBox checkBoxAutoSave;
    }
}