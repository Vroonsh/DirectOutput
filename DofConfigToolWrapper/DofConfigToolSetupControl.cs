using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DofConfigToolWrapper
{
    public class DofConfigToolSetupControl : UserControl
    {
        private Label labelUserName;
        private TextBox textBoxUserName;
        private Label labelAPIKey;
        private Label labelControllers;
        private System.ComponentModel.IContainer components;
        private Button buttonAddController;
        private DataGridView dataGridViewControllers;
        private Label label1;
        private DataGridView dataGridViewOutputMappings;
        private Button buttonAddOutputMapping;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewComboBoxColumn dataGridViewComboBoxColumn1;
        private DataGridViewComboBoxColumn dataGridViewComboBoxColumn2;
        private Button buttonSaveSetup;
        private Button buttonLoadSetup;
        private DataGridViewComboBoxColumn dataGridViewComboBoxColumn3;
        private DataGridViewComboBoxColumn dataGridViewComboBoxColumn4;
        private DataGridViewComboBoxColumn dataGridViewComboBoxColumn5;
        private DataGridViewComboBoxColumn dataGridViewComboBoxColumn6;
        private BindingSource controllerSetupBindingSource;
        private BindingSource outputMappingsBindingSource;
        private DataGridViewTextBoxColumn numberDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn portNumberDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn portRangeDataGridViewTextBoxColumn;
        private DataGridViewComboBoxColumn outputDataGridViewTextBoxColumn;
        private TextBox textBoxAPIKey;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.labelUserName = new System.Windows.Forms.Label();
            this.textBoxUserName = new System.Windows.Forms.TextBox();
            this.labelAPIKey = new System.Windows.Forms.Label();
            this.textBoxAPIKey = new System.Windows.Forms.TextBox();
            this.labelControllers = new System.Windows.Forms.Label();
            this.buttonAddController = new System.Windows.Forms.Button();
            this.dataGridViewControllers = new System.Windows.Forms.DataGridView();
            this.numberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.controllerSetupBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridViewOutputMappings = new System.Windows.Forms.DataGridView();
            this.nameDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.portNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.portRangeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.outputDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.outputMappingsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.buttonAddOutputMapping = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewComboBoxColumn1 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewComboBoxColumn2 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.buttonSaveSetup = new System.Windows.Forms.Button();
            this.buttonLoadSetup = new System.Windows.Forms.Button();
            this.dataGridViewComboBoxColumn3 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewComboBoxColumn4 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewComboBoxColumn5 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewComboBoxColumn6 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewControllers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.controllerSetupBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOutputMappings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.outputMappingsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // labelUserName
            // 
            this.labelUserName.AutoSize = true;
            this.labelUserName.Location = new System.Drawing.Point(3, 8);
            this.labelUserName.Name = "labelUserName";
            this.labelUserName.Size = new System.Drawing.Size(60, 13);
            this.labelUserName.TabIndex = 0;
            this.labelUserName.Text = "User Name";
            // 
            // textBoxUserName
            // 
            this.textBoxUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxUserName.Location = new System.Drawing.Point(69, 5);
            this.textBoxUserName.Name = "textBoxUserName";
            this.textBoxUserName.Size = new System.Drawing.Size(890, 20);
            this.textBoxUserName.TabIndex = 1;
            this.textBoxUserName.TextChanged += new System.EventHandler(this.TextBoxUserName_TextChanged);
            // 
            // labelAPIKey
            // 
            this.labelAPIKey.AutoSize = true;
            this.labelAPIKey.Location = new System.Drawing.Point(6, 37);
            this.labelAPIKey.Name = "labelAPIKey";
            this.labelAPIKey.Size = new System.Drawing.Size(45, 13);
            this.labelAPIKey.TabIndex = 2;
            this.labelAPIKey.Text = "API Key";
            // 
            // textBoxAPIKey
            // 
            this.textBoxAPIKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxAPIKey.Location = new System.Drawing.Point(69, 34);
            this.textBoxAPIKey.Name = "textBoxAPIKey";
            this.textBoxAPIKey.Size = new System.Drawing.Size(890, 20);
            this.textBoxAPIKey.TabIndex = 3;
            this.textBoxAPIKey.TextChanged += new System.EventHandler(this.TextBoxAPIKey_TextChanged);
            // 
            // labelControllers
            // 
            this.labelControllers.AutoSize = true;
            this.labelControllers.Location = new System.Drawing.Point(9, 72);
            this.labelControllers.Name = "labelControllers";
            this.labelControllers.Size = new System.Drawing.Size(56, 13);
            this.labelControllers.TabIndex = 5;
            this.labelControllers.Text = "Controllers";
            // 
            // buttonAddController
            // 
            this.buttonAddController.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddController.Location = new System.Drawing.Point(927, 72);
            this.buttonAddController.Name = "buttonAddController";
            this.buttonAddController.Size = new System.Drawing.Size(29, 23);
            this.buttonAddController.TabIndex = 7;
            this.buttonAddController.Text = "+";
            this.buttonAddController.UseVisualStyleBackColor = true;
            this.buttonAddController.Click += new System.EventHandler(this.ButtonAddController_Click);
            // 
            // dataGridViewControllers
            // 
            this.dataGridViewControllers.AllowUserToAddRows = false;
            this.dataGridViewControllers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewControllers.AutoGenerateColumns = false;
            this.dataGridViewControllers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewControllers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.numberDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn});
            this.dataGridViewControllers.DataSource = this.controllerSetupBindingSource;
            this.dataGridViewControllers.Location = new System.Drawing.Point(12, 103);
            this.dataGridViewControllers.MultiSelect = false;
            this.dataGridViewControllers.Name = "dataGridViewControllers";
            this.dataGridViewControllers.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridViewControllers.Size = new System.Drawing.Size(944, 137);
            this.dataGridViewControllers.TabIndex = 8;
            this.dataGridViewControllers.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGridViewControllers_CellMouseClick);
            this.dataGridViewControllers.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DataGridViewControllers_KeyDown);
            // 
            // numberDataGridViewTextBoxColumn
            // 
            this.numberDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.numberDataGridViewTextBoxColumn.DataPropertyName = "Number";
            this.numberDataGridViewTextBoxColumn.HeaderText = "Number";
            this.numberDataGridViewTextBoxColumn.Name = "numberDataGridViewTextBoxColumn";
            this.numberDataGridViewTextBoxColumn.Width = 69;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            // 
            // controllerSetupBindingSource
            // 
            this.controllerSetupBindingSource.DataSource = typeof(DofConfigToolWrapper.DofConfigToolSetup.ControllerSetup);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 247);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Output Mappings";
            // 
            // dataGridViewOutputMappings
            // 
            this.dataGridViewOutputMappings.AllowUserToAddRows = false;
            this.dataGridViewOutputMappings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewOutputMappings.AutoGenerateColumns = false;
            this.dataGridViewOutputMappings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewOutputMappings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn1,
            this.portNumberDataGridViewTextBoxColumn,
            this.portRangeDataGridViewTextBoxColumn,
            this.outputDataGridViewTextBoxColumn});
            this.dataGridViewOutputMappings.DataSource = this.outputMappingsBindingSource;
            this.dataGridViewOutputMappings.Location = new System.Drawing.Point(15, 275);
            this.dataGridViewOutputMappings.MultiSelect = false;
            this.dataGridViewOutputMappings.Name = "dataGridViewOutputMappings";
            this.dataGridViewOutputMappings.Size = new System.Drawing.Size(941, 376);
            this.dataGridViewOutputMappings.TabIndex = 10;
            this.dataGridViewOutputMappings.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGridViewOutputMappings_CellMouseClick);
            this.dataGridViewOutputMappings.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewOutputMappings_CellValueChanged);
            this.dataGridViewOutputMappings.CurrentCellDirtyStateChanged += new System.EventHandler(this.DataGridViewOutputMappings_CurrentCellDirtyStateChanged);
            this.dataGridViewOutputMappings.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DataGridViewOutputMappings_KeyDown);
            // 
            // nameDataGridViewTextBoxColumn1
            // 
            this.nameDataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.nameDataGridViewTextBoxColumn1.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn1.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn1.Name = "nameDataGridViewTextBoxColumn1";
            this.nameDataGridViewTextBoxColumn1.Width = 60;
            // 
            // portNumberDataGridViewTextBoxColumn
            // 
            this.portNumberDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.portNumberDataGridViewTextBoxColumn.DataPropertyName = "PortNumber";
            this.portNumberDataGridViewTextBoxColumn.HeaderText = "PortNumber";
            this.portNumberDataGridViewTextBoxColumn.Name = "portNumberDataGridViewTextBoxColumn";
            this.portNumberDataGridViewTextBoxColumn.Width = 88;
            // 
            // portRangeDataGridViewTextBoxColumn
            // 
            this.portRangeDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.portRangeDataGridViewTextBoxColumn.DataPropertyName = "PortRange";
            this.portRangeDataGridViewTextBoxColumn.HeaderText = "PortRange";
            this.portRangeDataGridViewTextBoxColumn.Name = "portRangeDataGridViewTextBoxColumn";
            this.portRangeDataGridViewTextBoxColumn.ReadOnly = true;
            this.portRangeDataGridViewTextBoxColumn.Width = 83;
            // 
            // outputDataGridViewTextBoxColumn
            // 
            this.outputDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.outputDataGridViewTextBoxColumn.DataPropertyName = "Output";
            this.outputDataGridViewTextBoxColumn.HeaderText = "Output";
            this.outputDataGridViewTextBoxColumn.Name = "outputDataGridViewTextBoxColumn";
            this.outputDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.outputDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // outputMappingsBindingSource
            // 
            this.outputMappingsBindingSource.DataMember = "OutputMappings";
            this.outputMappingsBindingSource.DataSource = this.controllerSetupBindingSource;
            // 
            // buttonAddOutputMapping
            // 
            this.buttonAddOutputMapping.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddOutputMapping.Location = new System.Drawing.Point(927, 246);
            this.buttonAddOutputMapping.Name = "buttonAddOutputMapping";
            this.buttonAddOutputMapping.Size = new System.Drawing.Size(29, 23);
            this.buttonAddOutputMapping.TabIndex = 11;
            this.buttonAddOutputMapping.Text = "+";
            this.buttonAddOutputMapping.UseVisualStyleBackColor = true;
            this.buttonAddOutputMapping.Click += new System.EventHandler(this.ButtonAddOutputMapping_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Output";
            this.dataGridViewTextBoxColumn1.HeaderText = "Output";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Output";
            this.dataGridViewTextBoxColumn2.HeaderText = "Output";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Output";
            this.dataGridViewTextBoxColumn3.HeaderText = "Output";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewComboBoxColumn1
            // 
            this.dataGridViewComboBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewComboBoxColumn1.DataPropertyName = "Output";
            this.dataGridViewComboBoxColumn1.HeaderText = "Output";
            this.dataGridViewComboBoxColumn1.Name = "dataGridViewComboBoxColumn1";
            this.dataGridViewComboBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewComboBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dataGridViewComboBoxColumn2
            // 
            this.dataGridViewComboBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewComboBoxColumn2.DataPropertyName = "Output";
            this.dataGridViewComboBoxColumn2.HeaderText = "Output";
            this.dataGridViewComboBoxColumn2.Name = "dataGridViewComboBoxColumn2";
            this.dataGridViewComboBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewComboBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // buttonSaveSetup
            // 
            this.buttonSaveSetup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveSetup.Location = new System.Drawing.Point(881, 657);
            this.buttonSaveSetup.Name = "buttonSaveSetup";
            this.buttonSaveSetup.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveSetup.TabIndex = 12;
            this.buttonSaveSetup.Text = "Save Setup";
            this.buttonSaveSetup.UseVisualStyleBackColor = true;
            this.buttonSaveSetup.Click += new System.EventHandler(this.ButtonSaveSetup_Click);
            // 
            // buttonLoadSetup
            // 
            this.buttonLoadSetup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLoadSetup.Location = new System.Drawing.Point(800, 657);
            this.buttonLoadSetup.Name = "buttonLoadSetup";
            this.buttonLoadSetup.Size = new System.Drawing.Size(75, 23);
            this.buttonLoadSetup.TabIndex = 13;
            this.buttonLoadSetup.Text = "Load Setup";
            this.buttonLoadSetup.UseVisualStyleBackColor = true;
            this.buttonLoadSetup.Click += new System.EventHandler(this.ButtonLoadSetup_Click);
            // 
            // dataGridViewComboBoxColumn3
            // 
            this.dataGridViewComboBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewComboBoxColumn3.DataPropertyName = "Output";
            this.dataGridViewComboBoxColumn3.HeaderText = "Output";
            this.dataGridViewComboBoxColumn3.Name = "dataGridViewComboBoxColumn3";
            this.dataGridViewComboBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewComboBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dataGridViewComboBoxColumn4
            // 
            this.dataGridViewComboBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewComboBoxColumn4.DataPropertyName = "Output";
            this.dataGridViewComboBoxColumn4.HeaderText = "Output";
            this.dataGridViewComboBoxColumn4.Name = "dataGridViewComboBoxColumn4";
            this.dataGridViewComboBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewComboBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dataGridViewComboBoxColumn5
            // 
            this.dataGridViewComboBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewComboBoxColumn5.DataPropertyName = "Output";
            this.dataGridViewComboBoxColumn5.HeaderText = "Output";
            this.dataGridViewComboBoxColumn5.Name = "dataGridViewComboBoxColumn5";
            this.dataGridViewComboBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewComboBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dataGridViewComboBoxColumn6
            // 
            this.dataGridViewComboBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewComboBoxColumn6.DataPropertyName = "Output";
            this.dataGridViewComboBoxColumn6.HeaderText = "Output";
            this.dataGridViewComboBoxColumn6.Name = "dataGridViewComboBoxColumn6";
            this.dataGridViewComboBoxColumn6.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewComboBoxColumn6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // DofConfigToolSetupControl
            // 
            this.Controls.Add(this.buttonLoadSetup);
            this.Controls.Add(this.buttonSaveSetup);
            this.Controls.Add(this.buttonAddOutputMapping);
            this.Controls.Add(this.dataGridViewOutputMappings);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridViewControllers);
            this.Controls.Add(this.buttonAddController);
            this.Controls.Add(this.labelControllers);
            this.Controls.Add(this.textBoxAPIKey);
            this.Controls.Add(this.labelAPIKey);
            this.Controls.Add(this.textBoxUserName);
            this.Controls.Add(this.labelUserName);
            this.Name = "DofConfigToolSetupControl";
            this.Size = new System.Drawing.Size(962, 688);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewControllers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.controllerSetupBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOutputMappings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.outputMappingsBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #region DofConfigToolSetup Management
        private string _FileName = string.Empty;
        public string FileName
        {
            get {
                return _FileName;
            }
            set {
                LoadSetup(value);
                _FileName = (_DofConfigToolSetup != null) ? value : string.Empty;
            }
        }

        private DofConfigToolSetup _DofConfigToolSetup = new DofConfigToolSetup();
        public DofConfigToolSetup DofConfigToolSetup 
        {
            get {
                return _DofConfigToolSetup;
            }

            set {
                _DofConfigToolSetup = value;
                RemapSetup();
                Invalidate();
            }
        }

        public bool Dirty { get; private set; } = false;

        private DofConfigToolSetup.ControllerSetup SelectedController = null;
        private DofConfigToolSetup.OutputMapping SelectedMapping = null;

        private readonly int OutputColumnNum = 3;

        public DofConfigToolSetupControl()
        {
            InitializeComponent();
            RemapSetup();


            (dataGridViewOutputMappings.Columns[OutputColumnNum] as DataGridViewComboBoxColumn).DataSource = DofConfigToolOutputs.GetPublicDofOutput(true);
        }

        private void RemapSetup()
        {
            if (!FileName.IsNullOrEmpty()) {
                DofConfigToolSetup.Validate();
            }
            textBoxUserName.Text = DofConfigToolSetup.UserName;
            textBoxAPIKey.Text = DofConfigToolSetup.APIKey;
            SelectedMapping = null;
            SelectedController = null;
            dataGridViewOutputMappings.DataSource = null;
            dataGridViewOutputMappings.Refresh();
            dataGridViewControllers.DataSource = DofConfigToolSetup.ControllerSetups.ToArray();
            dataGridViewControllers.Refresh();
            Dirty = false;
        }

        private void TextBoxUserName_TextChanged(object sender, EventArgs e)
        {
            DofConfigToolSetup.UserName = textBoxUserName.Text;
            Dirty = true;
        }

        private void TextBoxAPIKey_TextChanged(object sender, EventArgs e)
        {
            DofConfigToolSetup.APIKey = textBoxAPIKey.Text;
            Dirty = true;
        }

        private void SelectLastController()
        {
            dataGridViewControllers.ClearSelection();
            dataGridViewControllers.Rows[dataGridViewControllers.Rows.Count - 1].Selected = true;
            dataGridViewControllers.CurrentCell = dataGridViewControllers.Rows[dataGridViewControllers.Rows.Count - 1].Cells[0];
            SelectedController = DofConfigToolSetup.ControllerSetups[DofConfigToolSetup.ControllerSetups.Count - 1];
            SelectedMapping = null;
        }

        private void ButtonAddController_Click(object sender, EventArgs e)
        {
            DofConfigToolSetup.ControllerSetups.Add(new DofConfigToolSetup.ControllerSetup() { Name = $"New Controller #{DofConfigToolSetup.ControllerSetups.Count}" });
            dataGridViewControllers.DataSource = DofConfigToolSetup.ControllerSetups.ToArray();
            Dirty = true;
            SelectLastController();
            dataGridViewControllers.Refresh();
        }

        private void DataGridViewControllers_KeyDown(object sender, KeyEventArgs e)
        {
            if(SelectedController != null && e.KeyCode == Keys.Delete) {
                if (MessageBox.Show($"Delete controller {SelectedController} and all its output mappings ?", "Delete controller", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    DofConfigToolSetup.ControllerSetups.Remove(SelectedController);
                    SelectedController = null;
                    dataGridViewControllers.DataSource = DofConfigToolSetup.ControllerSetups.ToArray();
                    dataGridViewControllers.Refresh();
                    SelectedMapping = null;
                    Dirty = true;
                    dataGridViewOutputMappings.DataSource = null;
                    dataGridViewOutputMappings.Refresh();
                }
            }
        }

        private void DataGridViewControllers_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0) {
                SelectedController = DofConfigToolSetup.ControllerSetups[e.RowIndex];
                dataGridViewOutputMappings.DataSource = SelectedController.OutputMappings.ToArray();
            } else {
                dataGridViewOutputMappings.DataSource = null;
            }
            SelectedMapping = null;
            dataGridViewOutputMappings.Refresh();
        }

        private void SelectLastMapping()
        {
            dataGridViewOutputMappings.ClearSelection();
            dataGridViewOutputMappings.Rows[dataGridViewOutputMappings.Rows.Count - 1].Selected = true;
            dataGridViewOutputMappings.CurrentCell = dataGridViewOutputMappings.Rows[dataGridViewOutputMappings.Rows.Count - 1].Cells[0];
            SelectedMapping = SelectedController.OutputMappings[SelectedController.OutputMappings.Count - 1];
        }

        private void ButtonAddOutputMapping_Click(object sender, EventArgs e)
        {
            if (SelectedController != null) {
                SelectedController.OutputMappings.Add(new DofConfigToolSetup.OutputMapping() { PortNumber = SelectedController.OutputMappings.Count == 0 ? 1 : SelectedController.OutputMappings.Max(O=>O.PortNumber+O.PortRange) });
                dataGridViewOutputMappings.DataSource = SelectedController.OutputMappings.ToArray();
                SelectLastMapping();
                Dirty = true;
                dataGridViewOutputMappings.Refresh();
            }
        }

        private void DataGridViewOutputMappings_KeyDown(object sender, KeyEventArgs e)
        {
            if (SelectedController != null && SelectedMapping != null && e.KeyCode == Keys.Delete) {
                if (MessageBox.Show($"Delete output mapping {SelectedMapping.PortNumber} => {SelectedMapping.Output}  ?", "Delete output mapping", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    SelectedController.OutputMappings.Remove(SelectedMapping);
                    SelectedMapping = null;
                    dataGridViewOutputMappings.DataSource = SelectedController.OutputMappings.ToArray();
                    Dirty = true;
                    dataGridViewOutputMappings.Refresh();
                }
            }
        }

        private void DataGridViewOutputMappings_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (SelectedController != null && e.RowIndex >= 0) {
                SelectedMapping = SelectedController.OutputMappings[e.RowIndex];
            }
        }

        public static readonly string FileFilter = "DofConfigToolSetup files|*.dofsetup|Xml files|*.xml|All Files|*.*";
        public static readonly string FileDefaultExt = "dofsetup";

        public void SaveSetup()
        {
            if (DofConfigToolSetup.Validate()) {
                SaveFileDialog fd = new SaveFileDialog() {
                    Filter = FileFilter,
                    DefaultExt = FileDefaultExt,
                    Title = "Save DofConfigTool Setup"
                };

                fd.ShowDialog();

                if (!fd.FileName.IsNullOrEmpty()) {
                    DofConfigToolSetup.WriteToXml(fd.FileName);
                    _FileName = fd.FileName;
                    Dirty = false;
                }
            }
        }

        private void ButtonSaveSetup_Click(object sender, EventArgs e)
        {
            SaveSetup();
        }

        private void LoadSetup(string Filename)
        {
            DofConfigToolSetup = DofConfigToolSetup.ReadFromXml(Filename);
        }

        private void ButtonLoadSetup_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog() {
                Filter = FileFilter,
                DefaultExt = FileDefaultExt,
                Title = "Load DofConfigTool Setup"
            };

            fd.ShowDialog();

            if (!fd.FileName.IsNullOrEmpty()) {
                FileName = fd.FileName;
            }
        }

        private void DataGridViewOutputMappings_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridViewOutputMappings.IsCurrentCellDirty) {
                // This fires the cell value changed handler below
                dataGridViewOutputMappings.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void DataGridViewOutputMappings_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            Dirty = true;

            DataGridViewComboBoxCell cb = (DataGridViewComboBoxCell)dataGridViewOutputMappings.Rows[e.RowIndex].Cells[OutputColumnNum];
            if (cb.Value != null) {
                dataGridViewOutputMappings.Invalidate();
            }
        }
        #endregion

    }
}
