namespace DirectOutputToolkit
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
            this.checkBoxDisplayAreas = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.treeViewVisibility = new System.Windows.Forms.TreeView();
            this.directOutputPreviewControl1 = new DirectOutputControls.DirectOutputPreviewControl();
            this.imageListDefaultIcons = new System.Windows.Forms.ImageList(this.components);
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
            this.splitContainer1.Panel1.Controls.Add(this.checkBoxDisplayAreas);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.treeViewVisibility);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.directOutputPreviewControl1);
            this.splitContainer1.Size = new System.Drawing.Size(687, 926);
            this.splitContainer1.SplitterDistance = 141;
            this.splitContainer1.TabIndex = 0;
            // 
            // checkBoxDisplayAreas
            // 
            this.checkBoxDisplayAreas.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxDisplayAreas.AutoSize = true;
            this.checkBoxDisplayAreas.Location = new System.Drawing.Point(12, 905);
            this.checkBoxDisplayAreas.Name = "checkBoxDisplayAreas";
            this.checkBoxDisplayAreas.Size = new System.Drawing.Size(114, 17);
            this.checkBoxDisplayAreas.TabIndex = 2;
            this.checkBoxDisplayAreas.Text = "Display areas infos";
            this.checkBoxDisplayAreas.UseVisualStyleBackColor = true;
            this.checkBoxDisplayAreas.CheckedChanged += new System.EventHandler(this.checkBoxDisplayAreas_CheckedChanged);
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
            this.treeViewVisibility.Size = new System.Drawing.Size(125, 874);
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
            this.directOutputPreviewControl1.Size = new System.Drawing.Size(542, 926);
            this.directOutputPreviewControl1.TabIndex = 0;
            // 
            // imageListDefaultIcons
            // 
            this.imageListDefaultIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDefaultIcons.ImageStream")));
            this.imageListDefaultIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListDefaultIcons.Images.SetKeyName(0, "AuthLaunchButton");
            this.imageListDefaultIcons.Images.SetKeyName(1, "Bell");
            this.imageListDefaultIcons.Images.SetKeyName(2, "BumperCap");
            this.imageListDefaultIcons.Images.SetKeyName(3, "BumperCapKiss1");
            this.imageListDefaultIcons.Images.SetKeyName(4, "BumperCapKiss2");
            this.imageListDefaultIcons.Images.SetKeyName(5, "BumperCapKiss3");
            this.imageListDefaultIcons.Images.SetKeyName(6, "BumperCapKiss4");
            this.imageListDefaultIcons.Images.SetKeyName(7, "ButtonBackground");
            this.imageListDefaultIcons.Images.SetKeyName(8, "Chime");
            this.imageListDefaultIcons.Images.SetKeyName(9, "Coin");
            this.imageListDefaultIcons.Images.SetKeyName(10, "Exit");
            this.imageListDefaultIcons.Images.SetKeyName(11, "ExtraBall");
            this.imageListDefaultIcons.Images.SetKeyName(12, "Fan");
            this.imageListDefaultIcons.Images.SetKeyName(13, "FireButton");
            this.imageListDefaultIcons.Images.SetKeyName(14, "Gear");
            this.imageListDefaultIcons.Images.SetKeyName(15, "Genre");
            this.imageListDefaultIcons.Images.SetKeyName(16, "BeaconBlue");
            this.imageListDefaultIcons.Images.SetKeyName(17, "BeaconRed");
            this.imageListDefaultIcons.Images.SetKeyName(18, "HowToPlay");
            this.imageListDefaultIcons.Images.SetKeyName(19, "Knocker");
            this.imageListDefaultIcons.Images.SetKeyName(20, "LaunchButton");
            this.imageListDefaultIcons.Images.SetKeyName(21, "LeftFlipper");
            this.imageListDefaultIcons.Images.SetKeyName(22, "RightFlipper");
            this.imageListDefaultIcons.Images.SetKeyName(23, "Shaker");
            this.imageListDefaultIcons.Images.SetKeyName(24, "SlingShotLeft");
            this.imageListDefaultIcons.Images.SetKeyName(25, "SlingShotRight");
            this.imageListDefaultIcons.Images.SetKeyName(26, "StartButton");
            this.imageListDefaultIcons.Images.SetKeyName(27, "ZBLaunchButton");
            // 
            // DirectOutputToolkitPreviewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(687, 926);
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
        private System.Windows.Forms.CheckBox checkBoxDisplayAreas;
        public System.Windows.Forms.ImageList imageListDefaultIcons;
    }
}