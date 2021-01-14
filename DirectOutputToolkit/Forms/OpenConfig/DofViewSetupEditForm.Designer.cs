
namespace DirectOutputToolkit
{
    partial class DofViewSetupEditForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DofViewSetupEditForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.buttonOK = new System.Windows.Forms.Button();
            this.checkBoxDrawAreasInfos = new System.Windows.Forms.CheckBox();
            this.directOutputViewSetupControl1 = new DirectOutputControls.DirectOutputViewSetupControl();
            this.directOutputPreviewControl1 = new DirectOutputControls.DirectOutputPreviewControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.directOutputViewSetupControl1);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer1.Panel2.Controls.Add(this.directOutputPreviewControl1);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(3);
            this.splitContainer1.Size = new System.Drawing.Size(1261, 886);
            this.splitContainer1.SplitterDistance = 788;
            this.splitContainer1.TabIndex = 0;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(1064, 892);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(187, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // checkBoxDrawAreasInfos
            // 
            this.checkBoxDrawAreasInfos.AutoSize = true;
            this.checkBoxDrawAreasInfos.Checked = true;
            this.checkBoxDrawAreasInfos.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDrawAreasInfos.Location = new System.Drawing.Point(795, 891);
            this.checkBoxDrawAreasInfos.Name = "checkBoxDrawAreasInfos";
            this.checkBoxDrawAreasInfos.Size = new System.Drawing.Size(130, 17);
            this.checkBoxDrawAreasInfos.TabIndex = 2;
            this.checkBoxDrawAreasInfos.Text = "Draw view areas infos";
            this.checkBoxDrawAreasInfos.UseVisualStyleBackColor = true;
            this.checkBoxDrawAreasInfos.CheckedChanged += new System.EventHandler(this.checkBoxDrawAreasInfos_CheckedChanged);
            // 
            // directOutputViewSetupControl1
            // 
            this.directOutputViewSetupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.directOutputViewSetupControl1.FileName = "";
            this.directOutputViewSetupControl1.Location = new System.Drawing.Point(3, 3);
            this.directOutputViewSetupControl1.Name = "directOutputViewSetupControl1";
            this.directOutputViewSetupControl1.SetupChanged = null;
            this.directOutputViewSetupControl1.Size = new System.Drawing.Size(782, 880);
            this.directOutputViewSetupControl1.TabIndex = 0;
            // 
            // directOutputPreviewControl1
            // 
            this.directOutputPreviewControl1.AreaDisplayColor = System.Drawing.Color.Green;
            this.directOutputPreviewControl1.BackgroundColor = System.Drawing.Color.MidnightBlue;
            this.directOutputPreviewControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.directOutputPreviewControl1.DrawViewAreasInfos = false;
            this.directOutputPreviewControl1.Location = new System.Drawing.Point(3, 3);
            this.directOutputPreviewControl1.Name = "directOutputPreviewControl1";
            this.directOutputPreviewControl1.Size = new System.Drawing.Size(463, 880);
            this.directOutputPreviewControl1.TabIndex = 0;
            // 
            // DofViewSetupEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1263, 920);
            this.Controls.Add(this.checkBoxDrawAreasInfos);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DofViewSetupEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Direct Output View Setup Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DofViewSetupEditForm_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private DirectOutputControls.DirectOutputViewSetupControl directOutputViewSetupControl1;
        private DirectOutputControls.DirectOutputPreviewControl directOutputPreviewControl1;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.CheckBox checkBoxDrawAreasInfos;
    }
}