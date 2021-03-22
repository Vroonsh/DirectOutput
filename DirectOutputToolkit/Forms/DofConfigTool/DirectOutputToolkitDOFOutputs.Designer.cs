namespace DirectOutputToolkit
{
    partial class DirectOutputToolkitDOFOutputs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DirectOutputToolkitDOFOutputs));
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxOutput = new System.Windows.Forms.ComboBox();
            this.richTextBoxDOFCommand = new System.Windows.Forms.RichTextBox();
            this.checkBoxFullRangeIntensity = new System.Windows.Forms.CheckBox();
            this.checkBoxSortEffects = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "DofConfigTool Output";
            // 
            // comboBoxOutput
            // 
            this.comboBoxOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxOutput.FormattingEnabled = true;
            this.comboBoxOutput.Location = new System.Drawing.Point(129, 4);
            this.comboBoxOutput.Name = "comboBoxOutput";
            this.comboBoxOutput.Size = new System.Drawing.Size(517, 21);
            this.comboBoxOutput.TabIndex = 2;
            this.comboBoxOutput.SelectedIndexChanged += new System.EventHandler(this.comboBoxOutput_SelectedIndexChanged);
            // 
            // richTextBoxDOFCommand
            // 
            this.richTextBoxDOFCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxDOFCommand.Location = new System.Drawing.Point(4, 63);
            this.richTextBoxDOFCommand.Name = "richTextBoxDOFCommand";
            this.richTextBoxDOFCommand.Size = new System.Drawing.Size(642, 455);
            this.richTextBoxDOFCommand.TabIndex = 3;
            this.richTextBoxDOFCommand.Text = "";
            // 
            // checkBoxFullRangeIntensity
            // 
            this.checkBoxFullRangeIntensity.AutoSize = true;
            this.checkBoxFullRangeIntensity.Location = new System.Drawing.Point(4, 40);
            this.checkBoxFullRangeIntensity.Name = "checkBoxFullRangeIntensity";
            this.checkBoxFullRangeIntensity.Size = new System.Drawing.Size(140, 17);
            this.checkBoxFullRangeIntensity.TabIndex = 4;
            this.checkBoxFullRangeIntensity.Text = "Use full range intensities";
            this.checkBoxFullRangeIntensity.UseVisualStyleBackColor = true;
            this.checkBoxFullRangeIntensity.CheckedChanged += new System.EventHandler(this.checkBoxFullRangeIntensity_CheckedChanged);
            // 
            // checkBoxSortEffects
            // 
            this.checkBoxSortEffects.AutoSize = true;
            this.checkBoxSortEffects.Checked = true;
            this.checkBoxSortEffects.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSortEffects.Location = new System.Drawing.Point(150, 40);
            this.checkBoxSortEffects.Name = "checkBoxSortEffects";
            this.checkBoxSortEffects.Size = new System.Drawing.Size(139, 17);
            this.checkBoxSortEffects.TabIndex = 5;
            this.checkBoxSortEffects.Text = "Sort effects by start time";
            this.checkBoxSortEffects.UseVisualStyleBackColor = true;
            this.checkBoxSortEffects.CheckedChanged += new System.EventHandler(this.checkBoxSortEffects_CheckedChanged);
            // 
            // DirectOutputToolkitDOFOutputs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 519);
            this.Controls.Add(this.checkBoxSortEffects);
            this.Controls.Add(this.checkBoxFullRangeIntensity);
            this.Controls.Add(this.richTextBoxDOFCommand);
            this.Controls.Add(this.comboBoxOutput);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DirectOutputToolkitDOFOutputs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export DofConfigTool commands";
            this.Load += new System.EventHandler(this.LedControlToolkitDOFOutputs_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxOutput;
        private System.Windows.Forms.RichTextBox richTextBoxDOFCommand;
        private System.Windows.Forms.CheckBox checkBoxFullRangeIntensity;
        private System.Windows.Forms.CheckBox checkBoxSortEffects;
    }
}