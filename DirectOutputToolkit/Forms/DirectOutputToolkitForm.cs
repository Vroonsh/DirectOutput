using DirectOutput;
using DirectOutput.FX;
using DirectOutput.LedControl.Loader;
using DirectOutput.Table;
using DirectOutputControls;
using DofConfigToolWrapper;
using System;
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
            privateDoubleBuffered.SetValue(treeViewTableEffects, true);

            treeViewTableEffects.ImageList = imageListIcons;
            treeViewTableEffects.FullRowSelect = true;
            treeViewTableEffects.HideSelection = false;
            treeViewEditionTable.ImageList = imageListIcons;
            treeViewEditionTable.FullRowSelect = true;
            treeViewEditionTable.HideSelection = false;
        }

        public bool LoadConfig()
        {
            OpenConfigDialog OCD = new OpenConfigDialog(Settings);
            if (OCD.ShowDialog() == DialogResult.OK) {

                Handler = new DirectOutputToolkitHandler(Settings);

                DofConfigToolSetup = DofConfigToolSetup.ReadFromXml(Settings.LastDofConfigSetup);
                DofViewSetup = DirectOutputViewSetupSerializer.ReadFromXml(Settings.LastDofViewSetup);

                Handler.DofConfigToolSetup = DofConfigToolSetup;
                Handler.DofViewSetup = DofViewSetup;
                Handler.PreviewControl = PreviewForm.PreviewControl;
                if (!Handler.SetupPinball()) {
                    return false;
                }

                PreviewForm.Show(this);
                Screen current = Screen.FromControl(this);
                PreviewForm.Location = new Point(Math.Min(Bounds.Right, current.WorkingArea.Right), Bounds.Y);
                PreviewForm.PreviewControl.OnSetupChanged(DofViewSetup);

                RomNameComboBox.Items.Clear();
                RomNameComboBox.Items.Add("");
                RomNameComboBox.Items.AddRange(Handler.LedControlConfigList[0].TableConfigurations.Select(TC => TC.ShortRomName).ToArray());

                EditionTableNode = new EditionTableTreeNode(Handler.GetTable(DirectOutputToolkitHandler.ETableType.EditionTable));
                EditionTableNode.Rebuild(Handler);
                treeViewEditionTable.Nodes.Add(EditionTableNode);
                treeViewEditionTable.Refresh();

                return true;
            } else {
                return false;
            }
        }

        private void SaveSettings()
        {
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
            Handler?.FinishPinball();
            SaveSettings();
        }

        #region Main Menu

        private void newTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Do you really want to start a new Table ?\n" +
                                $"The table {Handler.GetTable(DirectOutputToolkitHandler.ETableType.EditionTable).TableName} will be deleted.",
                                "Create New Table",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning) == DialogResult.Yes) {
                ResetEditionTable();
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
                ResetEditionTable();
                var serializer = new DirectOutputToolkitSerializer();
                if (serializer.Deserialize(EditionTableNode, fd.FileName, Handler)) {
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

        private void importFromDofConfigToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DirectOutputToolkitDOFCommandsDialog dlg = new DirectOutputToolkitDOFCommandsDialog() { Handler = Handler, AvailableToys = Handler.Toys.Select(T=>T.Name).ToArray() };
            dlg.ShowDialog(this);

            if (dlg.CommandLines != null) {

                Dictionary<TableElement, List<TableConfigSetting>> TableConfigSettings = new Dictionary<TableElement, List<TableConfigSetting>>();

                int TCCNumber = EditionTableNode.EditionTable.TableElements.Count;

                var cmdLines = dlg.CommandLines.ToList();
                Handler.LedControlConfigList[0].ResolveTableVariables(cmdLines);
                Handler.LedControlConfigList[0].ResolveVariables(cmdLines);

                foreach (var line in cmdLines) {
                    CreateEffectsFromDofCommand(EditionTableNode, TCCNumber, line, dlg.ToyName, Handler);
                    TCCNumber++;
                }
            }
            EditionTableNode.Rebuild(Handler);
        }

        private void exportToDofConfigToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DirectOutputToolkitDOFOutputs dlg = new DirectOutputToolkitDOFOutputs() { TableNode = EditionTableNode, Handler = Handler };
            dlg.ShowDialog(this);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm frm = new SettingsForm() { Settings = Settings };
            frm.ShowDialog(this);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Effect Editor
        private void ResetEditionTable()
        {
            Handler.ResetEditionTable();
            EditionTableNode.EditionTable = Handler.GetTable(DirectOutputToolkitHandler.ETableType.EditionTable);
            EditionTableNode.Rebuild(Handler);
            ResetAllTableElements();
            UpdateActivationButton(buttonActivationEdition, 0);
            UpdateActivationButton(buttonActivationTable, 0);
            UpdatePulseButton(buttonPulseEdition, 0);
            UpdatePulseButton(buttonPulseTable, 0);
        }

        private void treeViewEffect_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.None) {
                //Right mouse
                if (e.Button == MouseButtons.Right) {
                    ShowContextMenu(e);
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

        private void buttonPulseEdition_Click(object sender, EventArgs e)
        {
            if (treeViewEditionTable.SelectedNode is ITableElementTreeNode) {
                var value = Handler.SwitchTableElement(treeViewEditionTable.SelectedNode);
                SetEffectTreeNodeActive(treeViewEditionTable.SelectedNode, value > 0 ? 1 : 0);
                UpdatePulseButton(buttonPulseEdition, value);
                Thread.Sleep(Settings.PulseDurationMs);
                value = Handler.SwitchTableElement(treeViewEditionTable.SelectedNode);
                SetEffectTreeNodeActive(treeViewEditionTable.SelectedNode, value > 0 ? 1 : 0);
                UpdatePulseButton(buttonPulseEdition, value);
            }
        }

        private void DeleteEffectNode(EffectTreeNode node, bool silent = false)
        {
            if (silent || MessageBox.Show($"Do you want to delete effect {node.Text} from {(node.Parent as TableElementTreeNode)?.TE.Name} ?", "Delete Effect", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                if (node.Parent is TableElementTreeNode TENode) {
                    TENode.TE.AssignedEffects.RemoveAll(AE => AE.Effect == node.Effect);
                    Handler.RemoveEffects(new List<IEffect> { node.Effect }, (node.Parent as TableElementTreeNode)?.TE, node.GetTableType());
                    if (!silent) {
                        TENode.Rebuild(Handler);
                        SetCurrentSelectedNode(TENode);
                    }
                } else if (node.Parent is StaticEffectsTreeNode staticEffectsNode) {
                    EditionTableNode.EditionTable.AssignedStaticEffects.RemoveAll(AE => AE.Effect == node.Effect);
                    Handler.RemoveEffects(new List<IEffect> { node.Effect }, null, node.GetTableType());
                    if (!silent) {
                        staticEffectsNode.Rebuild(Handler);
                        SetCurrentSelectedNode(staticEffectsNode);
                    }
                }
            }
        }

        private void DeleteTableElementNode(TableElementTreeNode node)
        {
            if (MessageBox.Show($"Do you want to delete {node.TE.Name} and all its effects ?", "Delete Table Element", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                foreach (var effNode in node.Nodes) {
                    DeleteEffectNode(effNode as EffectTreeNode, true);
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

        private void treeViewEditionTable_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SetCurrentSelectedNode(treeViewEditionTable.SelectedNode);
        }

        private void treeViewEditionTable_MouseDown(object sender, MouseEventArgs e)
        {

        }

        #endregion


        #region Dof Table Effects
        private void RomNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var combo = (sender as ComboBox);
            Settings.LastRomName = combo.Text;
            treeViewTableEffects.Nodes.Clear();
            treeViewTableEffects.Refresh();

            PopulateReferenceTableElements();
        }

        private void PopulateReferenceTableElements()
        {
            //Resetup the Reference Table
            Handler.SetupTable(DirectOutputToolkitHandler.ETableType.ReferenceTable, Settings.LastRomName);

            var table = Handler.GetTable(DirectOutputToolkitHandler.ETableType.ReferenceTable);

            treeViewTableEffects.Nodes.Clear();

            var staticEffectsNode = new StaticEffectsTreeNode(table, DirectOutputToolkitHandler.ETableType.ReferenceTable);
            staticEffectsNode.Rebuild(Handler);
            treeViewTableEffects.Nodes.Add(staticEffectsNode);

            foreach (TableElement tableElement in table.TableElements) {
                var elementName = tableElement.Name.IsNullOrEmpty() ? $"{tableElement.TableElementType}[{tableElement.Number}]" : tableElement.Name;
                var listNode = new TableElementTreeNode(tableElement, DirectOutputToolkitHandler.ETableType.ReferenceTable);

                foreach (var effect in tableElement.AssignedEffects) {
                    var effectNode = new EffectTreeNode(tableElement, DirectOutputToolkitHandler.ETableType.ReferenceTable, effect.Effect, Handler);
                    listNode.Nodes.Add(effectNode);
                }

                listNode.Refresh();
                treeViewTableEffects.Nodes.Add(listNode);
            }

            Handler.LaunchTable(DirectOutputToolkitHandler.ETableType.ReferenceTable);
            PreviewForm.Invalidate();
        }

        private void buttonActivationTable_Click(object sender, EventArgs e)
        {
            if (treeViewTableEffects.SelectedNode is ITableElementTreeNode nodeWithTE) {
                var value = Handler.SwitchTableElement(treeViewTableEffects.SelectedNode);
                SetEffectTreeNodeActive(treeViewTableEffects.SelectedNode, value > 0 ? 1 : 0);
                UpdateActivationButton(buttonActivationTable, value);
            }
        }

        private void buttonPulseTable_Click(object sender, EventArgs e)
        {
            if (treeViewTableEffects.SelectedNode is ITableElementTreeNode nodeWithTE) {
                var value = Handler.SwitchTableElement(treeViewTableEffects.SelectedNode);
                SetEffectTreeNodeActive(treeViewTableEffects.SelectedNode, value > 0 ? 1 : 0);
                UpdatePulseButton(buttonPulseTable, value);
                Thread.Sleep(Settings.PulseDurationMs);
                value = Handler.SwitchTableElement(treeViewTableEffects.SelectedNode);
                SetEffectTreeNodeActive(treeViewTableEffects.SelectedNode, value > 0 ? 1 : 0);
                UpdatePulseButton(buttonPulseTable, value);
            }
        }

        private void treeViewTableLedEffects_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.None) {
                if (e.Button == MouseButtons.Right) {
                    ShowContextMenu(e);
                }
            }
        }

        private void treeViewTableLedEffects_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SetCurrentSelectedNode(treeViewTableEffects.SelectedNode);
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
            propertyGridMain.Refresh();
            if (treeViewEditionTable.SelectedNode is EditionTableTreeNode editionTableNode) {
                if (editionTableNode.EditionTable == TD.EditionTable) {
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
            TD.Refresh();
            propertyGridMain.Refresh();
            if (treeViewEditionTable.SelectedNode is TableElementTreeNode editionTETreeNode) {
                if (editionTETreeNode.TE == TE) {
                    Handler.RemoveTableElement(TE, DirectOutputToolkitHandler.ETableType.EditionTable);
                    editionTETreeNode.Rebuild(Handler);
                    Handler.AddTableElement(TE, DirectOutputToolkitHandler.ETableType.EditionTable);
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
                    if (e.ChangedItem.Value == TCS.ColorConfig) {
                        TCS.ColorName = TCS.ColorConfig.Name;
                    } else if (e.ChangedItem.Value == TCS.ColorConfig2) {
                        TCS.ColorName2 = TCS.ColorConfig2.Name;
                    }
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
                    treeViewTableEffects.SelectedNode = node;
                    treeViewTableEffects.Refresh();
                }
            } else {
                treeViewEditionTable.SelectedNode = node;
                treeViewEditionTable.Refresh();
            }

            //Update property grid
            if (node is EffectTreeNode effectNode) {
                propertyGridMain.SelectedObject = new TableConfigSettingTypeDescriptor(effectNode, node.TreeView == treeViewEditionTable, Handler);
            } else if (node is TableElementTreeNode TENode) {
                propertyGridMain.SelectedObject = new TableElementTypeDescriptor(TENode.TE, node.TreeView == treeViewEditionTable);
            } else if (node is EditionTableTreeNode editionTableNode) {
                propertyGridMain.SelectedObject = new EditionTableTypeDescriptor(editionTableNode.EditionTable, node.TreeView == treeViewEditionTable);
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
            but.Text = value > 0 ? "Dectivate" : "Activate";
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
            foreach (var node in treeViewTableEffects.Nodes) {
                ResetTreeNodeRecursive(node as TreeNode);
            }
            ResetTreeNodeRecursive(EditionTableNode);
        }

        private static string NewTableElementName = "Table_Element_";

        private void OnCopyEffectToEditor(object sender, EventArgs e)
        {
            var item = (sender as MenuItem);
            var command = (item.Tag as TreeNodeCommand);
            var SrcEffectNode = (command.Sender as EffectTreeNode);
            var TENode = (command.Target as TableElementTreeNode);
            var StaticNode = (command.Target as StaticEffectsTreeNode);
            var parentTE = TENode?.TE ?? null;
            var EditionTable = Handler.GetTable(DirectOutputToolkitHandler.ETableType.EditionTable);

            if (TENode == null && StaticNode == null) {
                parentTE = new TableElement() { TableElementType = TableElementTypeEnum.NamedElement, Name = $"{NewTableElementName}{EditionTable.TableElements.Count}" };
                parentTE.AssignedEffects = new AssignedEffectList();
                EditionTable.TableElements.Add(parentTE);
                TENode = new TableElementTreeNode(parentTE, DirectOutputToolkitHandler.ETableType.EditionTable);
                EditionTableNode.Nodes.Add(TENode);
                EditionTableNode.Refresh();
            }

            if (TENode != null) {
                var newEffectNode = new EffectTreeNode(parentTE, DirectOutputToolkitHandler.ETableType.EditionTable, SrcEffectNode.Effect, Handler);
                newEffectNode.Rebuild(Handler, SrcEffectNode.Effect);
                parentTE.AssignedEffects.Init(EditionTable);
                TENode.Nodes.Add(newEffectNode);
                TENode.Rebuild(Handler);
                SetCurrentSelectedNode(TENode);
            } else {
                var newEffectNode = new EffectTreeNode(null, DirectOutputToolkitHandler.ETableType.EditionTable, SrcEffectNode.Effect, Handler);
                newEffectNode.Rebuild(Handler, SrcEffectNode.Effect);
                EditionTable.AssignedStaticEffects.Init(EditionTable);
                EditionTableNode.StaticEffectsNode.Nodes.Add(newEffectNode);
                EditionTableNode.StaticEffectsNode.Rebuild(Handler);
                SetCurrentSelectedNode(EditionTableNode.StaticEffectsNode);
            }
        }

        private void OnCopyTableElementToEditor(object sender, EventArgs e)
        {
            var item = (sender as MenuItem);
            var command = (item.Tag as TreeNodeCommand);
            var SrcTENode = (command.Sender as TableElementTreeNode);
            var TargetTENode = (command.Target as TableElementTreeNode);
            var StaticNode = (command.Target as StaticEffectsTreeNode);
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
                SetCurrentSelectedNode(EditionTableNode.StaticEffectsNode);
            }
        }

        private void OnInsertTableElementToEditor(object sender, EventArgs e)
        {
            var item = (sender as MenuItem);
            var command = (item.Tag as TreeNodeCommand);
            var SrcTENode = (command.Sender as TableElementTreeNode);
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

        public void CreateEffectsFromDofCommand(EditionTableTreeNode TableNode, int TCCNumber, string DofCommand, string ToyName, DirectOutputToolkitHandler Handler)
        {
            TableConfigSetting TCS = new TableConfigSetting();
            TCS.ParseSettingData(DofCommand);
            TCS.ResolveColorConfigs(Handler.ColorConfigurations);

            int LastEffectsCount = EditionTableNode.EditionTable.Effects.Count;

            var Toy = Handler.Toys.FirstOrDefault(T => T.Name.Equals(ToyName, StringComparison.InvariantCultureIgnoreCase));

            var newEffect = Handler.RebuildConfigurator.CreateEffect(TCS, TCCNumber, TCCNumber, TableNode.EditionTable
                                                                        , Toy
                                                                        , Handler.GetToyLedwizNum(ToyName)
                                                                        , Handler.InitFilesPath
                                                                        , TableNode.EditionTable.RomName);

            for (var num = LastEffectsCount; num < EditionTableNode.EditionTable.Effects.Count; ++num) {
                EditionTableNode.EditionTable.Effects[num].Init(EditionTableNode.EditionTable);
            }

            foreach (var te in TableNode.EditionTable.TableElements) {
                te.AssignedEffects.Init(TableNode.EditionTable);
                var teNode = TableNode.Nodes.OfType<TableElementTreeNode>().Cast<TableElementTreeNode>().FirstOrDefault(N => N.TE == te);
                if (teNode == null) {
                    teNode = new TableElementTreeNode(te, DirectOutputToolkitHandler.ETableType.EditionTable);
                    TableNode.Nodes.Add(teNode);
                }
                if (teNode.TE.AssignedEffects.Any(AE => AE.Effect == newEffect)) {
                    teNode.Rebuild(Handler);
                }
            }
        }


        private void ShowContextMenu(TreeNodeMouseClickEventArgs e)
        {
            if (e.Node is EffectTreeNode effectNode) {
                ContextMenu effectMenu = new ContextMenu();

                var addMenu = new MenuItem("Add effect to");
                effectMenu.MenuItems.Add(addMenu);

                addMenu.MenuItems.Add(new MenuItem("New Table Element", new EventHandler(this.OnCopyEffectToEditor)) { Tag = new TreeNodeCommand() { Sender = e.Node, Target = null } });
                foreach (var node in EditionTableNode.Nodes) {
                    addMenu.MenuItems.Add(new MenuItem($"{(node as TreeNode).Text}", new EventHandler(this.OnCopyEffectToEditor)) { Tag = new TreeNodeCommand() { Sender = e.Node, Target = (node as TreeNode) } });
                }

                effectMenu.Show(effectNode.GetTableType() == DirectOutputToolkitHandler.ETableType.EditionTable ? treeViewEditionTable : treeViewTableEffects, e.Location);
            } else if (e.Node is TableElementTreeNode tableElementNode) {
                ContextMenu teMenu = new ContextMenu();

                teMenu.MenuItems.Add(new MenuItem($"Add [{tableElementNode.ToString()}] to {Handler.GetTable(DirectOutputToolkitHandler.ETableType.EditionTable).TableName}", new EventHandler(this.OnCopyTableElementToEditor)) { Tag = new TreeNodeCommand() { Sender = e.Node, Target = null } });

                if (EditionTableNode.Nodes.Count > 0) {
                    var insertMenu = new MenuItem($"Copy [{tableElementNode.ToString()}] into");
                    teMenu.MenuItems.Add(insertMenu);
                    foreach (var node in EditionTableNode.Nodes) {
                        insertMenu.MenuItems.Add(new MenuItem($"{(node as TreeNode).Text}", new EventHandler(this.OnCopyTableElementToEditor)) { Tag = new TreeNodeCommand() { Sender = e.Node, Target = (node as TreeNode) } });
                    }

                    insertMenu = new MenuItem($"Insert [{tableElementNode.ToString()}] before");
                    teMenu.MenuItems.Add(insertMenu);
                    foreach (var node in EditionTableNode.Nodes) {
                        if (node is TableElementTreeNode) {
                            insertMenu.MenuItems.Add(new MenuItem($"{(node as TreeNode).Text}", new EventHandler(this.OnInsertTableElementToEditor)) { Tag = new TreeNodeCommand() { Sender = e.Node, Target = (node as TreeNode) } });
                        }
                    }
                }
                teMenu.Show(tableElementNode.GetTableType() == DirectOutputToolkitHandler.ETableType.EditionTable ? treeViewEditionTable : treeViewTableEffects, e.Location);
            }
        }
        #endregion
    }
}
