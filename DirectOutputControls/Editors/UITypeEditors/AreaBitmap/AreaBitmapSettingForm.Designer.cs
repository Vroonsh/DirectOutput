namespace DirectOutputControls
{
    partial class AreaBitmapSettingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AreaBitmapSettingForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.propertyGridSettings = new System.Windows.Forms.PropertyGrid();
            this.buttonValidate = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.trackBarZoomFactor = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxImageSelect = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBoxImage = new System.Windows.Forms.PictureBox();
            this.comboBoxFrameSelect = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarZoomFactor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.propertyGridSettings);
            this.splitContainer1.Panel1.Controls.Add(this.buttonValidate);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox3);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(3);
            this.splitContainer1.Size = new System.Drawing.Size(765, 889);
            this.splitContainer1.SplitterDistance = 242;
            this.splitContainer1.TabIndex = 0;
            // 
            // propertyGridSettings
            // 
            this.propertyGridSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGridSettings.Location = new System.Drawing.Point(6, 6);
            this.propertyGridSettings.Name = "propertyGridSettings";
            this.propertyGridSettings.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.propertyGridSettings.Size = new System.Drawing.Size(753, 200);
            this.propertyGridSettings.TabIndex = 3;
            this.propertyGridSettings.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGridSettings_PropertyValueChanged);
            // 
            // buttonValidate
            // 
            this.buttonValidate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonValidate.Location = new System.Drawing.Point(684, 211);
            this.buttonValidate.Name = "buttonValidate";
            this.buttonValidate.Size = new System.Drawing.Size(75, 23);
            this.buttonValidate.TabIndex = 2;
            this.buttonValidate.Text = "Validate";
            this.buttonValidate.UseVisualStyleBackColor = true;
            this.buttonValidate.Click += new System.EventHandler(this.buttonValidate_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.trackBarZoomFactor);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.comboBoxImageSelect);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.pictureBoxImage);
            this.groupBox3.Controls.Add(this.comboBoxFrameSelect);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(759, 637);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Image";
            // 
            // trackBarZoomFactor
            // 
            this.trackBarZoomFactor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.trackBarZoomFactor.AutoSize = false;
            this.trackBarZoomFactor.Location = new System.Drawing.Point(80, 609);
            this.trackBarZoomFactor.Maximum = 20;
            this.trackBarZoomFactor.Minimum = 1;
            this.trackBarZoomFactor.Name = "trackBarZoomFactor";
            this.trackBarZoomFactor.Size = new System.Drawing.Size(205, 19);
            this.trackBarZoomFactor.TabIndex = 6;
            this.trackBarZoomFactor.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarZoomFactor.Value = 1;
            this.trackBarZoomFactor.Scroll += new System.EventHandler(this.trackBarZoomFactor_Scroll);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 609);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Zoom factor";
            // 
            // comboBoxImageSelect
            // 
            this.comboBoxImageSelect.FormattingEnabled = true;
            this.comboBoxImageSelect.Location = new System.Drawing.Point(89, 19);
            this.comboBoxImageSelect.Name = "comboBoxImageSelect";
            this.comboBoxImageSelect.Size = new System.Drawing.Size(184, 21);
            this.comboBoxImageSelect.TabIndex = 4;
            this.comboBoxImageSelect.SelectedIndexChanged += new System.EventHandler(this.comboBoxImageSelect_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Image selector";
            // 
            // pictureBoxImage
            // 
            this.pictureBoxImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxImage.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxImage.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBoxImage.BackgroundImage")));
            this.pictureBoxImage.Location = new System.Drawing.Point(10, 46);
            this.pictureBoxImage.Name = "pictureBoxImage";
            this.pictureBoxImage.Size = new System.Drawing.Size(740, 558);
            this.pictureBoxImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxImage.TabIndex = 2;
            this.pictureBoxImage.TabStop = false;
            this.pictureBoxImage.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxImage_Paint);
            this.pictureBoxImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxImage_MouseDown);
            this.pictureBoxImage.MouseEnter += new System.EventHandler(this.pictureBoxImage_MouseEnter);
            this.pictureBoxImage.MouseLeave += new System.EventHandler(this.pictureBoxImage_MouseLeave);
            this.pictureBoxImage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxImage_MouseMove);
            this.pictureBoxImage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxImage_MouseUp);
            // 
            // comboBoxFrameSelect
            // 
            this.comboBoxFrameSelect.FormattingEnabled = true;
            this.comboBoxFrameSelect.Location = new System.Drawing.Point(361, 19);
            this.comboBoxFrameSelect.Name = "comboBoxFrameSelect";
            this.comboBoxFrameSelect.Size = new System.Drawing.Size(121, 21);
            this.comboBoxFrameSelect.TabIndex = 1;
            this.comboBoxFrameSelect.SelectedIndexChanged += new System.EventHandler(this.comboBoxFrameSelect_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(279, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Frame selector";
            // 
            // AreaBitmapSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 889);
            this.Controls.Add(this.splitContainer1);
            this.Name = "AreaBitmapSettingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AeraBitmap editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AeraBitmapSettingForm_FormClosing);
            this.Load += new System.EventHandler(this.AeraBitmapSettingForm_Load);
            this.Resize += new System.EventHandler(this.AeraBitmapSettingForm_Resize);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarZoomFactor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox comboBoxImageSelect;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBoxImage;
        private System.Windows.Forms.ComboBox comboBoxFrameSelect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar trackBarZoomFactor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PropertyGrid propertyGridSettings;
        private System.Windows.Forms.Button buttonValidate;
    }
}