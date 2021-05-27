using DirectOutput;
using DirectOutput.FX;
using DirectOutput.FX.MatrixFX;
using DirectOutput.FX.RGBAFX;
using DirectOutput.FX.TimmedFX;
using DirectOutput.General;
using DirectOutput.LedControl.Loader;
using DirectOutput.LedControl.Setup;
using DirectOutput.Table;
using DirectOutputControls;
using DofConfigToolWrapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DirectOutputToolkit.DirectOutputToolkitHandler;

namespace DirectOutputToolkit
{
    public partial class DirectOutputToolkitForm : Form
    {
        private Settings Settings = new Settings();

        private DofConfigToolSetup DofConfigToolSetup = null;
        private DirectOutputViewSetup DofViewSetup = null;
        private DirectOutputToolkitPreviewForm PreviewForm = new DirectOutputToolkitPreviewForm();

        private DirectOutputToolkitHandler Handler = null;

        private EditionTableTreeNode EditionTableNode;

        public DirectOutputToolkitForm()
        {
            InitializeComponent();

            Settings = Settings.LoadSettings();

            var privateDoubleBuffered = typeof(TreeView).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            privateDoubleBuffered.SetValue(treeViewEditionTable, true);
            privateDoubleBuffered.SetValue(treeViewReferenceTable, true);

            treeViewEditionTable.ShowNodeToolTips = true;

            treeViewReferenceTable.ImageList = imageListIcons;
            treeViewReferenceTable.FullRowSelect = true;
            treeViewReferenceTable.HideSelection = false;
            treeViewEditionTable.ImageList = imageListIcons;
            treeViewEditionTable.FullRowSelect = true;
            treeViewEditionTable.HideSelection = false;
        }

        public bool LoadConfig()
        {
            OpenConfigDialog OCD = new OpenConfigDialog(Settings);
            if (OCD.ShowDialog() == DialogResult.OK) {

                Handler = new DirectOutputToolkitHandler(Settings);
                Handler.ForceDofConfigToolUpdate = OCD.ForceDofConfigToolUpdate;
                DofConfigToolSetup = DofConfigToolSetup.ReadFromXml(Settings.LastDofConfigSetup);
                DofViewSetup = DirectOutputViewSetupSerializer.ReadFromXml(Settings.LastDirectOutputViewSetup);

                Handler.DofConfigToolSetup = DofConfigToolSetup;
                Handler.DofViewSetup = DofViewSetup;
                Handler.PreviewControl = PreviewForm.PreviewControl;
                if (!Handler.SetupPinball()) {
                    return false;
                }

                RomNameComboBox.Text = Settings.LastRomName;
                RomNameComboBox.Items.Clear();
                RomNameComboBox.Items.Add("");
                RomNameComboBox.Items.AddRange(Handler.LedControlConfigList[0].TableConfigurations.Select(TC => TC.ShortRomName).ToArray());
                PopulateReferenceTable(Settings.LastRomName);

                comboBoxEditionTableOutputFilter.DataSource = DofConfigToolOutputs.GetPublicDofOutput(true);
                comboBoxEditionTableOutputFilter.Text = DofConfigToolOutputEnum.Invalid.ToString();
                comboBoxRefTableOutputFilter.DataSource = DofConfigToolOutputs.GetPublicDofOutput(true);
                comboBoxRefTableOutputFilter.Text = DofConfigToolOutputEnum.Invalid.ToString();

                EditionTableNode = new EditionTableTreeNode(Handler, Handler.GetTable(DirectOutputToolkitHandler.ETableType.EditionTable));
                EditionTableNode.Rebuild(Handler);
                treeViewEditionTable.Nodes.Add(EditionTableNode);
                treeViewEditionTable.Refresh();

                PreviewForm.Show(this);

                if (Settings.LastMainWindowRect != Rectangle.Empty) {
                    this.Bounds = Settings.LastMainWindowRect;
                }

                if (Settings.LastPreviewWindowRect != Rectangle.Empty) {
                    PreviewForm.Bounds = Settings.LastPreviewWindowRect;
                } else {
                    PreviewForm.Location = new Point(Math.Min(Bounds.Right, Screen.FromControl(this).WorkingArea.Right), Bounds.Y);
                }

                PreviewForm.PreviewControl.BackgroundColor = Settings.PreviewBackgroundColor;
                PreviewForm.PreviewControl.OnSetupChanged(DofViewSetup);

                return true;
            } else {
                return false;
            }
        }

        private void SaveSettings()
        {
            Settings.LastMainWindowRect = this.Bounds;
            Settings.LastPreviewWindowRect = PreviewForm.Bounds;
            Settings.SaveSettings();
        }

        private void DirectOutputToolkitForm_Load(object sender, EventArgs e)
        {
            if (!LoadConfig()) {
                this.Close();
            }
        }

        private void DirectOutputToolkitForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            PreviewForm.Close();
            while (PreviewForm.Disposing) { Application.DoEvents(); }
            Handler?.FinishPinball();
            if (Settings.AutoSaveOnQuit) {
                SaveSettings();
            }
        }

        #region Main Menu

