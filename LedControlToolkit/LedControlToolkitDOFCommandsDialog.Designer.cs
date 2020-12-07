namespace LedControlToolkit
{
    partial class LedControlToolkitDOFCommandsDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonParse = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.listBoxCommandLines = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonGenerate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(243, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Paste or write here a DofConfigTool command line";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(262, 56);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(279, 21);
            this.comboBox1.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(16, 30);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(525, 20);
            this.textBox1.TabIndex = 2;
            // 
            // buttonParse
            // 
            this.buttonParse.Location = new System.Drawing.Point(548, 54);
            this.buttonParse.Name = "buttonParse";
            this.buttonParse.Size = new System.Drawing.Size(64, 23);
            this.buttonParse.TabIndex = 3;
            this.buttonParse.Text = "Parse";
            this.buttonParse.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(221, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Select output the command will be applied on";
            // 
            // listBoxCommandLines
            // 
            this.listBoxCommandLines.FormattingEnabled = true;
            this.listBoxCommandLines.Location = new System.Drawing.Point(19, 114);
            this.listBoxCommandLines.Name = "listBoxCommandLines";
            this.listBoxCommandLines.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.listBoxCommandLines.Size = new System.Drawing.Size(593, 290);
            this.listBoxCommandLines.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Commands list";
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.Location = new System.Drawing.Point(482, 410);
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.Size = new System.Drawing.Size(130, 23);
            this.buttonGenerate.TabIndex = 7;
            this.buttonGenerate.Text = "Generate effects";
            this.buttonGenerate.UseVisualStyleBackColor = true;
            // 
            // LedControlToolkitDOFCommandsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 443);
            this.Controls.Add(this.buttonGenerate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listBoxCommandLines);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonParse);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Name = "LedControlToolkitDOFCommandsDialog";
            this.Text = "LedControlToolkitDOFCommandsDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button buttonParse;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBoxCommandLines;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonGenerate;
    }
}