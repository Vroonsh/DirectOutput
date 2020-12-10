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
            this.buttonExportDOF = new System.Windows.Forms.Button();
            this.buttonImportDOF = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonPulseEdition = new System.Windows.Forms.Button();
            this.buttonActivationEdition = new System.Windows.Forms.Button();
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
            this.buttonNewEditionTable = new System.Windows.Forms.Button();
            this.treeViewEditionTable = new System.Windows.Forms.TreeView();
            this.tabPageSettings = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownMatrixMinDuration = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDownMinDuration = new System.Windows.Forms.NumericUpDown();
            this.buttonCreateMissingAreas = new System.Windows.Forms.Button();
            this.buttonSaveSettings = new System.Windows.Forms.Button();
            this.buttonDeleteArea = new System.Windows.Forms.Button();
            this.buttonDuplicateArea = new System.Windows.Forms.Button();
            this.buttonNewArea = new System.Windows.Forms.Button();
            this.listBoxPreviewAreas = new System.Windows.Forms.ListBox();
            this.checkBoxPreviewArea = new System.Windows.Forms.CheckBox();
            this.checkBoxPreviewGrid = new System.Windows.Forms.CheckBox();
            this.checkBoxDebugEffects = new System.Windows.Forms.CheckBox();
            this.panelPreviewLedMatrix = new LedControlToolkit.LedMatrixPreviewControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPageEffectEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPulseDuration)).BeginInit();
            this.tabPageSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMatrixMinDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinDuration)).BeginInit();
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
            this.splitContainer1.Panel2.Controls.Add(this.checkBoxDebugEffects);
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
            this.tabPageEffectEditor.Controls.Add(this.buttonExportDOF);
            this.tabPageEffectEditor.Controls.Add(this.buttonImportDOF);
            this.tabPageEffectEditor.Controls.Add(this.label1);
            this.tabPageEffectEditor.Controls.Add(this.buttonPulseEdition);
            this.tabPageEffectEditor.Controls.Add(this.buttonActivationEdition);
            this.tabPageEffectEditor.Controls.Add(this.buttonPulseTable);
            this.tabPageEffectEditor.Controls.Add(this.buttonActivationTable);
            this.tabPageEffectEditor.Controls.Add(this.numericUpDownPulseDuration);
            this.tabPageEffectEditor.Controls.Add(this.labelPulseDelay);
            this.tabPageEffectEditor.Controls.Add(this.treeViewTableLedEffects);
            this.tabPageEffectEditor.Controls.Add(this.labelRomName);
            this.tabPageEffectEditor.Controls.Add(this.RomNameComboBox);
            this.tabPageEffectEditor.Controls.Add(this.buttonSaveEffect);
            this.tabPageEffectEditor.Controls.Add(this.buttonLoadEffect);
            this.tabPageEffectEditor.Controls.Add(this.buttonNewEditionTable);
            this.tabPageEffectEditor.Controls.Add(this.treeViewEditionTable);
            this.tabPageEffectEditor.Location = new System.Drawing.Point(4, 22);
            this.tabPageEffectEditor.Name = "tabPageEffectEditor";
            this.tabPageEffectEditor.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEffectEditor.Size = new System.Drawing.Size(1122, 968);
            this.tabPageEffectEditor.TabIndex = 1;
            this.tabPageEffectEditor.Text = "Effect Editor";
            this.tabPageEffectEditor.UseVisualStyleBackColor = true;
            this.tabPageEffectEditor.Enter += new System.EventHandler(this.tabPageEffectEditor_Enter);
            // 
            // buttonExportDOF
            // 
            this.buttonExportDOF.Location = new System.Drawing.Point(354, 6);
            this.buttonExportDOF.Name = "buttonExportDOF";
            this.buttonExportDOF.Size = new System.Drawing.Size(75, 23);
            this.buttonExportDOF.TabIndex = 25;
            this.buttonExportDOF.Text = "Export DOF";
            this.buttonExportDOF.UseVisualStyleBackColor = true;
            this.buttonExportDOF.Click += new System.EventHandler(this.buttonExportDOF_Click);
            // 
            // buttonImportDOF
            // 
            this.buttonImportDOF.Location = new System.Drawing.Point(273, 6);
            this.buttonImportDOF.Name = "buttonImportDOF";
            this.buttonImportDOF.Size = new System.Drawing.Size(75, 23);
            this.buttonImportDOF.TabIndex = 24;
            this.buttonImportDOF.Text = "Import DOF";
            this.buttonImportDOF.UseVisualStyleBackColor = true;
            this.buttonImportDOF.Click += new System.EventHandler(this.buttonImportDOF_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 484);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "DofConfigTool Table Effects";
            // 
            // buttonPulseEdition
            // 
            this.buttonPulseEdition.Location = new System.Drawing.Point(88, 443);
            this.buttonPulseEdition.Name = "buttonPulseEdition";
            this.buttonPulseEdition.Size = new System.Drawing.Size(75, 23);
            this.buttonPulseEdition.TabIndex = 22;
            this.buttonPulseEdition.Text = "Pulse";
            this.buttonPulseEdition.UseVisualStyleBackColor = true;
            this.buttonPulseEdition.Click += new System.EventHandler(this.buttonPulseEdition_Click);
            // 
            // buttonActivationEdition
            // 
            this.buttonActivationEdition.Location = new System.Drawing.Point(7, 443);
            this.buttonActivationEdition.Name = "buttonActivationEdition";
            this.buttonActivationEdition.Size = new System.Drawing.Size(75, 23);
            this.buttonActivationEdition.TabIndex = 21;
            this.buttonActivationEdition.Text = "Activate";
            this.buttonActivationEdition.UseVisualStyleBackColor = true;
            this.buttonActivationEdition.Click += new System.EventHandler(this.buttonActivationEdition_Click);
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
            this.treeViewTableLedEffects.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewTableLedEffects_AfterSelect);
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
            this.buttonSaveEffect.Click += new System.EventHandler(this.buttonSaveEffect_Click);
            // 
            // buttonLoadEffect
            // 
            this.buttonLoadEffect.Location = new System.Drawing.Point(111, 6);
            this.buttonLoadEffect.Name = "buttonLoadEffect";
            this.buttonLoadEffect.Size = new System.Drawing.Size(75, 23);
            this.buttonLoadEffect.TabIndex = 2;
            this.buttonLoadEffect.Text = "Load";
            this.buttonLoadEffect.UseVisualStyleBackColor = true;
            this.buttonLoadEffect.Click += new System.EventHandler(this.buttonLoadEffect_Click);
            // 
            // buttonNewEditionTable
            // 
            this.buttonNewEditionTable.Location = new System.Drawing.Point(7, 6);
            this.buttonNewEditionTable.Name = "buttonNewEditionTable";
            this.buttonNewEditionTable.Size = new System.Drawing.Size(98, 23);
            this.buttonNewEditionTable.TabIndex = 1;
            this.buttonNewEditionTable.Text = "New Table";
            this.buttonNewEditionTable.UseVisualStyleBackColor = true;
            this.buttonNewEditionTable.Click += new System.EventHandler(this.buttonNewEditionTable_Click);
            // 
            // treeViewEditionTable
            // 
            this.treeViewEditionTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewEditionTable.Location = new System.Drawing.Point(7, 35);
            this.treeViewEditionTable.Name = "treeViewEditionTable";
            this.treeViewEditionTable.Size = new System.Drawing.Size(714, 404);
            this.treeViewEditionTable.TabIndex = 0;
            this.treeViewEditionTable.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewEditionTable_AfterSelect);
            this.treeViewEditionTable.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewEffect_NodeMouseClick);
            this.treeViewEditionTable.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeViewEffect_KeyDown);
            // 
            // tabPageSettings
            // 
            this.tabPageSettings.Controls.Add(this.label3);
            this.tabPageSettings.Controls.Add(this.numericUpDownMatrixMinDuration);
            this.tabPageSettings.Controls.Add(this.label2);
            this.tabPageSettings.Controls.Add(this.numericUpDownMinDuration);
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(270, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(147, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Minimal Matrix Effect Duration";
            // 
            // numericUpDownMatrixMinDuration
            // 
            this.numericUpDownMatrixMinDuration.Location = new System.Drawing.Point(423, 79);
            this.numericUpDownMatrixMinDuration.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownMatrixMinDuration.Name = "numericUpDownMatrixMinDuration";
            this.numericUpDownMatrixMinDuration.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownMatrixMinDuration.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Minimal Effect Duration";
            // 
            // numericUpDownMinDuration
            // 
            this.numericUpDownMinDuration.Location = new System.Drawing.Point(131, 77);
            this.numericUpDownMinDuration.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownMinDuration.Name = "numericUpDownMinDuration";
            this.numericUpDownMinDuration.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownMinDuration.TabIndex = 10;
            // 
            // buttonCreateMissingAreas
            // 
            this.buttonCreateMissingAreas.Location = new System.Drawing.Point(9, 322);
            this.buttonCreateMissingAreas.Name = "buttonCreateMissingAreas";
            this.buttonCreateMissingAreas.Size = new System.Drawing.Size(121, 23);
            this.buttonCreateMissingAreas.TabIndex = 9;
            this.buttonCreateMissingAreas.Text = "Create Missing Areas";
            this.buttonCreateMissingAreas.UseVisualStyleBackColor = true;
            this.buttonCreateMissingAreas.Click += new System.EventHandler(this.buttonCreateMissingAreas_Click);
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
            this.buttonDeleteArea.Location = new System.Drawing.Point(360, 323);
            this.buttonDeleteArea.Name = "buttonDeleteArea";
            this.buttonDeleteArea.Size = new System.Drawing.Size(137, 23);
            this.buttonDeleteArea.TabIndex = 7;
            this.buttonDeleteArea.Text = "Delete Selected Area";
            this.buttonDeleteArea.UseVisualStyleBackColor = true;
            this.buttonDeleteArea.Click += new System.EventHandler(this.buttonDeleteArea_Click);
            // 
            // buttonDuplicateArea
            // 
            this.buttonDuplicateArea.Location = new System.Drawing.Point(217, 322);
            this.buttonDuplicateArea.Name = "buttonDuplicateArea";
            this.buttonDuplicateArea.Size = new System.Drawing.Size(137, 23);
            this.buttonDuplicateArea.TabIndex = 6;
            this.buttonDuplicateArea.Text = "Duplicate Selected Area";
            this.buttonDuplicateArea.UseVisualStyleBackColor = true;
            this.buttonDuplicateArea.Click += new System.EventHandler(this.buttonDuplicateArea_Click);
            // 
            // buttonNewArea
            // 
            this.buttonNewArea.Location = new System.Drawing.Point(136, 322);
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
            this.listBoxPreviewAreas.Location = new System.Drawing.Point(9, 105);
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
            // checkBoxDebugEffects
            // 
            this.checkBoxDebugEffects.AutoSize = true;
            this.checkBoxDebugEffects.Enabled = false;
            this.checkBoxDebugEffects.Location = new System.Drawing.Point(7, 968);
            this.checkBoxDebugEffects.Name = "checkBoxDebugEffects";
            this.checkBoxDebugEffects.Size = new System.Drawing.Size(161, 17);
            this.checkBoxDebugEffects.TabIndex = 1;
            this.checkBoxDebugEffects.Text = "Show pinball && effects status";
            this.checkBoxDebugEffects.UseVisualStyleBackColor = true;
            this.checkBoxDebugEffects.CheckedChanged += new System.EventHandler(this.checkBoxDebugEffects_CheckedChanged);
            // 
            // panelPreviewLedMatrix
            // 
            this.panelPreviewLedMatrix.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelPreviewLedMatrix.Location = new System.Drawing.Point(3, 3);
            this.panelPreviewLedMatrix.Name = "panelPreviewLedMatrix";
            this.panelPreviewLedMatrix.Size = new System.Drawing.Size(552, 958);
            this.panelPreviewLedMatrix.TabIndex = 0;
            this.panelPreviewLedMatrix.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.panelPreviewLedMatrix_ControlRemoved);
            // 
            // LedControlToolkitDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1698, 1000);
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LedControlToolkitDialog";
            this.Text = "LedControl Toolkit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LedControlToolkit_FormClosing);
            this.Load += new System.EventHandler(this.LedControlToolkit_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControlMain.ResumeLayout(false);
            this.tabPageEffectEditor.ResumeLayout(false);
            this.tabPageEffectEditor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPulseDuration)).EndInit();
            this.tabPageSettings.ResumeLayout(false);
            this.tabPageSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMatrixMinDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinDuration)).EndInit();
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
        private Button buttonNewEditionTable;
        private TreeView treeViewEditionTable;
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
        private Button buttonPulseEdition;
        private Button buttonActivationEdition;
        private Button buttonPulseTable;
        private Button buttonActivationTable;
        private Label label1;
        private ImageList imageListIcons;
        private Button buttonCreateMissingAreas;
        private Button buttonExportDOF;
        private Button buttonImportDOF;
        private CheckBox checkBoxDebugEffects;
        private Label label3;
        private NumericUpDown numericUpDownMatrixMinDuration;
        private Label label2;
        private NumericUpDown numericUpDownMinDuration;
    }
}

