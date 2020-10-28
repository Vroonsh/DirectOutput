using System.Windows.Forms;

namespace LedMatrixToolkit
{
    partial class LedMatrixToolkit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LedMatrixToolkit));
            this.panelPreview = new LedMatrixPreviewControl();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageEffectTemplates = new System.Windows.Forms.TabPage();
            this.tabPageEffectEditor = new System.Windows.Forms.TabPage();
            this.tabPageTableEffects = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.propertyGridEffect = new System.Windows.Forms.PropertyGrid();
            this.tabControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelPreview
            // 
            this.panelPreview.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panelPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPreview.Location = new System.Drawing.Point(3, 3);
            this.panelPreview.Margin = new System.Windows.Forms.Padding(0);
            this.panelPreview.Name = "panelPreview";
            this.panelPreview.Size = new System.Drawing.Size(820, 994);
            this.panelPreview.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageEffectTemplates);
            this.tabControl1.Controls.Add(this.tabPageEffectEditor);
            this.tabControl1.Controls.Add(this.tabPageTableEffects);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(862, 994);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageEffectTemplates
            // 
            this.tabPageEffectTemplates.Location = new System.Drawing.Point(4, 22);
            this.tabPageEffectTemplates.Name = "tabPageEffectTemplates";
            this.tabPageEffectTemplates.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEffectTemplates.Size = new System.Drawing.Size(854, 968);
            this.tabPageEffectTemplates.TabIndex = 0;
            this.tabPageEffectTemplates.Text = "Effect Templates";
            this.tabPageEffectTemplates.UseVisualStyleBackColor = true;
            // 
            // tabPageEffectEditor
            // 
            this.tabPageEffectEditor.Location = new System.Drawing.Point(4, 22);
            this.tabPageEffectEditor.Name = "tabPageEffectEditor";
            this.tabPageEffectEditor.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEffectEditor.Size = new System.Drawing.Size(796, 823);
            this.tabPageEffectEditor.TabIndex = 1;
            this.tabPageEffectEditor.Text = "Effect Editor";
            this.tabPageEffectEditor.UseVisualStyleBackColor = true;
            // 
            // tabPageTableEffects
            // 
            this.tabPageTableEffects.Location = new System.Drawing.Point(4, 22);
            this.tabPageTableEffects.Name = "tabPageTableEffects";
            this.tabPageTableEffects.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTableEffects.Size = new System.Drawing.Size(796, 823);
            this.tabPageTableEffects.TabIndex = 2;
            this.tabPageTableEffects.Text = "Table Effects";
            this.tabPageTableEffects.UseVisualStyleBackColor = true;
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
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panelPreview);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(3);
            this.splitContainer1.Size = new System.Drawing.Size(1698, 1000);
            this.splitContainer1.SplitterDistance = 868;
            this.splitContainer1.TabIndex = 2;
            // 
            // propertyGridEffect
            // 
            this.propertyGridEffect.Dock = System.Windows.Forms.DockStyle.Right;
            this.propertyGridEffect.Location = new System.Drawing.Point(467, 3);
            this.propertyGridEffect.Name = "propertyGridEffect";
            this.propertyGridEffect.Size = new System.Drawing.Size(398, 994);
            this.propertyGridEffect.TabIndex = 1;
            // 
            // LedMatrixToolkit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1698, 1000);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LedMatrixToolkit";
            this.Text = "Led Matrix Toolkit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LedMatrixToolkit_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private LedMatrixPreviewControl panelPreview;
        private SplitContainer splitContainer1;
        private TabControl tabControl1;
        private TabPage tabPageEffectTemplates;
        private TabPage tabPageEffectEditor;
        private TabPage tabPageTableEffects;
        private PropertyGrid propertyGridEffect;
    }
}

