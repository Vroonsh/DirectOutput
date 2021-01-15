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
            this.numericUpDownPulseDuration = new System.Windows.Forms.NumericUpDown();
            this.labelPulseDelay = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxDrawAreasInfos = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxDofAutoUpdate = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonSaveSettings = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMatrixMinDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPulseDuration)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(147, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Minimal Matrix Effect Duration";
            // 
            // numericUpDownMatrixMinDuration
            // 
            this.numericUpDownMatrixMinDuration.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownMatrixMinDuration.Location = new System.Drawing.Point(302, 65);
            this.numericUpDownMatrixMinDuration.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownMatrixMinDuration.Name = "numericUpDownMatrixMinDuration";
            this.numericUpDownMatrixMinDuration.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownMatrixMinDuration.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Minimal Effect Duration";
            // 
            // numericUpDownMinDuration
            // 
            this.numericUpDownMinDuration.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownMinDuration.Location = new System.Drawing.Point(302, 39);
            this.numericUpDownMinDuration.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownMinDuration.Name = "numericUpDownMinDuration";
            this.numericUpDownMinDuration.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownMinDuration.TabIndex = 14;
            // 
            // numericUpDownPulseDuration
            // 
            this.numericUpDownPulseDuration.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownPulseDuration.Location = new System.Drawing.Point(302, 14);
            this.numericUpDownPulseDuration.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDownPulseDuration.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownPulseDuration.Name = "numericUpDownPulseDuration";
            this.numericUpDownPulseDuration.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownPulseDuration.TabIndex = 20;
            this.numericUpDownPulseDuration.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // labelPulseDelay
            // 
            this.labelPulseDelay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPulseDelay.AutoSize = true;
            this.labelPulseDelay.Location = new System.Drawing.Point(17, 16);
            this.labelPulseDelay.Name = "labelPulseDelay";
            this.labelPulseDelay.Size = new System.Drawing.Size(76, 13);
            this.labelPulseDelay.TabIndex = 19;
            this.labelPulseDelay.Text = "Pulse Duration";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.checkBoxDrawAreasInfos);
            this.groupBox1.Location = new System.Drawing.Point(15, 177);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(428, 53);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Preview";
            // 
            // checkBoxDrawAreasInfos
            // 
            this.checkBoxDrawAreasInfos.AutoSize = true;
            this.checkBoxDrawAreasInfos.Location = new System.Drawing.Point(6, 19);
            this.checkBoxDrawAreasInfos.Name = "checkBoxDrawAreasInfos";
            this.checkBoxDrawAreasInfos.Size = new System.Drawing.Size(130, 17);
            this.checkBoxDrawAreasInfos.TabIndex = 3;
            this.checkBoxDrawAreasInfos.Text = "Draw view areas infos";
            this.checkBoxDrawAreasInfos.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.checkBoxDofAutoUpdate);
            this.groupBox2.Location = new System.Drawing.Point(15, 119);
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
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.labelPulseDelay);
            this.groupBox3.Controls.Add(this.numericUpDownMinDuration);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.numericUpDownPulseDuration);
            this.groupBox3.Controls.Add(this.numericUpDownMatrixMinDuration);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(15, 13);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(428, 100);
            this.groupBox3.TabIndex = 23;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Pinball";
            // 
            // buttonSaveSettings
            // 
            this.buttonSaveSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveSettings.Location = new System.Drawing.Point(368, 248);
            this.buttonSaveSettings.Name = "buttonSaveSettings";
            this.buttonSaveSettings.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveSettings.TabIndex = 24;
            this.buttonSaveSettings.Text = "Save";
            this.buttonSaveSettings.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(15, 252);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(171, 17);
            this.checkBox1.TabIndex = 25;
            this.checkBox1.Text = "Auto save when leaving toolkit";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 283);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.buttonSaveSettings);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsForm";
            this.Text = "DirectOutput Toolkit Settings";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMatrixMinDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPulseDuration)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        private System.Windows.Forms.NumericUpDown numericUpDownPulseDuration;
        private System.Windows.Forms.Label labelPulseDelay;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxDrawAreasInfos;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBoxDofAutoUpdate;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button buttonSaveSettings;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}