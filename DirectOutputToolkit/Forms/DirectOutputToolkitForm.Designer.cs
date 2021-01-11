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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DirectOutputToolkitForm));
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.DofEditionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DofConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importFromDofConfigToolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToDofConfigToolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.propertyGridMain = new System.Windows.Forms.PropertyGrid();
            this.menuStripMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DofEditionToolStripMenuItem,
            this.DofConfigToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(1291, 24);
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
            // 
            // loadTableToolStripMenuItem
            // 
            this.loadTableToolStripMenuItem.Name = "loadTableToolStripMenuItem";
            this.loadTableToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.loadTableToolStripMenuItem.Text = "Load Table";
            // 
            // saveTableToolStripMenuItem
            // 
            this.saveTableToolStripMenuItem.Name = "saveTableToolStripMenuItem";
            this.saveTableToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveTableToolStripMenuItem.Text = "Save Table";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
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
            // 
            // exportToDofConfigToolToolStripMenuItem
            // 
            this.exportToDofConfigToolToolStripMenuItem.Name = "exportToDofConfigToolToolStripMenuItem";
            this.exportToDofConfigToolToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.exportToDofConfigToolToolStripMenuItem.Text = "Export to DofConfigTool";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.propertyGridMain);
            this.splitContainer1.Size = new System.Drawing.Size(1291, 889);
            this.splitContainer1.SplitterDistance = 974;
            this.splitContainer1.TabIndex = 1;
            // 
            // propertyGridMain
            // 
            this.propertyGridMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridMain.Location = new System.Drawing.Point(0, 0);
            this.propertyGridMain.Name = "propertyGridMain";
            this.propertyGridMain.Size = new System.Drawing.Size(313, 889);
            this.propertyGridMain.TabIndex = 0;
            // 
            // DirectOutputToolkitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1291, 913);
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
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DofConfigToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importFromDofConfigToolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToDofConfigToolToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PropertyGrid propertyGridMain;
    }
}

