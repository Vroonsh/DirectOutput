using System.Windows.Forms;

namespace LedControlToolkit
{
    partial class LedControlToolkitDialog
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LedControlToolkitDialog));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.propertyGridEffect = new System.Windows.Forms.PropertyGrid();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageEffectEditor = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.buttonPulseTable = new System.Windows.Forms.Button();
            this.buttonActivationTable = new System.Windows.Forms.Button();
            this.numericUpDownPulseDuration = new System.Windows.Forms.NumericUpDown();
            this.labelPulseDelay = new System.Windows.Forms.Label();
            this.treeViewTableLedEffects = new System.Windows.Forms.TreeView();
            this.imageListIcons = new System.Windows.Forms.ImageList(this.components);
            this.labelRomName = new System.Windows.Forms.Label();
            this.RomNameComboBox = new System.Windows.Forms.ComboBox();
            this.buttonSaveEffect = new System.Windows.Forms.Button();
            this.buttonLoadEffect = new System.Windows.Forms.Button();
            this.buttonNewEffectList = new System.Windows.Forms.Button();
            this.treeViewEffect = new System.Windows.Forms.TreeView();
            this.tabPageSettings = new System.Windows.Forms.TabPage();
            this.buttonSaveSettings = new System.Windows.Forms.Button();
            this.buttonDeleteArea = new System.Windows.Forms.Button();
            this.buttonDuplicateArea = new System.Windows.Forms.Button();
            this.buttonNewArea = new System.Windows.Forms.Button();
            this.listBoxPreviewAreas = new System.Windows.Forms.ListBox();
            this.checkBoxPreviewArea = new System.Windows.Forms.CheckBox();
            this.checkBoxPreviewGrid = new System.Windows.Forms.CheckBox();
            this.panelPreviewLedMatrix = new LedControlToolkit.LedMatrixPreviewControl();
            this.buttonCreateMissingAreas = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPageEffectEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPulseDuration)).BeginInit();
            this.tabPageSettings.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.propertyGridEffect);
            this.splitContainer1.Panel1.Controls.Add(this.tabControlMain);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panelPreviewLedMatrix);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(3);
            this.splitContainer1.Size = new System.Drawing.Size(1698, 1000);
            this.splitContainer1.SplitterDistance = 1136;
            this.splitContainer1.TabIndex = 2;
            // 
            // propertyGridEffect
            // 
            this.propertyGridEffect.Dock = System.Windows.Forms.DockStyle.Right;
            this.propertyGridEffect.Location = new System.Drawing.Point(733, 3);
            this.propertyGridEffect.Name = "propertyGridEffect";
            this.propertyGridEffect.Size = new System.Drawing.Size(400, 994);
            this.propertyGridEffect.TabIndex = 1;
            this.propertyGridEffect.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGridEffect_PropertyValueChanged);
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageEffectEditor);
            this.tabControlMain.Controls.Add(this.tabPageSettings);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(3, 3);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(1130, 994);
            this.tabControlMain.TabIndex = 0;
            // 
            // tabPageEffectEditor
            // 
            this.tabPageEffectEditor.Controls.Add(this.label1);
            this.tabPageEffectEditor.Controls.Add(this.button1);
            this.tabPageEffectEditor.Controls.Add(this.button2);
            this.tabPageEffectEditor.Controls.Add(this.buttonPulseTable);
            this.tabPageEffectEditor.Controls.Add(this.buttonActivationTable);
            this.tabPageEffectEditor.Controls.Add(this.numericUpDownPulseDuration);
            this.tabPageEffectEditor.Controls.Add(this.labelPulseDelay);
            this.tabPageEffectEditor.Controls.Add(this.treeViewTableLedEffects);
            this.tabPageEffectEditor.Controls.Add(this.labelRomName);
            this.tabPageEffectEditor.Controls.Add(this.RomNameComboBox);
            this.tabPageEffectEditor.Controls.Add(this.buttonSaveEffect);
            this.tabPageEffectEditor.Controls.Add(this.buttonLoadEffect);
            this.tabPageEffectEditor.Controls.Add(this.buttonNewEffectList);
            this.tabPageEffectEditor.Controls.Add(this.treeViewEffect);
            this.tabPageEffectEditor.Location = new System.Drawing.Point(4, 22);
            this.tabPageEffectEditor.Name = "tabPageEffectEditor";
            this.tabPageEffectEditor.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEffectEditor.Size = new System.Drawing.Size(1122, 968);
            this.tabPageEffectEditor.TabIndex = 1;
            this.tabPageEffectEditor.Text = "Effect Editor";
            this.tabPageEffectEditor.UseVisualStyleBackColor = true;
            this.tabPageEffectEditor.Enter += new System.EventHandler(this.tabPageEffectEditor_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 484);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Table Effects";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(88, 443);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 22;
            this.button1.Text = "Pulse";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(7, 443);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 21;
            this.button2.Text = "Activate";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // buttonPulseTable
            // 
            this.buttonPulseTable.Location = new System.Drawing.Point(88, 939);
            this.buttonPulseTable.Name = "buttonPulseTable";
            this.buttonPulseTable.Size = new System.Drawing.Size(75, 23);
            this.buttonPulseTable.TabIndex = 20;
            this.buttonPulseTable.Text = "Pulse";
            this.buttonPulseTable.UseVisualStyleBackColor = true;
            this.buttonPulseTable.Click += new System.EventHandler(this.buttonPulseTable_Click);
            // 
            // buttonActivationTable
            // 
            this.buttonActivationTable.Location = new System.Drawing.Point(7, 939);
            this.buttonActivationTable.Name = "buttonActivationTable";
            this.buttonActivationTable.Size = new System.Drawing.Size(75, 23);
            this.buttonActivationTable.TabIndex = 19;
            this.buttonActivationTable.Text = "Activate";
            this.buttonActivationTable.UseVisualStyleBackColor = true;
            this.buttonActivationTable.Click += new System.EventHandler(this.buttonActivationTable_Click);
            // 
            // numericUpDownPulseDuration
            // 
            this.numericUpDownPulseDuration.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownPulseDuration.Location = new System.Drawing.Point(600, 7);
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
            this.numericUpDownPulseDuration.TabIndex = 18;
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
            this.labelPulseDelay.Location = new System.Drawing.Point(518, 9);
            this.labelPulseDelay.Name = "labelPulseDelay";
            this.labelPulseDelay.Size = new System.Drawing.Size(76, 13);
            this.labelPulseDelay.TabIndex = 17;
            this.labelPulseDelay.Text = "Pulse Duration";
            // 
            // treeViewTableLedEffects
            // 
            this.treeViewTableLedEffects.ImageIndex = 0;
            this.treeViewTableLedEffects.ImageList = this.imageListIcons;
            this.treeViewTableLedEffects.Location = new System.Drawing.Point(9, 527);
            this.treeViewTableLedEffects.Name = "treeViewTableLedEffects";
            this.treeViewTableLedEffects.SelectedImageIndex = 0;
            this.treeViewTableLedEffects.Size = new System.Drawing.Size(711, 409);
            this.treeViewTableLedEffects.TabIndex = 16;
            this.treeViewTableLedEffects.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewTableLedEffects_NodeMouseClick);
            // 
            // imageListIcons
            // 
            this.imageListIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListIcons.ImageStream")));
            this.imageListIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListIcons.Images.SetKeyName(0, "red_cross.png");
            this.imageListIcons.Images.SetKeyName(1, "green_gears.png");
            // 
            // labelRomName
            // 
            this.labelRomName.AutoSize = true;
            this.labelRomName.Location = new System.Drawing.Point(6, 500);
            this.labelRomName.Name = "labelRomName";
            this.labelRomName.Size = new System.Drawing.Size(60, 13);
            this.labelRomName.TabIndex = 15;
            this.labelRomName.Text = "RomName:";
            // 
            // RomNameComboBox
            // 
            this.RomNameComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RomNameComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.RomNameComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.RomNameComboBox.FormattingEnabled = true;
            this.RomNameComboBox.Location = new System.Drawing.Point(72, 500);
            this.RomNameComboBox.Name = "RomNameComboBox";
            this.RomNameComboBox.Size = new System.Drawing.Size(648, 21);
            this.RomNameComboBox.TabIndex = 14;
            this.RomNameComboBox.SelectedIndexChanged += new System.EventHandler(this.RomNameComboBox_SelectedIndexChanged);
            // 
            // buttonSaveEffect
            // 
            this.buttonSaveEffect.Location = new System.Drawing.Point(192, 6);
            this.buttonSaveEffect.Name = "buttonSaveEffect";
            this.buttonSaveEffect.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveEffect.TabIndex = 3;
            this.buttonSaveEffect.Text = "Save As...";
            this.buttonSaveEffect.UseVisualStyleBackColor = true;
            // 
            // buttonLoadEffect
            // 
            this.buttonLoadEffect.Location = new System.Drawing.Point(111, 6);
            this.buttonLoadEffect.Name = "buttonLoadEffect";
            this.buttonLoadEffect.Size = new System.Drawing.Size(75, 23);
            this.buttonLoadEffect.TabIndex = 2;
            this.buttonLoadEffect.Text = "Load";
            this.buttonLoadEffect.UseVisualStyleBackColor = true;
            // 
            // buttonNewEffectList
            // 
            this.buttonNewEffectList.Location = new System.Drawing.Point(7, 6);
            this.buttonNewEffectList.Name = "buttonNewEffectList";
            this.buttonNewEffectList.Size = new System.Drawing.Size(98, 23);
            this.buttonNewEffectList.TabIndex = 1;
            this.buttonNewEffectList.Text = "New Effect List";
            this.buttonNewEffectList.UseVisualStyleBackColor = true;
            // 
            // treeViewEffect
            // 
            this.treeViewEffect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewEffect.Location = new System.Drawing.Point(6, 33);
            this.treeViewEffect.Name = "treeViewEffect";
            this.treeViewEffect.Size = new System.Drawing.Size(714, 404);
            this.treeViewEffect.TabIndex = 0;
            // 
            // tabPageSettings
            // 
            this.tabPageSettings.Controls.Add(this.buttonCreateMissingAreas);
            this.tabPageSettings.Controls.Add(this.buttonSaveSettings);
            this.tabPageSettings.Controls.Add(this.buttonDeleteArea);
            this.tabPageSettings.Controls.Add(this.buttonDuplicateArea);
            this.tabPageSettings.Controls.Add(this.buttonNewArea);
            this.tabPageSettings.Controls.Add(this.listBoxPreviewAreas);
            this.tabPageSettings.Controls.Add(this.checkBoxPreviewArea);
            this.tabPageSettings.Controls.Add(this.checkBoxPreviewGrid);
            this.tabPageSettings.Location = new System.Drawing.Point(4, 22);
            this.tabPageSettings.Name = "tabPageSettings";
            this.tabPageSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSettings.Size = new System.Drawing.Size(1122, 968);
            this.tabPageSettings.TabIndex = 3;
            this.tabPageSettings.Text = "Settings";
            this.tabPageSettings.UseVisualStyleBackColor = true;
            this.tabPageSettings.Enter += new System.EventHandler(this.tabPageSettings_Enter);
            // 
            // buttonSaveSettings
            // 
            this.buttonSaveSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveSettings.Location = new System.Drawing.Point(583, 939);
            this.buttonSaveSettings.Name = "buttonSaveSettings";
            this.buttonSaveSettings.Size = new System.Drawing.Size(137, 23);
            this.buttonSaveSettings.TabIndex = 8;
            this.buttonSaveSettings.Text = "Save Settings";
            this.buttonSaveSettings.UseVisualStyleBackColor = true;
            this.buttonSaveSettings.Click += new System.EventHandler(this.buttonSaveSettings_Click);
            // 
            // buttonDeleteArea
            // 
            this.buttonDeleteArea.Location = new System.Drawing.Point(360, 302);
            this.buttonDeleteArea.Name = "buttonDeleteArea";
            this.buttonDeleteArea.Size = new System.Drawing.Size(137, 23);
            this.buttonDeleteArea.TabIndex = 7;
            this.buttonDeleteArea.Text = "Delete Selected Area";
            this.buttonDeleteArea.UseVisualStyleBackColor = true;
            this.buttonDeleteArea.Click += new System.EventHandler(this.buttonDeleteArea_Click);
            // 
            // buttonDuplicateArea
            // 
            this.buttonDuplicateArea.Location = new System.Drawing.Point(217, 301);
            this.buttonDuplicateArea.Name = "buttonDuplicateArea";
            this.buttonDuplicateArea.Size = new System.Drawing.Size(137, 23);
            this.buttonDuplicateArea.TabIndex = 6;
            this.buttonDuplicateArea.Text = "Duplicate Selected Area";
            this.buttonDuplicateArea.UseVisualStyleBackColor = true;
            this.buttonDuplicateArea.Click += new System.EventHandler(this.buttonDuplicateArea_Click);
            // 
            // buttonNewArea
            // 
            this.buttonNewArea.Location = new System.Drawing.Point(136, 301);
            this.buttonNewArea.Name = "buttonNewArea";
            this.buttonNewArea.Size = new System.Drawing.Size(75, 23);
            this.buttonNewArea.TabIndex = 5;
            this.buttonNewArea.Text = "New Area";
            this.buttonNewArea.UseVisualStyleBackColor = true;
            this.buttonNewArea.Click += new System.EventHandler(this.buttonNewArea_Click);
            // 
            // listBoxPreviewAreas
            // 
            this.listBoxPreviewAreas.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxPreviewAreas.FormattingEnabled = true;
            this.listBoxPreviewAreas.Location = new System.Drawing.Point(9, 84);
            this.listBoxPreviewAreas.Name = "listBoxPreviewAreas";
            this.listBoxPreviewAreas.Size = new System.Drawing.Size(711, 212);
            this.listBoxPreviewAreas.TabIndex = 4;
            this.listBoxPreviewAreas.SelectedIndexChanged += new System.EventHandler(this.listBoxPreviewAreas_SelectedIndexChanged);
            // 
            // checkBoxPreviewArea
            // 
            this.checkBoxPreviewArea.AutoSize = true;
            this.checkBoxPreviewArea.Location = new System.Drawing.Point(9, 58);
            this.checkBoxPreviewArea.Name = "checkBoxPreviewArea";
            this.checkBoxPreviewArea.Size = new System.Drawing.Size(129, 17);
            this.checkBoxPreviewArea.TabIndex = 3;
            this.checkBoxPreviewArea.Text = "Display preview areas";
            this.checkBoxPreviewArea.UseVisualStyleBackColor = true;
            this.checkBoxPreviewArea.CheckedChanged += new System.EventHandler(this.checkBoxPreviewArea_CheckedChanged);
            // 
            // checkBoxPreviewGrid
            // 
            this.checkBoxPreviewGrid.AutoSize = true;
            this.checkBoxPreviewGrid.Location = new System.Drawing.Point(9, 35);
            this.checkBoxPreviewGrid.Name = "checkBoxPreviewGrid";
            this.checkBoxPreviewGrid.Size = new System.Drawing.Size(121, 17);
            this.checkBoxPreviewGrid.TabIndex = 2;
            this.checkBoxPreviewGrid.Text = "Display ledstrips grid";
            this.checkBoxPreviewGrid.UseVisualStyleBackColor = true;
            this.checkBoxPreviewGrid.CheckedChanged += new System.EventHandler(this.checkBoxPreviewGrid_CheckedChanged);
            // 
            // panelPreviewLedMatrix
            // 
            this.panelPreviewLedMatrix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPreviewLedMatrix.Location = new System.Drawing.Point(3, 3);
            this.panelPreviewLedMatrix.Name = "panelPreviewLedMatrix";
            this.panelPreviewLedMatrix.Size = new System.Drawing.Size(552, 994);
            this.panelPreviewLedMatrix.TabIndex = 0;
            // 
            // buttonCreateMissingAreas
            // 
            this.buttonCreateMissingAreas.Location = new System.Drawing.Point(9, 301);
            this.buttonCreateMissingAreas.Name = "buttonCreateMissingAreas";
            this.buttonCreateMissingAreas.Size = new System.Drawing.Size(121, 23);
            this.buttonCreateMissingAreas.TabIndex = 9;
            this.buttonCreateMissingAreas.Text = "Create Missing Areas";
            this.buttonCreateMissingAreas.UseVisualStyleBackColor = true;
            this.buttonCreateMissingAreas.Click += new System.EventHandler(this.buttonCreateMissingAreas_Click);
            // 
            // LedControlToolkitDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1698, 1000);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LedControlToolkitDialog";
            this.Text = "LedControl Toolkit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LedControlToolkit_FormClosing);
            this.Load += new System.EventHandler(this.LedControlToolkit_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControlMain.ResumeLayout(false);
            this.tabPageEffectEditor.ResumeLayout(false);
            this.tabPageEffectEditor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPulseDuration)).EndInit();
            this.tabPageSettings.ResumeLayout(false);
            this.tabPageSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private SplitContainer splitContainer1;

        private PropertyGrid propertyGridEffect;
        private LedMatrixPreviewControl panelPreviewLedMatrix;
        private TabControl tabControlMain;

        #region Effect Editor Tab
        private TabPage tabPageEffectEditor;
        private Button buttonSaveEffect;
        private Button buttonLoadEffect;
        private Button buttonNewEffectList;
        private TreeView treeViewEffect;
        private Label labelRomName;
        private ComboBox RomNameComboBox;
        #endregion

        #region Settings Tab
        private TabPage tabPageSettings;
        #endregion
        private CheckBox checkBoxPreviewArea;
        private CheckBox checkBoxPreviewGrid;
        private ListBox listBoxPreviewAreas;
        private Button buttonNewArea;
        private Button buttonDeleteArea;
        private Button buttonDuplicateArea;
        private Button buttonSaveSettings;
        private TreeView treeViewTableLedEffects;
        private NumericUpDown numericUpDownPulseDuration;
        private Label labelPulseDelay;
        private Button button1;
        private Button button2;
        private Button buttonPulseTable;
        private Button buttonActivationTable;
        private Label label1;
        private ImageList imageListIcons;
        private Button buttonCreateMissingAreas;
    }
}