        private void newTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Do you really want to start a new Table ?\n" +
                                $"The table {Handler.GetTable(DirectOutputToolkitHandler.ETableType.EditionTable).TableName} will be deleted.",
                                "Create New Table",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning) == DialogResult.Yes) {
                ResetEditionTable(null);
                SetCurrentSelectedNode(EditionTableNode);
            }
        }

        private void loadTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog() {
                Filter = "DirectOutput Toolkit Files|*.dotk|DOTK XML Files|*.xml|All Files|*.*",
                DefaultExt = "dotk",
                Title = "Load Table Effects"
            };

            fd.ShowDialog();
            if (!fd.FileName.IsNullOrEmpty()) {
                ResetEditionTable(null);
                var serializer = new DirectOutputToolkitSerializer();
                if (serializer.Deserialize(EditionTableNode, fd.FileName, Handler)) {
                    PopulateTableElements(DirectOutputToolkitHandler.ETableType.EditionTable);
                    SetCurrentSelectedNode(EditionTableNode);
                    //MessageBox.Show($"Table [{EditionTableNode.EditionTable.TableName}] loaded", "Load Table Effects", MessageBoxButtons.OK);
                }
            }
        }

        private void saveTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog fd = new SaveFileDialog() {
                Filter = "DirectOutput Toolkit Files|*.dotk|DOTK XML Files|*.xml|All Files|*.*",
                DefaultExt = "dotk",
                Title = "Save Table Effects"
            };

            fd.ShowDialog();

            if (!fd.FileName.IsNullOrEmpty()) {
                //Sort all tableelementsnodes effects in their original creation ordering
                var TEnodes = EditionTableNode.Nodes.OfType<TableElementTreeNode>();
                var serializer = new DirectOutputToolkitSerializer();
                if (serializer.Serialize(EditionTableNode, fd.FileName, Handler)) {
                    SetCurrentSelectedNode(EditionTableNode);
                    MessageBox.Show($"Table [{EditionTableNode.EditionTable.TableName}] descriptor\n" +
                                    $"is saved in {fd.FileName}.", "Save Table Effects", MessageBoxButtons.OK);
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string[] ExtractVariables(string cmdLine)
        {
            var splitCommands = cmdLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var extractedVariables = new List<string>();
            foreach(var command in splitCommands) {
                if (command.StartsWith("@") && command.EndsWith("@")) {
                    var variable = command.Substring(1, command.Length - 2);
                    if (!extractedVariables.Contains(variable)) {
                        extractedVariables.Add(variable);
                    }
                }
            }
            return extractedVariables.ToArray();
        }

        private void importFromDofConfigToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DirectOutputToolkitDOFCommandsDialog dlg = new DirectOutputToolkitDOFCommandsDialog() { Handler = Handler };
            dlg.ShowDialog(this);

            if (dlg.CommandLines != null) {

                Dictionary<TableElement, List<TableConfigSetting>> TableConfigSettings = new Dictionary<TableElement, List<TableConfigSetting>>();

                int TCCNumber = EditionTableNode.EditionTable.TableElements.Count;

                var cmdLines = dlg.CommandLines.ToList();

                //Extract variables from lines
                var extractedVariables = cmdLines.Select(L => ExtractVariables(L)).ToList();

                Handler.LedControlConfigList[0].ResolveTableVariables(cmdLines);
                Handler.LedControlConfigList[0].ResolveVariables(cmdLines);

                for (int i = 0; i < cmdLines.Count; ++i) {
                    var line = cmdLines[i];
                    var variables = extractedVariables[i].ToList();
                    var Toys = Handler.GetToysFromOutput(DofConfigToolOutputs.GetOutput(dlg.OutputName));
                    foreach (var toy in Toys) {
                        var effNode = CreateEffectsFromDofCommand(EditionTableNode, TCCNumber, line, toy.Name, Handler);
                        if (variables.Count > 0 && effNode != null && effNode.Effect != null) {
                            Handler.SetEffectVariableOverrides(effNode.Effect, variables);
                        }
                        TCCNumber++;
                    }
                }
            }
            EditionTableNode.Rebuild(Handler);
        }

        private void exportToDofConfigToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DirectOutputToolkitDOFOutputs dlg = new DirectOutputToolkitDOFOutputs() { TableNode = EditionTableNode, Handler = Handler };
            dlg.ShowDialog(this);
        }

        private void OnSettingsPreviewBckColor()
        {
            PreviewForm.PreviewControl.BackgroundColor = Settings.PreviewBackgroundColor;
            PreviewForm.Refresh();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm frm = new SettingsForm() { Settings = Settings };
            frm.PreviewBackColorChanged += OnSettingsPreviewBckColor;
            frm.ShowDialog(this);
            frm.PreviewBackColorChanged -= OnSettingsPreviewBckColor;
        }
        #endregion

        #region Effect Editor
        private void ResetEditionTable(string RefRomName)
        {
            Handler.ResetEditionTable(RefRomName);
            EditionTableNode.EditionTable = Handler.GetTable(DirectOutputToolkitHandler.ETableType.EditionTable);
            EditionTableNode.Rebuild(Handler);
            ResetAllTableElements();
            UpdateActivationButton(buttonActivationEdition, 0);
            UpdateActivationButton(buttonActivationTable, 0);
            UpdatePulseButton(buttonPulseEdition, 0);
            UpdatePulseButton(buttonPulseTable, 0);
            treeViewEditionTable.Refresh();
            propertyGridMain.SelectedObject = null;
        }

        private void treeViewEditionTable_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.None) {
                var hit = treeViewEditionTable.HitTest(e.X, e.Y);
                SetCurrentSelectedNode(hit.Node);
                if (e.Button == MouseButtons.Right) {
                    ShowContextMenu(treeViewEditionTable, hit.Node, e.Location);
                } else if (e.Button == MouseButtons.Left) {
                    if (hit.Node is EffectTreeNode || hit.Node is TableElementTreeNode) {
                        DoDragDrop(hit.Node, DragDropEffects.All);
                        DragOverStartTime = DateTime.MaxValue;
                        DragOverNode = null;
                    }
                }
            }
        }

        private void buttonActivationEdition_Click(object sender, EventArgs e)
        {
            if (treeViewEditionTable.SelectedNode is ITableElementTreeNode) {
                var value = Handler.SwitchTableElement(treeViewEditionTable.SelectedNode);
                SetEffectTreeNodeActive(treeViewEditionTable.SelectedNode, value > 0 ? 1 : 0);
                UpdateActivationButton(buttonActivationEdition, value);
            } else if (treeViewEditionTable.SelectedNode is StaticEffectsTreeNode) {
                Handler.TriggerStaticEffects(DirectOutputToolkitHandler.ETableType.EditionTable);
                PreviewForm.Invalidate();
            }
        }

        private void buttonPulseEdition_MouseDown(object sender, MouseEventArgs e)
        {
            if (treeViewEditionTable.SelectedNode is ITableElementTreeNode) {
                var value = Handler.SetTableElementValue(treeViewEditionTable.SelectedNode, 1);
                SetEffectTreeNodeActive(treeViewEditionTable.SelectedNode, value > 0 ? 1 : 0);
                UpdatePulseButton(buttonPulseEdition, value);
            }
        }

        private void buttonPulseEdition_MouseUp(object sender, MouseEventArgs e)
        {
            if (treeViewEditionTable.SelectedNode is ITableElementTreeNode) {
                var value = Handler.SetTableElementValue(treeViewEditionTable.SelectedNode, 0);
                SetEffectTreeNodeActive(treeViewEditionTable.SelectedNode, value > 0 ? 1 : 0);
                UpdatePulseButton(buttonPulseEdition, value);
            }
        }


        private void DeleteEffectNode(EffectTreeNode node, bool silent = false, bool rebuild = true)
        {
            if (silent || MessageBox.Show($"Do you want to delete effect {node.Text} from {(node.Parent as TableElementTreeNode)?.TE.Name} ?", "Delete Effect", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                if (node.Parent is TableElementTreeNode TENode) {
                    TENode.TE.AssignedEffects.RemoveAll(AE => AE.Effect == node.Effect);
                    Handler.RemoveEffects(node.Effect.GetAllEffects(), (node.Parent as TableElementTreeNode)?.TE, node.GetTableType());
                    if (rebuild) {
                        TENode.Rebuild(Handler);
                    }
                    if (!silent) {
                        SetCurrentSelectedNode(TENode);
                    }
                } else if (node.Parent is StaticEffectsTreeNode staticEffectsNode) {
                    EditionTableNode.EditionTable.AssignedStaticEffects.RemoveAll(AE => AE.Effect == node.Effect);
                    Handler.RemoveEffects(node.Effect.GetAllEffects(), null, node.GetTableType());
                    if (rebuild) {
                        staticEffectsNode.Rebuild(Handler);
                        EditionTableNode.Refresh();
                    }
                    if (!silent) {
                        SetCurrentSelectedNode(staticEffectsNode);
                    }
                }
            }
        }

        private void DeleteTableElementNode(TableElementTreeNode node, bool silent = false)
        {
            if (silent || MessageBox.Show($"Do you want to delete {node.TE.Name} and all its effects ?", "Delete Table Element", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                foreach (var effNode in node.Nodes) {
                    DeleteEffectNode(effNode as EffectTreeNode, silent: true, rebuild: false);
                }
                Handler.RemoveTableElement(node.GetTableElement(), node.GetTableType());
                EditionTableNode.Rebuild(Handler);
                SetCurrentSelectedNode(EditionTableNode);
            }
        }

        private void treeViewEditionTable_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete) {

                if (treeViewEditionTable.SelectedNode is EffectTreeNode effectNode) {
                    DeleteEffectNode(effectNode);
                } else if (treeViewEditionTable.SelectedNode is TableElementTreeNode tableElementNode) {
                    DeleteTableElementNode(tableElementNode);
                } 

                e.Handled = true;
            }
        }

        private void comboBoxEditionTableOutputFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EditionTableNode != null) {
                PopulateTableElements(DirectOutputToolkitHandler.ETableType.EditionTable, comboBoxEditionTableOutputFilter.Text);
            }
        }

        private void buttonEditionTableClearFilter_Click(object sender, EventArgs e)
        {
            if (EditionTableNode != null) {
                comboBoxEditionTableOutputFilter.Text = DofConfigToolOutputEnum.Invalid.ToString();
                PopulateTableElements(DirectOutputToolkitHandler.ETableType.EditionTable);
            }
        }

        private ToolTip DragToolTip = new ToolTip();
        private TreeNode DragOverNode = null;
        private DateTime DragOverStartTime = DateTime.MaxValue;

        private void treeViewEditionTable_DragOver(object sender, DragEventArgs e)
        {
            var srcNode = e.Data.GetData(typeof(EffectTreeNode).ToString(), true) as TreeNode;
            if (srcNode == null) {
                srcNode = e.Data.GetData(typeof(TableElementTreeNode).ToString(), true) as TreeNode;
            }
            if (srcNode != null) {
                Point dscreen = new Point(e.X, e.Y);
                Point dclient = treeViewEditionTable.PointToClient(dscreen);
                var hit = treeViewEditionTable.HitTest(dclient);
                if (DragOverNode != hit.Node) {
                    DragOverNode = hit.Node;
                    DragOverStartTime = DateTime.Now;
                } else if (DragOverNode != null && DragOverNode.Nodes.Count > 0 && !DragOverNode.IsExpanded && (DateTime.Now - DragOverStartTime).Seconds >= 1.0f){
                    DragOverNode.Expand();
                    DragOverStartTime = DateTime.MaxValue;
                }
                if (hit.Node != null && hit.Node != srcNode && !(hit.Node is EffectTreeNode)) {
                    if (hit.Node.TreeView == srcNode.TreeView && (e.KeyState & 4) != 0) {
                        e.Effect = DragDropEffects.Move;
                        DragToolTip.Show($"Move {srcNode.Text} to {hit.Node.Text}", this, PointToClient(dscreen));
                    } else {
                        e.Effect = DragDropEffects.Copy;
                        DragToolTip.Show($"Copy {srcNode.Text} to {hit.Node.Text}", this, PointToClient(dscreen));
                    }
                } else {
                    e.Effect = DragDropEffects.None;
                    DragToolTip.Hide(this);
                }
            } else {
                e.Effect = DragDropEffects.None;
                DragToolTip.Hide(this);
            }
        }

        private void treeViewEditionTable_DragDrop(object sender, DragEventArgs e)
        {
            var srcNode = e.Data.GetData(typeof(EffectTreeNode).ToString(), true) as TreeNode;
            if (srcNode == null) {
                srcNode = e.Data.GetData(typeof(TableElementTreeNode).ToString(), true) as TreeNode;
            }
            DragToolTip.Hide(this);
            if (srcNode != null) {
                Point dscreen = new Point(e.X, e.Y);
                Point dclient = treeViewEditionTable.PointToClient(dscreen);
                var hit = treeViewEditionTable.HitTest(dclient);
                if (hit.Node != null && hit.Node != srcNode && !(hit.Node is EffectTreeNode)) {
                    if (srcNode is EffectTreeNode) {
                        CopyEffectToEditor(srcNode, hit.Node);
                        if ((e.KeyState & 4) != 0) {
                            DeleteEffectNode(srcNode as EffectTreeNode, silent: true, rebuild: true);
                        }
                    } else if (srcNode is TableElementTreeNode) {
                        CopyTableElementToEditor(srcNode, hit.Node);
                        if ((e.KeyState & 4) != 0) {
                            DeleteTableElementNode(srcNode as TableElementTreeNode, true);
                        }
                    }
                }
            }
        }
        #endregion


        #region Dof Table Effects
        private void PopulateReferenceTable(string romName)
        {
            Handler.SetupTable(DirectOutputToolkitHandler.ETableType.ReferenceTable, romName);
            PopulateTableElements(DirectOutputToolkitHandler.ETableType.ReferenceTable);
            Handler.LaunchTable(DirectOutputToolkitHandler.ETableType.ReferenceTable);
            PreviewForm.Invalidate();
        }

        private void RomNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.LastRomName = RomNameComboBox.Text;
            PopulateReferenceTable(Settings.LastRomName);
        }

        private void PopulateTableElements(DirectOutputToolkitHandler.ETableType TableType, string OutputFilter = "Invalid")
        {
            Handler.ResetPinball(false);

            var table = Handler.GetTable(TableType);

            var treeView = TableType == DirectOutputToolkitHandler.ETableType.EditionTable ? treeViewEditionTable : treeViewReferenceTable;

            var outputFilter = DofConfigToolOutputs.GetOutput(OutputFilter);
            var ToysFromOutputFilter = Handler.GetToysFromOutput(outputFilter);

            TreeNodeCollection rootNodes = treeView.Nodes;
            if (TableType == DirectOutputToolkitHandler.ETableType.EditionTable) {
                rootNodes = EditionTableNode.Nodes;
            }

            treeView.BeginUpdate();
            treeView.SelectedNode = null;

            rootNodes.Clear();

            var staticEffectsNode = new StaticEffectsTreeNode(table, TableType);
            staticEffectsNode.Rebuild(Handler, outputFilter != DofConfigToolOutputEnum.Invalid ? ToysFromOutputFilter : null);
            rootNodes.Add(staticEffectsNode);

            foreach (TableElement tableElement in table.TableElements) {

                if (outputFilter == DofConfigToolOutputEnum.Invalid ||
                    tableElement.AssignedEffects.Any(AE=>ToysFromOutputFilter.Contains(AE.Effect.GetAssignedToy()))) {

                    if (!tableElement.Name.StartsWith(EffectTreeNode.TableElementTestName, StringComparison.InvariantCultureIgnoreCase)) {
                        var elementName = tableElement.Name.IsNullOrEmpty() ? $"{tableElement.TableElementType}[{tableElement.Number}]" : tableElement.Name;
                        var listNode = new TableElementTreeNode(tableElement, TableType);

                        foreach (var effect in tableElement.AssignedEffects) {
                            if (outputFilter == DofConfigToolOutputEnum.Invalid ||
                                ToysFromOutputFilter.Contains(effect.Effect.GetAssignedToy())) {
                                var effectNode = new EffectTreeNode(tableElement, TableType, effect.Effect, Handler);
                                listNode.Nodes.Add(effectNode);
                            }
                        }
                        listNode.Refresh();
                        rootNodes.Add(listNode);
                    }
                }

            }

            if (TableType == DirectOutputToolkitHandler.ETableType.EditionTable) {
                EditionTableNode.Refresh();
            }

            treeView.EndUpdate();
            treeView.Refresh();
        }

        private void buttonActivationTable_Click(object sender, EventArgs e)
        {
            if (treeViewReferenceTable.SelectedNode is ITableElementTreeNode nodeWithTE) {
                var value = Handler.SwitchTableElement(treeViewReferenceTable.SelectedNode);
                SetEffectTreeNodeActive(treeViewReferenceTable.SelectedNode, value > 0 ? 1 : 0);
                UpdateActivationButton(buttonActivationTable, value);
            } else if (treeViewReferenceTable.SelectedNode is StaticEffectsTreeNode) {
                Handler.TriggerStaticEffects(DirectOutputToolkitHandler.ETableType.ReferenceTable);
                PreviewForm.Invalidate();
            }
        }

        private void buttonPulseTable_MouseDown(object sender, MouseEventArgs e)
        {
            if (treeViewReferenceTable.SelectedNode is ITableElementTreeNode) {
                var value = Handler.SetTableElementValue(treeViewReferenceTable.SelectedNode, 1);
                SetEffectTreeNodeActive(treeViewReferenceTable.SelectedNode, value > 0 ? 1 : 0);
                UpdatePulseButton(buttonPulseTable, value);
            }
        }

        private void buttonPulseTable_MouseUp(object sender, MouseEventArgs e)
        {
            if (treeViewReferenceTable.SelectedNode is ITableElementTreeNode) {
                var value = Handler.SetTableElementValue(treeViewReferenceTable.SelectedNode, 0);
                SetEffectTreeNodeActive(treeViewReferenceTable.SelectedNode, value > 0 ? 1 : 0);
                UpdatePulseButton(buttonPulseTable, value);
            }
        }

        private void treeViewReferenceTable_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.None) {
                var hit = treeViewReferenceTable.HitTest(e.X, e.Y);
                SetCurrentSelectedNode(hit.Node);
                if (e.Button == MouseButtons.Right) {
                    ShowContextMenu(treeViewReferenceTable, hit.Node, e.Location);
                } else if (e.Button == MouseButtons.Left) {
                    if (hit.Node is EffectTreeNode || hit.Node is TableElementTreeNode) {
                        DoDragDrop(hit.Node, DragDropEffects.All);
                        DragOverStartTime = DateTime.MaxValue;
                        DragOverNode = null;
                    }
                }
            }
        }

        private void comboBoxRefTableOutputFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateTableElements(DirectOutputToolkitHandler.ETableType.ReferenceTable, comboBoxRefTableOutputFilter.Text);
        }

        private void buttonClearRefTableFilter_Click(object sender, EventArgs e)
        {
            comboBoxRefTableOutputFilter.Text = DofConfigToolOutputEnum.Invalid.ToString();
            PopulateTableElements(DirectOutputToolkitHandler.ETableType.ReferenceTable, comboBoxRefTableOutputFilter.Text);
        }

        private void buttonCopyTable_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Do you really want to copy all {RomNameComboBox.Text} elements & effects to edition table ?", "Copy reference table", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) {
                return;
            }

            ResetEditionTable(RomNameComboBox.Text);

            var refTable = Handler.GetTable(DirectOutputToolkitHandler.ETableType.ReferenceTable);

            foreach(var eff in refTable.AssignedStaticEffects) {
                Handler.CreateEffect(eff.Effect, null, DirectOutputToolkitHandler.ETableType.EditionTable);
            }

            foreach(var te in refTable.TableElements) {
                var newTE = new TableElement() {
                    Name = te.Name,
                    Number = te.Number,
                    TableElementType = te.TableElementType
                };
                EditionTableNode.EditionTable.TableElements.Add(newTE);

                foreach(var eff in te.AssignedEffects) {
                    Handler.CreateEffect(eff.Effect, newTE, DirectOutputToolkitHandler.ETableType.EditionTable);
                }
            }

            PopulateTableElements(DirectOutputToolkitHandler.ETableType.EditionTable, comboBoxEditionTableOutputFilter.Text);
            treeViewEditionTable.SelectedNode = EditionTableNode;
            propertyGridMain.SelectedObject = null;
            EditionTableNode.Expand();
        }

        private void buttonLoadRefTable_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog() {
                Filter = "DirectOutput Toolkit Files|*.dotk|DOTK XML Files|*.xml|All Files|*.*",
                DefaultExt = "dotk",
                Title = "Load Table Effects"
            };

            fd.ShowDialog();
            if (!fd.FileName.IsNullOrEmpty()) {
                RomNameComboBox.Text = string.Empty;
                PopulateReferenceTable(string.Empty);
                var serializer = new DirectOutputToolkitSerializer();
                EditionTableTreeNode tableNode = new EditionTableTreeNode(Handler, Handler.GetTable(DirectOutputToolkitHandler.ETableType.ReferenceTable));
                tableNode.Rebuild(Handler);
                if (serializer.Deserialize(tableNode, fd.FileName, Handler)) {
                    PopulateTableElements(DirectOutputToolkitHandler.ETableType.ReferenceTable);
                }
            }
        }
        #endregion

        #region Property Grid
        private void propertyGridMain_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            var propertyGrid = (s as PropertyGrid);
            if (propertyGrid.SelectedObject is TableConfigSettingTypeDescriptor TCSDesc) {
                OnTableConfigSettingChanged(TCSDesc, e);
            } else if (propertyGrid.SelectedObject is TableElementTypeDescriptor TEDesc) {
                OnTableElementChanged(TEDesc, e);
            } else if (propertyGrid.SelectedObject is EditionTableTypeDescriptor ETDesc) {
                OnEditionTableChanged(ETDesc, e);
            }
        }

        private void OnEditionTableChanged(EditionTableTypeDescriptor TD, PropertyValueChangedEventArgs e)
        {
            TD.Refresh();
            if (e.ChangedItem.PropertyDescriptor.Name == "RomName") {
                TD.TableNode.OnImageChanged(TD.TableNode.Image, e.OldValue as string);
                var bitmapEffects = TD.TableNode.EditionTable.Effects.Where(E => E is IMatrixBitmapEffect).Cast<IMatrixBitmapEffect>();
                foreach(var eff in bitmapEffects) {
                    eff.BitmapFilePattern = new FilePattern("{0}\\{1}.*".Build(Handler.DofFilesHandler.UserLocalPath, TD.TableNode.EditionTable.RomName));
                }
            }
            propertyGridMain.Refresh();
            if (treeViewEditionTable.SelectedNode is EditionTableTreeNode editionTableNode) {
                if (editionTableNode.EditionTable == TD.TableNode.EditionTable) {
                    editionTableNode.Rebuild(Handler);
                    treeViewEditionTable.Refresh();
                }
            }
        }

        private void OnTableElementChanged(TableElementTypeDescriptor TD, PropertyValueChangedEventArgs e)
        {
            var TE = TD.WrappedTE;
            //Check for TE.Name

            if (TE.TableElementType == TableElementTypeEnum.NamedElement) {
                if (!TE.Name.All(C => char.IsLetterOrDigit(C) || C == '_')) {
                    var newName = string.Concat(TE.Name.Select(C => (char.IsLetterOrDigit(C) || C == '_') ? C : '_'));
                    MessageBox.Show($"Invalid Table Element Name [{TE.Name}]\n" +
                                    $"It must only contains letters, numbers & '_' characters.\n" +
                                    $"Will set name to [{newName}]", "Wrong Table Element Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TE.Name = newName;
                }
            } 

            TE.Name = TE.Name.Replace(" ", "_");

            var sameTETypes = Handler.GetTable(ETableType.EditionTable).TableElements.Where(E => E != TE && E.TableElementType == TE.TableElementType);
            if( TE.TableElementType == TableElementTypeEnum.NamedElement) {
                while (sameTETypes.Any(E => E.Name.Equals(TE.Name, StringComparison.InvariantCultureIgnoreCase))) {
                    TE.Name += "0";
                }
            } else {
                while (sameTETypes.Any(E => E.Number == TE.Number)) {
                    TE.Number++;
                }
            }

            TD.Refresh();
            propertyGridMain.Refresh();
            if (treeViewEditionTable.SelectedNode is TableElementTreeNode editionTETreeNode) {
                if (editionTETreeNode.TE == TE) {
                    Handler.RemoveTableElement(TE, ETableType.EditionTable);
                    editionTETreeNode.Rebuild(Handler);
                    Handler.AddTableElement(TE, ETableType.EditionTable);
                    treeViewEditionTable.Refresh();
                }
            }
        }

        private void OnTableConfigSettingChanged(TableConfigSettingTypeDescriptor TD, PropertyValueChangedEventArgs e)
        {
            var TCS = TD.WrappedTCS;
            TD.Refresh();
            propertyGridMain.Refresh();
            if (treeViewEditionTable.SelectedNode is EffectTreeNode editionEffectTreeNode) {
                if (editionEffectTreeNode.TCS == TCS) {
                    TCS.ColorName = TCS.ColorConfig?.Name ?? string.Empty;
                    TCS.ColorName2 = TCS.ColorConfig2?.Name ?? string.Empty;
                    editionEffectTreeNode.Rebuild(Handler, null);
                    treeViewEditionTable.Refresh();
                }
            }
        }
        #endregion


        #region Helpers
        private void SetCurrentSelectedNode(TreeNode node)
        {
            var value = 0;
            if (node is ITableElementTreeNode tableElementNode) {
                value = tableElementNode.GetTableElement().Value;
                if (tableElementNode.GetTableType() == DirectOutputToolkitHandler.ETableType.EditionTable) {
                    UpdateActivationButton(buttonActivationEdition, value);
                    UpdatePulseButton(buttonPulseEdition, value);
                    treeViewEditionTable.SelectedNode = node;
                    treeViewEditionTable.Refresh();
                } else {
                    UpdateActivationButton(buttonActivationTable, value);
                    UpdatePulseButton(buttonPulseTable, value);
                    treeViewReferenceTable.SelectedNode = node;
                    treeViewReferenceTable.Refresh();
                }
            } else {
                treeViewEditionTable.SelectedNode = node;
                treeViewEditionTable.Refresh();
            }

            //Update property grid
            if (node is EffectTreeNode effectNode) {
                propertyGridMain.SelectedObject = new TableConfigSettingTypeDescriptor(effectNode, effectNode.Effect != null && node.TreeView == treeViewEditionTable, Handler);
            } else if (node is TableElementTreeNode TENode) {
                propertyGridMain.SelectedObject = new TableElementTypeDescriptor(TENode.TE, node.TreeView == treeViewEditionTable);
            } else if (node is EditionTableTreeNode editionTableNode) {
                propertyGridMain.SelectedObject = new EditionTableTypeDescriptor(editionTableNode, node.TreeView == treeViewEditionTable);
            } else {
                propertyGridMain.SelectedObject = null;
            }
            propertyGridMain.Refresh();
        }

        private void SetEffectTreeNodeActive(TreeNode node, int active)
        {
            node.ImageIndex = active;
            node.SelectedImageIndex = active;
        }

        private void UpdateActivationButton(Button but, int value)
        {
            but.Text = value > 0 ? "Deactivate" : "Activate";
            but.Refresh();
        }

        private void UpdatePulseButton(Button but, int value)
        {
            but.Text = value > 0 ? @"Pulse ¯\_/¯" : @"Pulse _/¯\_";
            but.Refresh();
        }


        private void ResetTreeNodeRecursive(TreeNode node)
        {
            SetEffectTreeNodeActive(node, 0);
            if (node is ITableElementTreeNode tableElementNode) {
                tableElementNode.GetTableElement().Value = 0;
            }
            foreach (var child in node.Nodes) {
                ResetTreeNodeRecursive(child as TreeNode);
            }
        }

        private void ResetAllTableElements()
        {
            foreach (var node in treeViewReferenceTable.Nodes) {
                ResetTreeNodeRecursive(node as TreeNode);
            }
            ResetTreeNodeRecursive(EditionTableNode);
        }

        private static string NewTableElementName = "Table_Element_";

        private void OnCreateTableElement(object sender, EventArgs e)
        {
            var EditionTable = Handler.GetTable(DirectOutputToolkitHandler.ETableType.EditionTable);

            var newTE = new TableElement() { TableElementType = TableElementTypeEnum.NamedElement, Name = $"{NewTableElementName}{EditionTable.TableElements.Count}" };
            newTE.AssignedEffects = new AssignedEffectList();
            EditionTable.TableElements.Add(newTE);
            var TENode = new TableElementTreeNode(newTE, DirectOutputToolkitHandler.ETableType.EditionTable);
            EditionTableNode.Nodes.Add(TENode);
            EditionTableNode.Refresh();
            SetCurrentSelectedNode(TENode);
        }

        private void OnCreateEffect(object sender, EventArgs e, string dofCommand, DofConfigToolOutputEnum output)
        {
            var item = (sender as MenuItem);
            var command = (item.Tag as TreeNodeCommand);
            var TENode = (command.Source as TableElementTreeNode);
            var StaticNode = (command.Source as StaticEffectsTreeNode);

            int TCCNumber = EditionTableNode.EditionTable.TableElements.Count;

            var Toy = Handler.GetToysFromOutput(output).FirstOrDefault();

            var fullCommand = StaticNode != null ? $"ON {dofCommand}" : $"{TENode.TE} {dofCommand}";
            var effectNode = CreateEffectsFromDofCommand(EditionTableNode, TCCNumber, fullCommand, Toy?.Name, Handler);
            EditionTableNode.Refresh();
            treeViewEditionTable.SelectedNode = effectNode;
            SetCurrentSelectedNode(effectNode);
        }

        private void OnCreateAnalogAlphaEffect(object sender, EventArgs e)
        {
            OnCreateEffect(sender, e, "I48", DofConfigToolOutputEnum.StartButton);
        }

        private void OnCreateRGBColorEffect(object sender, EventArgs e)
        {
            OnCreateEffect(sender, e, "Magenta", DofConfigToolOutputEnum.Flasher5_Center);
        }

        private void OnCreateMxAreaEffect(object sender, EventArgs e)
        {
            OnCreateEffect(sender, e, "Magenta AL0 AT0 AW100 AH100", DofConfigToolOutputEnum.PFBackEffectsMX);
        }


        private class NodeSorter : IComparer
        {
            public enum SortEffectsEnum
            {
                Original,
                NameAscending,
                NameDescending,
                StartTimeAscending,
                StartTimeDescending,
            }

            public TableElementTreeNode TENode = null;
            public SortEffectsEnum SortType = SortEffectsEnum.Original;
            public Table EditionTable = null;

            public NodeSorter() { }

            public int Compare(object o1, object o2)
            {
                var node1 = o1 as EffectTreeNode;
                var node2 = o2 as EffectTreeNode;

                if (TENode == null || node1 == null || node2 == null ||
                    node1.Parent != TENode || node2.Parent != TENode) {
                    return 0;
                }

                int StartTimeComparison(IEffect E1, IEffect E2)
                {
                    var startTime1 = (E1.GetAllEffects().FirstOrDefault(E => E is DelayEffect) as DelayEffect)?.DelayMs ?? 0;
                    var startTime2 = (E2.GetAllEffects().FirstOrDefault(E => E is DelayEffect) as DelayEffect)?.DelayMs ?? 0;
                    return startTime1.CompareTo(startTime2);
                }

                switch (SortType) {
                    case SortEffectsEnum.NameAscending:
                        return node1.Text.CompareTo(node2.Text);
                    case SortEffectsEnum.NameDescending:
                        return node2.Text.CompareTo(node1.Text);
                    case SortEffectsEnum.StartTimeAscending:
                        return StartTimeComparison(node1.Effect, node2.Effect);
                    case SortEffectsEnum.StartTimeDescending:
                        return StartTimeComparison(node2.Effect, node1.Effect);
                    case SortEffectsEnum.Original:
                        if (EditionTable == null) return 0;
                        return EditionTable.Effects.IndexOf(node1.Effect).CompareTo(EditionTable.Effects.IndexOf(node2.Effect));
                    default:
                        break;
                }

                return 0;
            }
        }

        private void SortTableElementNode(TableElementTreeNode TENode, NodeSorter.SortEffectsEnum SortType)
        {
            TENode.TreeView.TreeViewNodeSorter = new NodeSorter() { TENode = TENode, SortType = SortType, EditionTable = Handler.GetTable(ETableType.EditionTable) };
            TENode.TreeView.Sort();
        }

        private void OnSortTableElementEffects(object sender, EventArgs e)
        {
            var item = (sender as MenuItem);
            var command = (item.Tag as TreeNodeCommand);
            var TENode = (command.Source as TableElementTreeNode);
            var SortType = (NodeSorter.SortEffectsEnum)(command.Params);

            SortTableElementNode(TENode, SortType);

            SetCurrentSelectedNode(TENode);
        }

        private void CopyEffectToEditor(TreeNode srcNode, TreeNode dstNode)
        {
            var SrcEffectNode = (srcNode as EffectTreeNode);
            var TENode = (dstNode as TableElementTreeNode);
            var StaticNode = (dstNode as StaticEffectsTreeNode);
            var parentTE = TENode?.TE ?? null;
            var EditionTable = Handler.GetTable(DirectOutputToolkitHandler.ETableType.EditionTable);

            List<TableElementTreeNode> targetTENodes = new List<TableElementTreeNode>();

            if (TENode != null) {
                targetTENodes.Add(TENode);
            }

            if (TENode == null && StaticNode == null) {
                //If it's a condition, find or create TE
                if (SrcEffectNode.TCS.OutputControl == OutputControlEnum.Condition) {

                } else {
                    parentTE = new TableElement() { TableElementType = TableElementTypeEnum.NamedElement, Name = $"{NewTableElementName}{EditionTable.TableElements.Count}" };
                    parentTE.AssignedEffects = new AssignedEffectList();
                    EditionTable.TableElements.Add(parentTE);
                    TENode = new TableElementTreeNode(parentTE, DirectOutputToolkitHandler.ETableType.EditionTable);
                    targetTENodes.Add(TENode);
                    EditionTableNode.Nodes.Add(TENode);
                    EditionTableNode.Refresh();
                }
            }

            if (targetTENodes.Count > 0) {
                foreach (var node in targetTENodes) {
                    var newEffectNode = new EffectTreeNode(node.TE, DirectOutputToolkitHandler.ETableType.EditionTable, SrcEffectNode.Effect, Handler);
                    newEffectNode.Rebuild(Handler, SrcEffectNode.Effect);
                    parentTE.AssignedEffects.Init(EditionTable);
                    TENode.Nodes.Add(newEffectNode);
                    TENode.Rebuild(Handler);
                    SetCurrentSelectedNode(TENode);
                }
            } else {
                var newEffectNode = new EffectTreeNode(null, DirectOutputToolkitHandler.ETableType.EditionTable, SrcEffectNode.Effect, Handler);
                newEffectNode.Rebuild(Handler, SrcEffectNode.Effect);
                EditionTable.AssignedStaticEffects.Init(EditionTable);
                EditionTableNode.StaticEffectsNode.Rebuild(Handler);
                EditionTableNode.Refresh();
                SetCurrentSelectedNode(EditionTableNode.StaticEffectsNode);
            }
        }

        private void OnCopyEffectToEditor(object sender, EventArgs e)
        {
            var item = (sender as MenuItem);
            var command = (item.Tag as TreeNodeCommand);
            CopyEffectToEditor(command.Source, command.Target);
        }

        private void OnEffectVariableSwitch(object sender, EventArgs e)
        {
            var item = (sender as MenuItem);
            var command = (item.Tag as TreeNodeCommand);
            var SrcEffectNode = (command.Source as EffectTreeNode);

            Handler.SwitchEffectVariableOverride(SrcEffectNode.Effect, item.Text);
            SrcEffectNode.Rebuild(Handler, null);
            propertyGridMain.Refresh();
        }

        private void OnEffectVariableClear(object sender, EventArgs e)
        {
            var item = (sender as MenuItem);
            var command = (item.Tag as TreeNodeCommand);
            var SrcEffectNode = (command.Source as EffectTreeNode);
            Handler.ClearEffectVariableOverrides(SrcEffectNode.Effect);
            SrcEffectNode.Rebuild(Handler, null);
            propertyGridMain.Refresh();
        }


        private void CopyTableElementToEditor(TreeNode srcNode, TreeNode dstNode)
        {
            var SrcTENode = (srcNode as TableElementTreeNode);
            var TargetTENode = (dstNode as TableElementTreeNode);
            var StaticNode = (dstNode as StaticEffectsTreeNode);
            var parentTE = TargetTENode?.TE ?? null;
            var EditionTable = Handler.GetTable(DirectOutputToolkitHandler.ETableType.EditionTable);

            if (TargetTENode == null && StaticNode == null) {
                parentTE = new TableElement() { TableElementType = TableElementTypeEnum.NamedElement, Name = $"{NewTableElementName}{EditionTable.TableElements.Count}" };
                parentTE.AssignedEffects = new AssignedEffectList();
                EditionTable.TableElements.Add(parentTE);
                TargetTENode = new TableElementTreeNode(parentTE, DirectOutputToolkitHandler.ETableType.EditionTable);
                EditionTableNode.Nodes.Add(TargetTENode);
                EditionTableNode.Refresh();
            }

            if (TargetTENode != null) {
                foreach (var node in SrcTENode.Nodes) {
                    var effectNode = node as EffectTreeNode;
                    var newEffectNode = new EffectTreeNode(parentTE, DirectOutputToolkitHandler.ETableType.EditionTable, effectNode.Effect, Handler);
                    newEffectNode.Rebuild(Handler, effectNode.Effect);
                }
                parentTE.AssignedEffects.Init(EditionTable);
                TargetTENode.Rebuild(Handler);
                SetCurrentSelectedNode(TargetTENode);
            } else {
                foreach (var node in SrcTENode.Nodes) {
                    var effectNode = node as EffectTreeNode;
                    var newEffectNode = new EffectTreeNode(null, DirectOutputToolkitHandler.ETableType.EditionTable, effectNode.Effect, Handler);
                    newEffectNode.Rebuild(Handler, effectNode.Effect);
                    EditionTableNode.StaticEffectsNode.Nodes.Add(newEffectNode);
                }
                EditionTable.AssignedStaticEffects.Init(EditionTable);
                EditionTableNode.StaticEffectsNode.Rebuild(Handler);
                EditionTableNode.Refresh();
                SetCurrentSelectedNode(EditionTableNode.StaticEffectsNode);
            }
        }

        private void OnCopyTableElementToEditor(object sender, EventArgs e)
        {
            var item = (sender as MenuItem);
            var command = (item.Tag as TreeNodeCommand);
            CopyTableElementToEditor(command.Source, command.Target);
        }

        private void OnInsertTableElementToEditor(object sender, EventArgs e)
        {
            var item = (sender as MenuItem);
            var command = (item.Tag as TreeNodeCommand);
            var SrcTENode = (command.Source as TableElementTreeNode);
            var TargetTENode = (command.Target as TableElementTreeNode);
            var EditionTable = Handler.GetTable(DirectOutputToolkitHandler.ETableType.EditionTable);

            var parentTE = new TableElement() { TableElementType = TableElementTypeEnum.NamedElement, Name = $"{NewTableElementName}{EditionTable.TableElements.Count}" };
            parentTE.AssignedEffects = new AssignedEffectList();
            EditionTable.TableElements.Add(parentTE);
            var index = EditionTableNode.Nodes.IndexOf(TargetTENode);
            var newTENode = new TableElementTreeNode(parentTE, DirectOutputToolkitHandler.ETableType.EditionTable);
            EditionTableNode.Nodes.Insert(index, newTENode);
            EditionTableNode.Refresh();

            foreach (var node in SrcTENode.Nodes) {
                var effectNode = node as EffectTreeNode;
                parentTE.AssignedEffects.Add(effectNode.Effect.Name);
            }
            parentTE.AssignedEffects.Init(EditionTable);
            newTENode.Rebuild(Handler);
            SetCurrentSelectedNode(newTENode);
        }

        private void OnCopyDofCommandToClipboard(object sender, EventArgs e)
        {
            var item = (sender as MenuItem);
            var command = (item.Tag as TreeNodeCommand);
            var SrcEffNode = (command.Source as EffectTreeNode);

            if (SrcEffNode != null) {
                Clipboard.SetText(SrcEffNode.DofConfigCommand);
            }
        }

        public EffectTreeNode CreateEffectsFromDofCommand(EditionTableTreeNode TableNode, int TCCNumber, string DofCommand, string ToyName, DirectOutputToolkitHandler Handler)
        {
            TableConfigSetting TCS = new TableConfigSetting();
            TCS.ParseSettingData(DofCommand);
            TCS.ResolveColorConfigs(Handler.ColorConfigurations);

            int LastEffectsCount = EditionTableNode.EditionTable.Effects.Count;

            var Toy = Handler.Toys.FirstOrDefault(T => T.Name.Equals(ToyName, StringComparison.InvariantCultureIgnoreCase));

            var newEffect = Handler.RebuildConfigurator.CreateEffect(TCS, TCCNumber, TCCNumber, TableNode.EditionTable
                                                                        , Toy
                                                                        , Handler.GetToyLedwizNum(ToyName)
                                                                        , Handler.DofFilesHandler.UserLocalPath
                                                                        , TableNode.EditionTable.RomName);

            for (var num = LastEffectsCount; num < EditionTableNode.EditionTable.Effects.Count; ++num) {
                EditionTableNode.EditionTable.Effects[num].Init(EditionTableNode.EditionTable);
            }

            EffectTreeNode returnNode = null;

            TableNode.EditionTable.AssignedStaticEffects.Init(TableNode.EditionTable);
            if (TableNode.EditionTable.AssignedStaticEffects.FirstOrDefault(AE=>AE.Effect == newEffect) != null) {
                TableNode.StaticEffectsNode.Rebuild(Handler);
                returnNode = TableNode.StaticEffectsNode.Nodes.Cast<EffectTreeNode>().FirstOrDefault(EN => EN.Effect == newEffect);
            }

            if (returnNode == null) {
                foreach (var te in TableNode.EditionTable.TableElements) {
                    te.AssignedEffects.Init(TableNode.EditionTable);
                    if (!te.Name.StartsWith(EffectTreeNode.TableElementTestName, StringComparison.InvariantCultureIgnoreCase)) {
                        var teNode = TableNode.Nodes.OfType<TableElementTreeNode>().Cast<TableElementTreeNode>().FirstOrDefault(N => N.TE == te);
                        if (teNode == null) {
                            teNode = new TableElementTreeNode(te, DirectOutputToolkitHandler.ETableType.EditionTable);
                            TableNode.Nodes.Add(teNode);
                        }
                        if (teNode.TE.AssignedEffects.Any(AE => AE.Effect == newEffect)) {
                            teNode.Rebuild(Handler);
                            returnNode = teNode.Nodes.Cast<EffectTreeNode>().FirstOrDefault(EN => EN.Effect == newEffect);
                        }
                    }
                }
            }

            return returnNode;
        }


        private void ShowContextMenu(TreeView treeview, TreeNode treeNode, Point location)
        {
            if (treeNode is EffectTreeNode effectNode) {
                ContextMenu effectMenu = new ContextMenu();

                effectMenu.MenuItems.Add(new MenuItem("Copy Dof command to clipboard", new EventHandler(this.OnCopyDofCommandToClipboard)) { Tag = new TreeNodeCommand() { Source = treeNode } });

                var addMenu = new MenuItem("Add effect to");
                effectMenu.MenuItems.Add(addMenu);

                if (effectNode.TCS.OutputControl != OutputControlEnum.Condition) {
                    addMenu.MenuItems.Add(new MenuItem("New Table Element", new EventHandler(this.OnCopyEffectToEditor)) { Tag = new TreeNodeCommand() { Source = treeNode, Target = null } });
                    foreach (var node in EditionTableNode.Nodes) {
                        addMenu.MenuItems.Add(new MenuItem($"{(node as TreeNode).Text}", new EventHandler(this.OnCopyEffectToEditor)) { Tag = new TreeNodeCommand() { Source = treeNode, Target = (node as TreeNode) } });
                    }
                } else {
                    addMenu.MenuItems.Add(new MenuItem("Edition table", new EventHandler(this.OnCopyEffectToEditor)) { Tag = new TreeNodeCommand() { Source = treeNode, Target = null } });
                }

                if (treeview == treeViewEditionTable) {
                    effectMenu.MenuItems.Add("-");

                    var variableMenu = new MenuItem("Variables");
                    effectMenu.MenuItems.Add(variableMenu);

                    variableMenu.MenuItems.Add(new MenuItem($"Clear Variables", new EventHandler(this.OnEffectVariableClear)) { Tag = new TreeNodeCommand() { Source = treeNode } });
                    variableMenu.MenuItems.Add("-");

                    var variables = Handler.GetCategorizedVariables(effectNode.Effect);
                    foreach (var variable in variables) {
                        variableMenu.MenuItems.Add(new MenuItem($"{variable}", new EventHandler(this.OnEffectVariableSwitch)) { Checked = Handler.GetEffectVariableOverrides(effectNode.Effect).Contains(variable), Tag = new TreeNodeCommand() { Source = treeNode } });
                    }

                }

                effectMenu.Show(effectNode.GetTableType() == DirectOutputToolkitHandler.ETableType.EditionTable ? treeViewEditionTable : treeViewReferenceTable, location);
            } else if (treeNode is TableElementTreeNode tableElementNode) {
                ContextMenu teMenu = new ContextMenu();

                if (treeview == treeViewEditionTable) {
                    var createMenu = new MenuItem("Create new effect");
                    teMenu.MenuItems.Add(createMenu);
                    createMenu.MenuItems.Add(new MenuItem("AnalogAlpha Effect", new EventHandler(this.OnCreateAnalogAlphaEffect)) { Tag = new TreeNodeCommand() { Source = treeNode, Target = null } });
                    createMenu.MenuItems.Add(new MenuItem("RGB Color Effect", new EventHandler(this.OnCreateRGBColorEffect)) { Tag = new TreeNodeCommand() { Source = treeNode, Target = null } });
                    createMenu.MenuItems.Add(new MenuItem("Mx Area Effect", new EventHandler(this.OnCreateMxAreaEffect)) { Tag = new TreeNodeCommand() { Source = treeNode, Target = null } });
                    teMenu.MenuItems.Add(new MenuItem("-"));
                }

                teMenu.MenuItems.Add(new MenuItem($"Add [{tableElementNode.ToString()}] to {Handler.GetTable(DirectOutputToolkitHandler.ETableType.EditionTable).TableName}", new EventHandler(this.OnCopyTableElementToEditor)) { Tag = new TreeNodeCommand() { Source = treeNode, Target = null } });

                if (EditionTableNode.Nodes.Count > 0) {
                    var insertMenu = new MenuItem($"Copy [{tableElementNode.ToString()}] into");
                    teMenu.MenuItems.Add(insertMenu);
                    foreach (var node in EditionTableNode.Nodes) {
                        insertMenu.MenuItems.Add(new MenuItem($"{(node as TreeNode).Text}", new EventHandler(this.OnCopyTableElementToEditor)) { Tag = new TreeNodeCommand() { Source = treeNode, Target = (node as TreeNode) } });
                    }

                    insertMenu = new MenuItem($"Insert [{tableElementNode.ToString()}] before");
                    teMenu.MenuItems.Add(insertMenu);
                    foreach (var node in EditionTableNode.Nodes) {
                        if (node is TableElementTreeNode) {
                            insertMenu.MenuItems.Add(new MenuItem($"{(node as TreeNode).Text}", new EventHandler(this.OnInsertTableElementToEditor)) { Tag = new TreeNodeCommand() { Source = treeNode, Target = (node as TreeNode) } });
                        }
                    }
                }

                if (treeview == treeViewEditionTable) {
                    teMenu.MenuItems.Add(new MenuItem("-"));
                    var sortMenu = new MenuItem("Sort effects");
                    teMenu.MenuItems.Add(sortMenu);
                    sortMenu.MenuItems.Add(new MenuItem("Original ordering", new EventHandler(this.OnSortTableElementEffects)) { Tag = new TreeNodeCommand() { Source = treeNode, Params = NodeSorter.SortEffectsEnum.Original } });
                    sortMenu.MenuItems.Add(new MenuItem("By name (ascending)", new EventHandler(this.OnSortTableElementEffects)) { Tag = new TreeNodeCommand() { Source = treeNode, Params = NodeSorter.SortEffectsEnum.NameAscending } });
                    sortMenu.MenuItems.Add(new MenuItem("By name (descending)", new EventHandler(this.OnSortTableElementEffects)) { Tag = new TreeNodeCommand() { Source = treeNode, Params = NodeSorter.SortEffectsEnum.NameDescending } });
                    sortMenu.MenuItems.Add(new MenuItem("By start time (ascending)", new EventHandler(this.OnSortTableElementEffects)) { Tag = new TreeNodeCommand() { Source = treeNode, Params = NodeSorter.SortEffectsEnum.StartTimeAscending } });
                    sortMenu.MenuItems.Add(new MenuItem("By start time (descending)", new EventHandler(this.OnSortTableElementEffects)) { Tag = new TreeNodeCommand() { Source = treeNode, Params = NodeSorter.SortEffectsEnum.StartTimeDescending } });
                }

                teMenu.Show(tableElementNode.GetTableType() == DirectOutputToolkitHandler.ETableType.EditionTable ? treeViewEditionTable : treeViewReferenceTable, location);
            } else if (treeNode is StaticEffectsTreeNode staticNode) {
                ContextMenu staticMenu = new ContextMenu();

                if (treeview == treeViewEditionTable) {
                    var createMenu = new MenuItem("Create new static effect");
                    staticMenu.MenuItems.Add(createMenu);
                    createMenu.MenuItems.Add(new MenuItem("AnalogAlpha Effect", new EventHandler(this.OnCreateAnalogAlphaEffect)) { Tag = new TreeNodeCommand() { Source = treeNode, Target = null } });
                    createMenu.MenuItems.Add(new MenuItem("RGB Color Effect", new EventHandler(this.OnCreateRGBColorEffect)) { Tag = new TreeNodeCommand() { Source = treeNode, Target = null } });
                    createMenu.MenuItems.Add(new MenuItem("Mx Area Effect", new EventHandler(this.OnCreateMxAreaEffect)) { Tag = new TreeNodeCommand() { Source = treeNode, Target = null } });
                }

                staticMenu.Show(treeview, location);
            } else if (treeNode is EditionTableTreeNode editionTableNode) {
                ContextMenu effMenu = new ContextMenu();
                effMenu.MenuItems.Add(new MenuItem("Create new table element", new EventHandler(this.OnCreateTableElement)) { Tag = new TreeNodeCommand() { Source = null, Target = null } });
                effMenu.Show(treeview, location);
            }
        }

        bool ShowDebugNodes = false;
        ToolTip DebugNodeTooltip = new ToolTip() { InitialDelay = 500 };

        private void treeViewEditionTable_MouseMove(object sender, MouseEventArgs e)
        {
            if (!ShowDebugNodes) return;

            var hit = treeViewEditionTable.HitTest(e.X, e.Y);
            if (hit.Node == null) {
                DebugNodeTooltip.Hide(this);
            } else {
                var message = string.Empty;

                if (hit.Node is EditionTableTreeNode tableNode) {
                    message = $"{tableNode.EditionTable.AssignedStaticEffects.Count} static effects\n" +
                              $"{tableNode.EditionTable.Effects.Count} effects\n" +
                              $"{string.Join("\n", tableNode.EditionTable.Effects.Select(E => E.Name))}" +
                              $"{tableNode.EditionTable.TableElements.Count} tableElements\n" +
                              $"{tableNode.EditionTable.Bitmaps.Count} bitmaps";
                }

                if (message != string.Empty) {
                    DebugNodeTooltip.Show(message, this, e.Location.X + 10, e.Location.Y);
                } else {
                    DebugNodeTooltip.Hide(this);
                }
            }


        }
        #endregion

    }
}
