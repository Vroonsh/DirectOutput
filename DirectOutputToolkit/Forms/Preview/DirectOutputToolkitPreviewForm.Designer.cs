﻿namespace DirectOutputToolkit
{
    partial class DirectOutputToolkitPreviewForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DirectOutputToolkitPreviewForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.treeViewVisibility = new System.Windows.Forms.TreeView();
            this.directOutputPreviewControl1 = new DirectOutputControls.DirectOutputPreviewControl();
            this.imageListIcons = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.treeViewVisibility);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.directOutputPreviewControl1);
            this.splitContainer1.Size = new System.Drawing.Size(778, 999);
            this.splitContainer1.SplitterDistance = 183;
            this.splitContainer1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Areas visibility";
            // 
            // treeViewVisibility
            // 
            this.treeViewVisibility.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewVisibility.CheckBoxes = true;
            this.treeViewVisibility.Location = new System.Drawing.Point(12, 25);
            this.treeViewVisibility.Name = "treeViewVisibility";
            this.treeViewVisibility.Size = new System.Drawing.Size(168, 971);
            this.treeViewVisibility.TabIndex = 0;
            this.treeViewVisibility.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeViewVisibility_AfterCheck);
            // 
            // directOutputPreviewControl1
            // 
            this.directOutputPreviewControl1.AreaDisplayColor = System.Drawing.Color.Green;
            this.directOutputPreviewControl1.BackgroundColor = System.Drawing.Color.MidnightBlue;
            this.directOutputPreviewControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.directOutputPreviewControl1.DrawViewAreasInfos = false;
            this.directOutputPreviewControl1.Location = new System.Drawing.Point(0, 0);
            this.directOutputPreviewControl1.Name = "directOutputPreviewControl1";
            this.directOutputPreviewControl1.Size = new System.Drawing.Size(591, 999);
            this.directOutputPreviewControl1.TabIndex = 0;
            // 
            // imageListIcons
            // 
            this.imageListIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListIcons.ImageStream")));
            this.imageListIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListIcons.Images.SetKeyName(0, "ButtonBackground");
            this.imageListIcons.Images.SetKeyName(1, "StartButton");
            this.imageListIcons.Images.SetKeyName(2, "LaunchButton");
            this.imageListIcons.Images.SetKeyName(3, "AuthLaunchButton");
            this.imageListIcons.Images.SetKeyName(4, "ZBLaunchButton");
            this.imageListIcons.Images.SetKeyName(5, "FireButton");
            this.imageListIcons.Images.SetKeyName(6, "Coin");
            this.imageListIcons.Images.SetKeyName(7, "Exit");
            this.imageListIcons.Images.SetKeyName(8, "ExtraBall");
            this.imageListIcons.Images.SetKeyName(9, "Genre");
            this.imageListIcons.Images.SetKeyName(10, "HowToPlay");
            this.imageListIcons.Images.SetKeyName(11, "BumperCap");
            this.imageListIcons.Images.SetKeyName(12, "BumperCapKiss1");
            this.imageListIcons.Images.SetKeyName(13, "BumperCapKiss2");
            this.imageListIcons.Images.SetKeyName(14, "BumperCapKiss3");
            this.imageListIcons.Images.SetKeyName(15, "BumperCapKiss4");
            this.imageListIcons.Images.SetKeyName(16, "Bell");
            this.imageListIcons.Images.SetKeyName(17, "Fan");
            this.imageListIcons.Images.SetKeyName(18, "GyroBlue");
            this.imageListIcons.Images.SetKeyName(19, "GyroRed");
            this.imageListIcons.Images.SetKeyName(20, "SlingShotLeft");
            this.imageListIcons.Images.SetKeyName(21, "SlingShotRight");
            this.imageListIcons.Images.SetKeyName(22, "Gear");
            this.imageListIcons.Images.SetKeyName(23, "LeftFlipper");
            this.imageListIcons.Images.SetKeyName(24, "RightFlipper");
            this.imageListIcons.Images.SetKeyName(25, "Shaker");
            this.imageListIcons.Images.SetKeyName(26, "Knocker");
            this.imageListIcons.Images.SetKeyName(27, "Chime");
            // 
            // DirectOutputToolkitPreviewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 999);
            this.ControlBox = false;
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.Name = "DirectOutputToolkitPreviewForm";
            this.ShowInTaskbar = false;
            this.Text = "DirectOutput Toolkit Preview";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView treeViewVisibility;
        private DirectOutputControls.DirectOutputPreviewControl directOutputPreviewControl1;
        private System.Windows.Forms.ImageList imageListIcons;
    }
}