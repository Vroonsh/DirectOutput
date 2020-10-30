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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LedMatrixToolkit));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageEffectTemplates = new System.Windows.Forms.TabPage();
            this.tabPageEffectEditor = new System.Windows.Forms.TabPage();
            this.tabPageTableEffects = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.RomNameComboBox = new System.Windows.Forms.ComboBox();
            this.TableElements = new System.Windows.Forms.DataGridView();
            this.TEType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TEName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TENumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TEValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TEActivate = new System.Windows.Forms.DataGridViewButtonColumn();
            this.TEPulse = new System.Windows.Forms.DataGridViewButtonColumn();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.propertyGridEffect = new System.Windows.Forms.PropertyGrid();
            this.panelPreviewLedMatrix = new LedMatrixPreviewControl();
            this.tabControl1.SuspendLayout();
            this.tabPageTableEffects.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TableElements)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
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
            this.tabControl1.Size = new System.Drawing.Size(1130, 994);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageEffectTemplates
            // 
            this.tabPageEffectTemplates.Location = new System.Drawing.Point(4, 22);
            this.tabPageEffectTemplates.Name = "tabPageEffectTemplates";
            this.tabPageEffectTemplates.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEffectTemplates.Size = new System.Drawing.Size(1122, 968);
            this.tabPageEffectTemplates.TabIndex = 0;
            this.tabPageEffectTemplates.Text = "Effect Templates";
            this.tabPageEffectTemplates.UseVisualStyleBackColor = true;
            // 
            // tabPageEffectEditor
            // 
            this.tabPageEffectEditor.Location = new System.Drawing.Point(4, 22);
            this.tabPageEffectEditor.Name = "tabPageEffectEditor";
            this.tabPageEffectEditor.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEffectEditor.Size = new System.Drawing.Size(1122, 968);
            this.tabPageEffectEditor.TabIndex = 1;
            this.tabPageEffectEditor.Text = "Effect Editor";
            this.tabPageEffectEditor.UseVisualStyleBackColor = true;
            // 
            // tabPageTableEffects
            // 
            this.tabPageTableEffects.Controls.Add(this.label2);
            this.tabPageTableEffects.Controls.Add(this.RomNameComboBox);
            this.tabPageTableEffects.Controls.Add(this.TableElements);
            this.tabPageTableEffects.Location = new System.Drawing.Point(4, 22);
            this.tabPageTableEffects.Name = "tabPageTableEffects";
            this.tabPageTableEffects.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTableEffects.Size = new System.Drawing.Size(1122, 968);
            this.tabPageTableEffects.TabIndex = 2;
            this.tabPageTableEffects.Text = "Table Effects";
            this.tabPageTableEffects.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "RomName:";
            // 
            // RomNameComboBox
            // 
            this.RomNameComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RomNameComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.RomNameComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.RomNameComboBox.FormattingEnabled = true;
            this.RomNameComboBox.Items.AddRange(new object[] {
            "",
            "abv106",
            "acd_170hc",
            "afm",
            "ali",
            "alienstr",
            "alpok",
            "amazonh",
            "apollo13",
            "arena",
            "atlantis",
            "austin",
            "babypac",
            "badgirls",
            "barbwire",
            "barra",
            "batmanf",
            "baywatch",
            "bbb109",
            "bcats",
            "beatclck",
            "bguns",
            "biggame",
            "bighouse",
            "bighurt",
            "bk",
            "bk2k",
            "blackblt",
            "blackjck",
            "blakpyra",
            "blckhole",
            "blkou",
            "bnzai",
            "bonebstr",
            "bop",
            "bowarrow",
            "br",
            "brvteam",
            "bsv103",
            "btmn",
            "bttf",
            "bullseye",
            "canasta",
            "cc",
            "centaur",
            "cftbl",
            "charlies",
            "ckpt",
            "clas1812",
            "closeenc",
            "comet",
            "congo",
            "corv",
            "csmic",
            "cueball",
            "cv",
            "cybrnaut",
            "cycln",
            "cyclopes",
            "dd",
            "deadweap",
            "dh",
            "diamond",
            "diner",
            "disco",
            "dm",
            "dollyptb",
            "drac",
            "dracula",
            "dragon",
            "dvlrider",
            "dw",
            "eatpm",
            "eballchp",
            "eballdlx",
            "eightbll",
            "eldorado",
            "elvis",
            "embryon",
            "esha",
            "f14",
            "faeton",
            "fathom",
            "fbclass",
            "ffv104",
            "fh",
            "flash",
            "flashgdn",
            "flight2k",
            "frankst",
            "freddy",
            "freefall",
            "frontier",
            "frpwr",
            "fs",
            "ft",
            "futurspa",
            "galaxy",
            "genesis",
            "genie",
            "gi",
            "gldneye",
            "gnr",
            "godzilla",
            "goldcue",
            "gprix",
            "grand",
            "grgar",
            "gs",
            "gw",
            "hd",
            "hglbtrtb",
            "hh",
            "hirolcas",
            "hook",
            "hothand",
            "hpgof",
            "hs",
            "hurr",
            "i500",
            "icefever",
            "id4",
            "ij",
            "ind250cc",
            "ironmaid",
            "jamesb2",
            "jb",
            "jd",
            "jm",
            "jokrz",
            "jolypark",
            "jplstw22",
            "jupk",
            "jy",
            "kissb",
            "kosteel",
            "kpv",
            "lah",
            "lectrono",
            "lightnin",
            "lostspc",
            "lostwrld",
            "lotr",
            "lsrcu",
            "lw3",
            "m",
            "matahari",
            "mav",
            "mb",
            "medusa",
            "mephisto",
            "metalman",
            "mm",
            "monopoly",
            "mousn",
            "mysticb",
            "nascar",
            "nbaf",
            "nf",
            "ngg",
            "ngndshkr",
            "nineball",
            "panthera",
            "paragon",
            "pb",
            "pharo",
            "pinchamp",
            "pinpool",
            "play",
            "playboyb",
            "playboys",
            "pmv112",
            "pnkpnthr",
            "polic",
            "pop",
            "poto",
            "princess",
            "prtyanim",
            "pwerplay",
            "pz",
            "qbquest",
            "rab",
            "radcl",
            "raven",
            "rctycn",
            "rdkng",
            "rescu911",
            "rflshdlx",
            "ripleys",
            "roadrunr",
            "robo",
            "robot",
            "robowars",
            "rocky",
            "rollr",
            "rollstob",
            "rs",
            "rvrbt",
            "seawitch",
            "sfight2",
            "simp",
            "simpprty",
            "slbmania",
            "smb3",
            "solaride",
            "sopranos",
            "sorcr",
            "spaceinv",
            "spcrider",
            "spectru4",
            "spidermn7",
            "spirit",
            "sprk",
            "SS",
            "sshooter",
            "sshtl",
            "sst",
            "ssvc",
            "stargat4",
            "stargoda",
            "stars",
            "startrek",
            "startrp",
            "stk",
            "strngsci",
            "sttng",
            "stwr",
            "swrds",
            "swtril43",
            "t2",
            "taf",
            "taxi",
            "teedoff3",
            "term3",
            "tftc",
            "tmac",
            "tmnt",
            "tom",
            "tomy",
            "totan",
            "trailer",
            "trek",
            "trident",
            "trucksp3",
            "ts",
            "tstrk",
            "twst",
            "tz",
            "vegas",
            "viprsega",
            "vortex",
            "wcs",
            "wd",
            "whirl",
            "wipeout",
            "wrldtou2",
            "ww",
            "wwfr",
            "Xenon",
            "xfiles"});
            this.RomNameComboBox.Location = new System.Drawing.Point(137, 3);
            this.RomNameComboBox.Name = "RomNameComboBox";
            this.RomNameComboBox.Size = new System.Drawing.Size(524, 21);
            this.RomNameComboBox.TabIndex = 11;
            this.RomNameComboBox.SelectedIndexChanged += new System.EventHandler(this.RomNameComboBox_SelectedIndexChanged);
            // 
            // TableElements
            // 
            this.TableElements.AllowUserToAddRows = false;
            this.TableElements.AllowUserToDeleteRows = false;
            this.TableElements.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TableElements.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TableElements.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TEType,
            this.TEName,
            this.TENumber,
            this.TEValue,
            this.TEActivate,
            this.TEPulse});
            this.TableElements.Location = new System.Drawing.Point(6, 35);
            this.TableElements.Name = "TableElements";
            this.TableElements.RowHeadersVisible = false;
            this.TableElements.Size = new System.Drawing.Size(655, 433);
            this.TableElements.TabIndex = 1;
            this.TableElements.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.TableElements_CellClick);
            this.TableElements.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.TableElements_CellValueChanged);
            // 
            // TEType
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.TEType.DefaultCellStyle = dataGridViewCellStyle1;
            this.TEType.HeaderText = "Type";
            this.TEType.Name = "TEType";
            this.TEType.ReadOnly = true;
            // 
            // TEName
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.TEName.DefaultCellStyle = dataGridViewCellStyle2;
            this.TEName.HeaderText = "Name";
            this.TEName.Name = "TEName";
            this.TEName.ReadOnly = true;
            // 
            // TENumber
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.TENumber.DefaultCellStyle = dataGridViewCellStyle3;
            this.TENumber.HeaderText = "Number";
            this.TENumber.Name = "TENumber";
            this.TENumber.ReadOnly = true;
            // 
            // TEValue
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.TEValue.DefaultCellStyle = dataGridViewCellStyle4;
            this.TEValue.HeaderText = "Value";
            this.TEValue.Name = "TEValue";
            // 
            // TEActivate
            // 
            this.TEActivate.HeaderText = "Activate";
            this.TEActivate.Name = "TEActivate";
            this.TEActivate.ReadOnly = true;
            // 
            // TEPulse
            // 
            this.TEPulse.HeaderText = "Pulse";
            this.TEPulse.Name = "TEPulse";
            this.TEPulse.ReadOnly = true;
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
            this.splitContainer1.Panel2.Controls.Add(this.panelPreviewLedMatrix);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(3);
            this.splitContainer1.Size = new System.Drawing.Size(1698, 1000);
            this.splitContainer1.SplitterDistance = 1136;
            this.splitContainer1.TabIndex = 2;
            // 
            // propertyGridEffect
            // 
            this.propertyGridEffect.Dock = System.Windows.Forms.DockStyle.Right;
            this.propertyGridEffect.Location = new System.Drawing.Point(674, 3);
            this.propertyGridEffect.Name = "propertyGridEffect";
            this.propertyGridEffect.Size = new System.Drawing.Size(459, 994);
            this.propertyGridEffect.TabIndex = 1;
            // 
            // panelPreviewLedMatrix
            // 
            this.panelPreviewLedMatrix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPreviewLedMatrix.Location = new System.Drawing.Point(3, 3);
            this.panelPreviewLedMatrix.Name = "panelPreviewLedMatrix";
            this.panelPreviewLedMatrix.Size = new System.Drawing.Size(552, 994);
            this.panelPreviewLedMatrix.TabIndex = 0;
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
            this.Load += new System.EventHandler(this.LedMatrixToolkit_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageTableEffects.ResumeLayout(false);
            this.tabPageTableEffects.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TableElements)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private SplitContainer splitContainer1;
        private TabControl tabControl1;
        private TabPage tabPageEffectTemplates;
        private TabPage tabPageEffectEditor;
        private TabPage tabPageTableEffects;
        private PropertyGrid propertyGridEffect;
        private DataGridView TableElements;
        private DataGridViewTextBoxColumn TEType;
        private DataGridViewTextBoxColumn TEName;
        private DataGridViewTextBoxColumn TENumber;
        private DataGridViewTextBoxColumn TEValue;
        private DataGridViewButtonColumn TEActivate;
        private DataGridViewButtonColumn TEPulse;
        private Label label2;
        private ComboBox RomNameComboBox;
        private LedMatrixPreviewControl panelPreviewLedMatrix;
    }
}

