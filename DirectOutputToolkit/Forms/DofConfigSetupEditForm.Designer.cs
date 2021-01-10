
namespace DirectOutputToolkit
{
    partial class DofConfigSetupEditForm
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
            DofConfigToolWrapper.DofConfigToolSetup dofConfigToolSetup1 = new DofConfigToolWrapper.DofConfigToolSetup();
            this.dofConfigToolSetupControl1 = new DofConfigToolWrapper.DofConfigToolSetupControl();
            this.buttonOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // dofConfigToolSetupControl1
            // 
            this.dofConfigToolSetupControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dofConfigToolSetup1.APIKey = "";
            dofConfigToolSetup1.UserName = "";
            this.dofConfigToolSetupControl1.DofConfigToolSetup = dofConfigToolSetup1;
            this.dofConfigToolSetupControl1.FileName = "";
            this.dofConfigToolSetupControl1.Location = new System.Drawing.Point(12, 12);
            this.dofConfigToolSetupControl1.Name = "dofConfigToolSetupControl1";
            this.dofConfigToolSetupControl1.Size = new System.Drawing.Size(974, 661);
            this.dofConfigToolSetupControl1.TabIndex = 0;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(824, 679);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(162, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // DofConfigSetupEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(998, 711);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.dofConfigToolSetupControl1);
            this.Name = "DofConfigSetupEditForm";
            this.Text = "DofConfig Setup Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DofConfigSetupEditForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private DofConfigToolWrapper.DofConfigToolSetupControl dofConfigToolSetupControl1;
        private System.Windows.Forms.Button buttonOK;
    }
}