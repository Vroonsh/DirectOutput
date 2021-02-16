namespace DirectOutputToolkit
{
    partial class DirectOutputToolkitForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DirectOutputToolkitForm));
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.DofEditionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DofConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importFromDofConfigToolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToDofConfigToolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.buttonEditionTableClearFilter = new System.Windows.Forms.Button();
            this.comboBoxEditionTableOutputFilter = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonPulseEdition = new System.Windows.Forms.Button();
            this.buttonActivationEdition = new System.Windows.Forms.Button();
            this.treeViewEditionTable = new System.Windows.Forms.TreeView();
            this.buttonLoadRefTable = new System.Windows.Forms.Button();
            this.buttonClearRefTableFilter = new System.Windows.Forms.Button();
            this.buttonCopyTable = new System.Windows.Forms.Button();
            this.comboBoxRefTableOutputFilter = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonPulseTable = new System.Windows.Forms.Button();
            this.buttonActivationTable = new System.Windows.Forms.Button();
            this.treeViewReferenceTable = new System.Windows.Forms.TreeView();
            this.labelRomName = new System.Windows.Forms.Label();
            this.RomNameComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.propertyGridMain = new System.Windows.Forms.PropertyGrid();
            this.imageListIcons = new System.Windows.Forms.ImageList(this.components);
            this.menuStripMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DofEditionToolStripMenuItem,
            this.DofConfigToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(1190, 24);
            this.menuStripMain.TabIndex = 0;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // DofEditionToolStripMenuItem
            // 
            this.DofEditionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newTableToolStripMenuItem,
            this.loadTableToolStripMenuItem,
            this.saveTableToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.DofEditionToolStripMenuItem.Name = "DofEditionToolStripMenuItem";
            this.DofEditionToolStripMenuItem.Size = new System.Drawing.Size(86, 20);
            this.DofEditionToolStripMenuItem.Text = "Table Edition";
            // 
            // newTableToolStripMenuItem
            // 
            this.newTableToolStripMenuItem.Name = "newTableToolStripMenuItem";
            this.newTableToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.newTableToolStripMenuItem.Text = "New Table";
            this.newTableToolStripMenuItem.Click += new System.EventHandler(this.newTableToolStripMenuItem_Click);
            // 
            // loadTableToolStripMenuItem
            // 
            this.loadTableToolStripMenuItem.Name = "loadTableToolStripMenuItem";
            this.loadTableToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.loadTableToolStripMenuItem.Text = "Load Table";
            this.loadTableToolStripMenuItem.Click += new System.EventHandler(this.loadTableToolStripMenuItem_Click);
            // 
            // saveTableToolStripMenuItem
            // 
            this.saveTableToolStripMenuItem.Name = "saveTableToolStripMenuItem";
            this.saveTableToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveTableToolStripMenuItem.Text = "Save Table";
            this.saveTableToolStripMenuItem.Click += new System.EventHandler(this.saveTableToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // DofConfigToolStripMenuItem
            // 
            this.DofConfigToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importFromDofConfigToolToolStripMenuItem,
            this.exportToDofConfigToolToolStripMenuItem});
            this.DofConfigToolStripMenuItem.Name = "DofConfigToolStripMenuItem";
            this.DofConfigToolStripMenuItem.Size = new System.Drawing.Size(94, 20);
            this.DofConfigToolStripMenuItem.Text = "Import/Export";
            // 
            // importFromDofConfigToolToolStripMenuItem
            // 
            this.importFromDofConfigToolToolStripMenuItem.Name = "importFromDofConfigToolToolStripMenuItem";
            this.importFromDofConfigToolToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.importFromDofConfigToolToolStripMenuItem.Text = "Import from DofConfigTool";
            this.importFromDofConfigToolToolStripMenuItem.Click += new System.EventHandler(this.importFromDofConfigToolToolStripMenuItem_Click);
            // 
            // exportToDofConfigToolToolStripMenuItem
            // 
            this.exportToDofConfigToolToolStripMenuItem.Name = "exportToDofConfigToolToolStripMenuItem";
            this.exportToDofConfigToolToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.exportToDofConfigToolToolStripMenuItem.Text = "Export to DofConfigTool";
            this.exportToDofConfigToolToolStripMenuItem.Click += new System.EventHandler(this.exportToDofConfigToolToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.propertyGridMain);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(3);
            this.splitContainer1.Size = new System.Drawing.Size(1190, 877);
            this.splitContainer1.SplitterDistance = 898;
            this.splitContainer1.TabIndex = 1;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.buttonEditionTableClearFilter);
            this.splitContainer2.Panel1.Controls.Add(this.comboBoxEditionTableOutputFilter);
            this.splitContainer2.Panel1.Controls.Add(this.label3);
            this.splitContainer2.Panel1.Controls.Add(this.label2);
            this.splitContainer2.Panel1.Controls.Add(this.buttonPulseEdition);
            this.splitContainer2.Panel1.Controls.Add(this.buttonActivationEdition);
            this.splitContainer2.Panel1.Controls.Add(this.treeViewEditionTable);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer2.Panel2.Controls.Add(this.buttonLoadRefTable);
            this.splitContainer2.Panel2.Controls.Add(this.buttonClearRefTableFilter);
            this.splitContainer2.Panel2.Controls.Add(this.buttonCopyTable);
            this.splitContainer2.Panel2.Controls.Add(this.comboBoxRefTableOutputFilter);
            this.splitContainer2.Panel2.Controls.Add(this.label4);
            this.splitContainer2.Panel2.Controls.Add(this.buttonPulseTable);
            this.splitContainer2.Panel2.Controls.Add(this.buttonActivationTable);
            this.splitContainer2.Panel2.Controls.Add(this.treeViewReferenceTable);
            this.splitContainer2.Panel2.Controls.Add(this.labelRomName);
            this.splitContainer2.Panel2.Controls.Add(this.RomNameComboBox);
            this.splitContainer2.Panel2.Controls.Add(this.label1);
            this.splitContainer2.Size = new System.Drawing.Size(892, 871);
            this.splitContainer2.SplitterDistance = 612;
            this.splitContainer2.TabIndex = 0;
            // 
            // buttonEditionTableClearFilter
            // 
            this.buttonEditionTableClearFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEditionTableClearFilter.Location = new System.Drawing.Point(818, 7);
            this.buttonEditionTableClearFilter.Name = "buttonEditionTableClearFilter";
            this.buttonEditionTableClearFilter.Size = new System.Drawing.Size(69, 23);
            this.buttonEditionTableClearFilter.TabIndex = 35;
            this.buttonEditionTableClearFilter.Text = "Clear filter";
            this.buttonEditionTableClearFilter.UseVisualStyleBackColor = true;
            this.buttonEditionTableClearFilter.Click += new System.EventHandler(this.buttonEditionTableClearFilter_Click);
            // 
            // comboBoxEditionTableOutputFilter
            // 
            this.comboBoxEditionTableOutputFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxEditionTableOutputFilter.FormattingEnabled = true;
            this.comboBoxEditionTableOutputFilter.Location = new System.Drawing.Point(671, 7);
            this.comboBoxEditionTableOutputFilter.Name = "comboBoxEditionTableOutputFilter";
            this.comboBoxEditionTableOutputFilter.Size = new System.Drawing.Size(141, 21);
            this.comboBoxEditionTableOutputFilter.TabIndex = 34;
            this.comboBoxEditionTableOutputFilter.SelectedIndexChanged += new System.EventHandler(this.comboBoxEditionTableOutputFilter_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(604, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 27;
            this.label3.Text = "Output filter";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 26;
            this.label2.Text = "Edition table effects";
            // 
            // buttonPulseEdition
            // 
            this.buttonPulseEdition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPulseEdition.Location = new System.Drawing.Point(812, 584);
            this.buttonPulseEdition.Name = "buttonPulseEdition";
            this.buttonPulseEdition.Size = new System.Drawing.Size(75, 23);
            this.buttonPulseEdition.TabIndex = 25;
            this.buttonPulseEdition.Text = "Pulse";
            this.buttonPulseEdition.UseVisualStyleBackColor = true;
            this.buttonPulseEdition.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonPulseEdition_MouseDown);
            this.buttonPulseEdition.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonPulseEdition_MouseUp);
            // 
            // buttonActivationEdition
            // 
            this.buttonActivationEdition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonActivationEdition.Location = new System.Drawing.Point(731, 584);
            this.buttonActivationEdition.Name = "buttonActivationEdition";
            this.buttonActivationEdition.Size = new System.Drawing.Size(75, 23);
            this.buttonActivationEdition.TabIndex = 24;
            this.buttonActivationEdition.Text = "Activate";
            this.buttonActivationEdition.UseVisualStyleBackColor = true;
            this.buttonActivationEdition.Click += new System.EventHandler(this.buttonActivationEdition_Click);
            // 
            // treeViewEditionTable
            // 
            this.treeViewEditionTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewEditionTable.Location = new System.Drawing.Point(17, 38);
            this.treeViewEditionTable.Name = "treeViewEditionTable";
            this.treeViewEditionTable.Size = new System.Drawing.Size(870, 540);
            this.treeViewEditionTable.TabIndex = 23;
            this.treeViewEditionTable.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeViewEditionTable_KeyDown);
            this.treeViewEditionTable.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeViewEditionTable_MouseDown);
            // 
            // buttonLoadRefTable
            // 
            this.buttonLoadRefTable.Location = new System.Drawing.Point(250, 29);
            this.buttonLoadRefTable.Name = "buttonLoadRefTable";
            this.buttonLoadRefTable.Size = new System.Drawing.Size(159, 23);
            this.buttonLoadRefTable.TabIndex = 35;
            this.buttonLoadRefTable.Text = "Load Reference DOTK file";
            this.buttonLoadRefTable.UseVisualStyleBackColor = true;
            this.buttonLoadRefTable.Click += new System.EventHandler(this.buttonLoadRefTable_Click);
            // 
            // buttonClearRefTableFilter
            // 
            this.buttonClearRefTableFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClearRefTableFilter.Location = new System.Drawing.Point(686, 29);
            this.buttonClearRefTableFilter.Name = "buttonClearRefTableFilter";
            this.buttonClearRefTableFilter.Size = new System.Drawing.Size(69, 23);
            this.buttonClearRefTableFilter.TabIndex = 34;
            this.buttonClearRefTableFilter.Text = "Clear filter";
            this.buttonClearRefTableFilter.UseVisualStyleBackColor = true;
            this.buttonClearRefTableFilter.Click += new System.EventHandler(this.buttonClearRefTableFilter_Click);
            // 
            // buttonCopyTable
            // 
            this.buttonCopyTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCopyTable.Location = new System.Drawing.Point(761, 29);
            this.buttonCopyTable.Name = "buttonCopyTable";
            this.buttonCopyTable.Size = new System.Drawing.Size(126, 23);
            this.buttonCopyTable.TabIndex = 33;
            this.buttonCopyTable.Text = "Copy table to edition";
            this.buttonCopyTable.UseVisualStyleBackColor = true;
            this.buttonCopyTable.Click += new System.EventHandler(this.buttonCopyTable_Click);
            // 
            // comboBoxRefTableOutputFilter
            // 
            this.comboBoxRefTableOutputFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxRefTableOutputFilter.FormattingEnabled = true;
            this.comboBoxRefTableOutputFilter.Location = new System.Drawing.Point(520, 31);
            this.comboBoxRefTableOutputFilter.Name = "comboBoxRefTableOutputFilter";
            this.comboBoxRefTableOutputFilter.Size = new System.Drawing.Size(160, 21);
            this.comboBoxRefTableOutputFilter.TabIndex = 32;
            this.comboBoxRefTableOutputFilter.SelectedIndexChanged += new System.EventHandler(this.comboBoxRefTableOutputFilter_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(455, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 31;
            this.label4.Text = "Output filter";
            // 
            // buttonPulseTable
            // 
            this.buttonPulseTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPulseTable.Location = new System.Drawing.Point(812, 227);
            this.buttonPulseTable.Name = "buttonPulseTable";
            this.buttonPulseTable.Size = new System.Drawing.Size(75, 23);
            this.buttonPulseTable.TabIndex = 29;
            this.buttonPulseTable.Text = "Pulse";
            this.buttonPulseTable.UseVisualStyleBackColor = true;
            this.buttonPulseTable.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonPulseTable_MouseDown);
            this.buttonPulseTable.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonPulseTable_MouseUp);
            // 
            // buttonActivationTable
            // 
            this.buttonActivationTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonActivationTable.Location = new System.Drawing.Point(731, 227);
            this.buttonActivationTable.Name = "buttonActivationTable";
            this.buttonActivationTable.Size = new System.Drawing.Size(75, 23);
            this.buttonActivationTable.TabIndex = 28;
            this.buttonActivationTable.Text = "Activate";
            this.buttonActivationTable.UseVisualStyleBackColor = true;
            this.buttonActivationTable.Click += new System.EventHandler(this.buttonActivationTable_Click);
            // 
            // treeViewReferenceTable
            // 
            this.treeViewReferenceTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewReferenceTable.Location = new System.Drawing.Point(17, 58);
            this.treeViewReferenceTable.Name = "treeViewReferenceTable";
            this.treeViewReferenceTable.Size = new System.Drawing.Size(870, 163);
            this.treeViewReferenceTable.TabIndex = 27;
            this.treeViewReferenceTable.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeViewReferenceTable_MouseDown);
            // 
            // labelRomName
            // 
            this.labelRomName.AutoSize = true;
            this.labelRomName.Location = new System.Drawing.Point(13, 34);
            this.labelRomName.Name = "labelRomName";
            this.labelRomName.Size = new System.Drawing.Size(57, 13);
            this.labelRomName.TabIndex = 26;
            this.labelRomName.Text = "RomName";
            // 
            // RomNameComboBox
            // 
            this.RomNameComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.RomNameComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.RomNameComboBox.FormattingEnabled = true;
            this.RomNameComboBox.Location = new System.Drawing.Point(76, 31);
            this.RomNameComboBox.Name = "RomNameComboBox";
            this.RomNameComboBox.Size = new System.Drawing.Size(168, 21);
            this.RomNameComboBox.TabIndex = 25;
            this.RomNameComboBox.SelectedIndexChanged += new System.EventHandler(this.RomNameComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "Reference table effects";
            // 
            // propertyGridMain
            // 
            this.propertyGridMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridMain.Location = new System.Drawing.Point(3, 3);
            this.propertyGridMain.Name = "propertyGridMain";
            this.propertyGridMain.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.propertyGridMain.Size = new System.Drawing.Size(282, 871);
            this.propertyGridMain.TabIndex = 0;
            this.propertyGridMain.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGridMain_PropertyValueChanged);
            // 
            // imageListIcons
            // 
            this.imageListIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListIcons.ImageStream")));
            this.imageListIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListIcons.Images.SetKeyName(0, "red_cross.png");
            this.imageListIcons.Images.SetKeyName(1, "green_gears.png");
            // 
            // DirectOutputToolkitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1190, 901);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStripMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStripMain;
            this.Name = "DirectOutputToolkitForm";
            this.Text = "DirectOutput Toolkit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DirectOutputToolkitForm_FormClosing);
            this.Load += new System.EventHandler(this.DirectOutputToolkitForm_Load);
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem DofEditionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newTableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadTableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveTableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DofConfigToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importFromDofConfigToolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToDofConfigToolToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PropertyGrid propertyGridMain;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelRomName;
        private System.Windows.Forms.ComboBox RomNameComboBox;
        private System.Windows.Forms.Button buttonPulseTable;
        private System.Windows.Forms.Button buttonActivationTable;
        private System.Windows.Forms.TreeView treeViewReferenceTable;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonPulseEdition;
        private System.Windows.Forms.Button buttonActivationEdition;
        private System.Windows.Forms.TreeView treeViewEditionTable;
        private System.Windows.Forms.ImageList imageListIcons;
        private System.Windows.Forms.ComboBox comboBoxRefTableOutputFilter;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonCopyTable;
        private System.Windows.Forms.ComboBox comboBoxEditionTableOutputFilter;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonClearRefTableFilter;
        private System.Windows.Forms.Button buttonEditionTableClearFilter;
        private System.Windows.Forms.Button buttonLoadRefTable;
    }
}

