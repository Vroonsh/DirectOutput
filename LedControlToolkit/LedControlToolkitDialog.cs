using DirectOutput;
using DirectOutput.Cab.Toys.Hardware;
using DirectOutput.FX;
using DirectOutput.FX.MatrixFX;
using DirectOutput.General.Color;
using DirectOutput.GlobalConfiguration;
using DirectOutput.LedControl.Loader;
using DirectOutput.Table;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LedControlToolkit
{
    public partial class LedControlToolkitDialog : Form
    {
        private LedControlToolkitHandler Handler;

        private Settings Settings = new Settings();

        EditionTableTreeNode EditionTableNode;

        #region Main Dialog
        public LedControlToolkitDialog()
        {
            InitializeComponent();

            Settings = Settings.LoadSettings();
            Handler = new LedControlToolkitHandler(Settings, panelPreviewLedMatrix);

            treeViewTableLedEffects.ImageList = imageListIcons;
            treeViewTableLedEffects.FullRowSelect = true;
            treeViewTableLedEffects.HideSelection = false;
            treeViewEffect.ImageList = imageListIcons;
            treeViewEffect.FullRowSelect = true;
            treeViewEffect.HideSelection = false;

            Handler.Start();
        }

        private void SaveSettings()
        {
            Settings.PulseDurationMs = (int)numericUpDownPulseDuration.Value;
            Settings.ShowMatrixGrid = panelPreviewLedMatrix.ShowMatrixGrid;
            Settings.ShowPreviewAreas = panelPreviewLedMatrix.ShowPreviewAreas;
            Settings.SaveSettings();
        }

        private void LedControlToolkit_FormClosing(object sender, FormClosingEventArgs e)
        {
            Handler.Finish();

            SaveSettings();
        }

        public bool LoadConfig()
        {
            OpenConfigDialog OCD = new OpenConfigDialog(Settings);
            if (OCD.ShowDialog() == DialogResult.OK) {

                RomNameComboBox.Text = Settings.LastRomName;
                numericUpDownPulseDuration.Value = Settings.PulseDurationMs;
                checkBoxPreviewArea.Checked = Settings.ShowPreviewAreas;
                checkBoxPreviewGrid.Checked = Settings.ShowMatrixGrid;

                RomNameComboBox.Items.Clear();
                RomNameComboBox.Items.Add("");
                Handler.LoadLedControlConfigData();
                RomNameComboBox.Items.AddRange(Handler.LedControlConfigData?.TableConfigurations.Select(TC => TC.ShortRomName).ToArray());

                UpdatePreviewAreaListControl();

                SetupPinball();

                EditionTableNode = new EditionTableTreeNode(Handler.GetTable(LedControlToolkitHandler.ETableType.EditionTable));

                treeViewEffect.Nodes.Add(EditionTableNode);
                treeViewEffect.Refresh();

                CheckPreviewAreaMissmatch();
                return true;
            } else {
                return false;
            }
        }

        private void SetupPinball()
        {
            Handler.SetupPinball();
            PopulateTableElements();
        }

        private void LedControlToolkit_Load(object sender, EventArgs e)
        {
            if (!LoadConfig()) {
                this.Close();
            }
        }

        #endregion

        #region Effects Panel
        private void tabPageEffectEditor_Enter(object sender, EventArgs e)
        {
        }
        #endregion

        #region Effect Editor
        private void buttonNewEditionTable_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Do you really want to start a new Table ?\n" +
                                $"The table {Handler.GetTable(LedControlToolkitHandler.ETableType.EditionTable).TableName} will be deleted.",
                                "Create New Table",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning) == DialogResult.Yes) {
                Handler.ResetEditionTable();
                EditionTableNode.EditionTable = Handler.GetTable(LedControlToolkitHandler.ETableType.EditionTable);
                EditionTableNode.Rebuild(Handler);
                ResetAllTableElements();
                UpdateActivationButton(buttonActivationEdition, 0);
                UpdateActivationButton(buttonActivationTable, 0);
                UpdatePulseButton(buttonPulseEdition, 0);
                UpdatePulseButton(buttonPulseTable, 0);
                treeViewEffect.Refresh();
            }
        }

        private void treeViewEffect_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.None) {
                SetCurrentSelectedNode(e.Node);

                //Right mouse
            }
        }

        private void treeViewEffect_AfterSelect(object sender, TreeViewEventArgs e)
        {
        }

        private void buttonActivationEdition_Click(object sender, EventArgs e)
        {
            if (treeViewEffect.SelectedNode is ITableElementTreeNode) {
                var value = Handler.SwitchTableElement(treeViewEffect.SelectedNode);
                SetEffectTreeNodeActive(treeViewEffect.SelectedNode, value > 0 ? 1 : 0);
                UpdateActivationButton(buttonActivationEdition, value);
            }
        }

        private void buttonPulseEdition_Click(object sender, EventArgs e)
        {
            if (treeViewEffect.SelectedNode is ITableElementTreeNode) {
                var value = Handler.SwitchTableElement(treeViewEffect.SelectedNode);
                SetEffectTreeNodeActive(treeViewEffect.SelectedNode, value > 0 ? 1 : 0);
                UpdatePulseButton(buttonPulseEdition, value);
                Thread.Sleep((int)numericUpDownPulseDuration.Value);
                value = Handler.SwitchTableElement(treeViewEffect.SelectedNode);
                SetEffectTreeNodeActive(treeViewEffect.SelectedNode, value > 0 ? 1 : 0);
                UpdatePulseButton(buttonPulseEdition, value);
            }
        }

        private void DeleteEffectNode(EffectTreeNode node, bool silent = false)
        {
            if (silent || MessageBox.Show($"Do you want to delete effect {node.Text} from {(node.Parent as TableElementTreeNode)?.TE.Name} ?", "Delete Effect", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                var TENode = (node.Parent as TableElementTreeNode);
                if (TENode != null) {
                    TENode.TE.AssignedEffects.RemoveAll(AE => AE.Effect == node.Effect);
                    var Table = Handler.GetTable(node.GetTableType());
                    Table.Effects.RemoveAll(E => E == node.Effect);
                    if (!silent) {
                        TENode.Rebuild(Handler);
                        SetCurrentSelectedNode(TENode);
                    }
                }
            }
        }

        private void DeleteTableElementNode(TableElementTreeNode node)
        {
            if (MessageBox.Show($"Do you want to delete {node.TE.Name} and all its effects ?", "Delete Table Element", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                foreach(var effNode in node.Nodes) {
                    DeleteEffectNode(effNode as EffectTreeNode, true);
                }
                var Table = Handler.GetTable(node.GetTableType());
                Table.TableElements.RemoveAll(TE => TE == node.TE);
                EditionTableNode.Rebuild(Handler);
                SetCurrentSelectedNode(EditionTableNode);
            }
        }

        private void treeViewEffect_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete) {

                if (treeViewEffect.SelectedNode is EffectTreeNode effectNode) {
                    DeleteEffectNode(effectNode);
                } else if (treeViewEffect.SelectedNode is TableElementTreeNode tableElementNode) {
                    DeleteTableElementNode(tableElementNode);
                }

                e.Handled = true;
            }
        }
        #endregion

        #region Table Effects
        private void PopulateTableElements()
        {
            Handler.Pinball.Table.TableElements.Sort((TE1, TE2) => (TE1.TableElementType == TE2.TableElementType ? TE1.Number.CompareTo(TE2.Number) : TE1.TableElementType.CompareTo(TE2.TableElementType)));

            treeViewTableLedEffects.Nodes.Clear();
            var ledstrips = Handler.Pinball.Cabinet.Toys.Where(T => T is LedStrip).Select(T => (T as LedStrip).Name).ToArray();
            var effects = Handler.Pinball.Table.Effects.Where(E => E.ActOnAnyToys(ledstrips)).ToList();

            List<TableElement> assignedeffects = new List<TableElement>();
            foreach (TableElement TE in Handler.Pinball.Table.TableElements) {
                foreach (var effect in TE.AssignedEffects) {
                    if (effects.Contains(effect.Effect)) {
                        assignedeffects.Add(TE);
                        break;
                    }
                }
            }

            foreach (var tableElement in assignedeffects) {
                var elementName = tableElement.Name.IsNullOrEmpty() ? $"{tableElement.TableElementType}[{tableElement.Number}]" : tableElement.Name;
                var listNode = new TableElementTreeNode(tableElement, LedControlToolkitHandler.ETableType.ReferenceTable);

                foreach (var effect in tableElement.AssignedEffects) {
                    var effectNode = new EffectTreeNode(tableElement, LedControlToolkitHandler.ETableType.ReferenceTable, effect.Effect, Handler.LedControlConfigData);
                    listNode.Nodes.Add(effectNode);
                }

                listNode.Refresh();
                treeViewTableLedEffects.Nodes.Add(listNode);
            }
        }

        private void RomNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var combo = (sender as ComboBox);
            Settings.LastRomName = combo.Text;
            treeViewTableLedEffects.Nodes.Clear();
            treeViewTableLedEffects.Refresh();
            SetupPinball();
        }

        private void buttonActivationTable_Click(object sender, EventArgs e)
        {
            if (treeViewTableLedEffects.SelectedNode is ITableElementTreeNode nodeWithTE) {
                var value = Handler.SwitchTableElement(treeViewTableLedEffects.SelectedNode);
                SetEffectTreeNodeActive(treeViewTableLedEffects.SelectedNode, value > 0 ? 1 : 0);
                UpdateActivationButton(buttonActivationTable, value);
            }
        }

        private void buttonPulseTable_Click(object sender, EventArgs e)
        {
            if (treeViewTableLedEffects.SelectedNode is ITableElementTreeNode nodeWithTE) {
                var value = Handler.SwitchTableElement(treeViewTableLedEffects.SelectedNode);
                SetEffectTreeNodeActive(treeViewTableLedEffects.SelectedNode, value > 0 ? 1 : 0);
                UpdatePulseButton(buttonPulseTable, value);
                Thread.Sleep((int)numericUpDownPulseDuration.Value);
                value = Handler.SwitchTableElement(treeViewTableLedEffects.SelectedNode);
                SetEffectTreeNodeActive(treeViewTableLedEffects.SelectedNode, value > 0 ? 1 : 0);
                UpdatePulseButton(buttonPulseTable, value);
            }
        }

        private void OnCopyEffectToEditor(object sender, EventArgs e)
        {
            var item = (sender as MenuItem);
            var command = (item.Tag as TreeNodeCommand);
            var SrcEffectNode = (command.Sender as EffectTreeNode);
            var TENode = (command.Target as TableElementTreeNode);
            var parentTE = TENode?.TE ?? null;
            var EditionTable = Handler.GetTable(LedControlToolkitHandler.ETableType.EditionTable);

            if (TENode == null) {
                parentTE = new TableElement() { TableElementType = TableElementTypeEnum.NamedElement, Name = $"Table Element #{EditionTable.TableElements.Count}"};
                parentTE.AssignedEffects = new AssignedEffectList();
                Handler.GetTable(LedControlToolkitHandler.ETableType.EditionTable).TableElements.Add(parentTE);
                TENode = new TableElementTreeNode(parentTE, LedControlToolkitHandler.ETableType.EditionTable);
                EditionTableNode.Nodes.Add(TENode);
            } 

            var newEffectNode = new EffectTreeNode(parentTE, LedControlToolkitHandler.ETableType.EditionTable, SrcEffectNode.Effect, Handler.LedControlConfigData);
            newEffectNode.Rebuild(Handler, SrcEffectNode.Effect);
            parentTE.AssignedEffects.Init(Handler.GetTable(LedControlToolkitHandler.ETableType.EditionTable));
            TENode.Nodes.Add(newEffectNode);
            TENode.Rebuild(Handler);
            SetCurrentSelectedNode(TENode);
        }

        private void OnCopyTableElementToEditor(object sender, EventArgs e)
        {
            var item = (sender as MenuItem);
            var command = (item.Tag as TreeNodeCommand);
            var SrcTENode = (command.Sender as TableElementTreeNode);
            var TargetTENode = (command.Target as TableElementTreeNode);
            var parentTE = TargetTENode?.TE ?? null;
            var EditionTable = Handler.GetTable(LedControlToolkitHandler.ETableType.EditionTable);

            if (TargetTENode == null) {
                parentTE = new TableElement() { TableElementType = TableElementTypeEnum.NamedElement, Name = $"Table Element #{EditionTable.TableElements.Count}" };
                parentTE.AssignedEffects = new AssignedEffectList();
                EditionTable.TableElements.Add(parentTE);
                TargetTENode = new TableElementTreeNode(parentTE, LedControlToolkitHandler.ETableType.EditionTable);
                EditionTableNode.Nodes.Add(TargetTENode);
            }

            foreach (var node in SrcTENode.Nodes) {
                var effectNode = node as EffectTreeNode;
                var newEffectNode = new EffectTreeNode(parentTE, LedControlToolkitHandler.ETableType.EditionTable, effectNode.Effect, Handler.LedControlConfigData);
                newEffectNode.Rebuild(Handler, effectNode.Effect);
            }
            parentTE.AssignedEffects.Init(EditionTable);
            TargetTENode.Rebuild(Handler);
            SetCurrentSelectedNode(TargetTENode);
        }

        private void OnInsertTableElementToEditor(object sender, EventArgs e)
        {
            var item = (sender as MenuItem);
            var command = (item.Tag as TreeNodeCommand);
            var SrcTENode = (command.Sender as TableElementTreeNode);
            var TargetTENode = (command.Target as TableElementTreeNode);

            var parentTE = new TableElement() { TableElementType = TableElementTypeEnum.NamedElement, Name = $"Table Element #{Handler.GetTable(LedControlToolkitHandler.ETableType.EditionTable).TableElements.Count}" };
            parentTE.AssignedEffects = new AssignedEffectList();
            Handler.GetTable(LedControlToolkitHandler.ETableType.EditionTable).TableElements.Add(parentTE);
            var index = EditionTableNode.Nodes.IndexOf(TargetTENode);
            var newTENode = new TableElementTreeNode(parentTE, LedControlToolkitHandler.ETableType.EditionTable);
            EditionTableNode.Nodes.Insert(index, newTENode);

            foreach (var node in SrcTENode.Nodes) {
                var effectNode = node as EffectTreeNode;
                parentTE.AssignedEffects.Add(effectNode.Effect.Name);
            }
            parentTE.AssignedEffects.Init(Handler.GetTable(LedControlToolkitHandler.ETableType.EditionTable));
            newTENode.Rebuild(Handler);
            SetCurrentSelectedNode(newTENode);
        }

        private void treeViewTableLedEffects_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.None) {
                SetCurrentSelectedNode(e.Node);

                if (e.Button == MouseButtons.Right) {
                    if (e.Node is EffectTreeNode effectNode) {
                        ContextMenu effectMenu = new ContextMenu();

                        var addMenu = new MenuItem("Add effect to");
                        effectMenu.MenuItems.Add(addMenu);

                        addMenu.MenuItems.Add(new MenuItem("<New Table Element>", new EventHandler(this.OnCopyEffectToEditor)) { Tag = new TreeNodeCommand() { Sender = e.Node, Target = null } });
                        foreach (var node in EditionTableNode.Nodes) {
                            addMenu.MenuItems.Add(new MenuItem($"{(node as TreeNode).Text}", new EventHandler(this.OnCopyEffectToEditor)) { Tag = new TreeNodeCommand() { Sender = e.Node, Target = (node as TreeNode) } });
                        }
                        effectMenu.Show(treeViewTableLedEffects, e.Location);
                    } else if (e.Node is TableElementTreeNode tableElementNode) {
                        ContextMenu teMenu = new ContextMenu();

                        teMenu.MenuItems.Add(new MenuItem($"Add [{tableElementNode.ToString()}] to {Handler.GetTable(LedControlToolkitHandler.ETableType.EditionTable).TableName}", new EventHandler(this.OnCopyTableElementToEditor)) { Tag = new TreeNodeCommand() { Sender = e.Node, Target = null } });

                        if (EditionTableNode.Nodes.Count > 0) {
                            var insertMenu = new MenuItem($"Copy [{tableElementNode.ToString()}] into");
                            teMenu.MenuItems.Add(insertMenu);
                            foreach (var node in EditionTableNode.Nodes) {
                                insertMenu.MenuItems.Add(new MenuItem($"{(node as TreeNode).Text}", new EventHandler(this.OnCopyTableElementToEditor)) { Tag = new TreeNodeCommand() { Sender = e.Node, Target = (node as TreeNode) } });
                            }

                            insertMenu = new MenuItem($"Insert [{tableElementNode.ToString()}] before");
                            teMenu.MenuItems.Add(insertMenu);
                            foreach (var node in EditionTableNode.Nodes) {
                                insertMenu.MenuItems.Add(new MenuItem($"{(node as TreeNode).Text}", new EventHandler(this.OnInsertTableElementToEditor)) { Tag = new TreeNodeCommand() { Sender = e.Node, Target = (node as TreeNode) } });
                            }
                        }
                        teMenu.Show(treeViewTableLedEffects, e.Location);
                    } 
                }
            }
        }

        #endregion


        #region Settings Editor
        private void tabPageSettings_Enter(object sender, EventArgs e)
        {
            propertyGridEffect.SelectedObject = listBoxPreviewAreas.SelectedItem;
        }

        private string[] GetMissingLedStrips()
        {
            var ledstrips = Handler.GetLedstrips();
            List<string> missingLedstrip = new List<string>();
            foreach (var ledstrip in ledstrips) {
                if (!Settings.LedPreviewAreas.Any(A => A.Name.Equals(ledstrip.Name, StringComparison.InvariantCultureIgnoreCase))) {
                    missingLedstrip.Add(ledstrip.Name);
                }
            }
            return missingLedstrip.ToArray();
        }

        private void CheckPreviewAreaMissmatch()
        {
            var missingLedstrip = GetMissingLedStrips();

            if (missingLedstrip.Length != 0) {
                if (MessageBox.Show("There are missing ledstrips from cabinet in preview areas settings.\n" +
                                $"Missing ledstrips :\n\t{string.Join(", ", missingLedstrip)}\n\n" +
                                $"Do you want to update your preview areas settings now ?",
                                "Preview areas missmatch", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) {
                    tabControlMain.SelectedIndex = 1;
                }
            }
        }

        private void checkBoxPreviewGrid_CheckedChanged(object sender, EventArgs e)
        {
            panelPreviewLedMatrix.ShowMatrixGrid = (sender as CheckBox).Checked;
            panelPreviewLedMatrix.Refresh();
        }

        private void checkBoxPreviewArea_CheckedChanged(object sender, EventArgs e)
        {
            panelPreviewLedMatrix.ShowPreviewAreas = (sender as CheckBox).Checked;
            panelPreviewLedMatrix.Refresh();
        }

        private void listBoxPreviewAreas_SelectedIndexChanged(object sender, EventArgs e)
        {
            var listCtrl = (sender as ListControl);
            if (listCtrl.SelectedIndex != -1) {
                propertyGridEffect.SelectedObject = listBoxPreviewAreas.SelectedItem;
            }
        }

        private void UpdatePreviewAreaListControl()
        {
            listBoxPreviewAreas.DataSource = null;
            listBoxPreviewAreas.ValueMember = "Id";
            listBoxPreviewAreas.DisplayMember = "Name";
            listBoxPreviewAreas.DataSource = Settings.LedPreviewAreas;
        }

        private string GetNewAreaName()
        {
            var newAreaNum = 0;
            string newAreaName = $"New Area {newAreaNum}";
            while (Settings.LedPreviewAreas.Any(A => A.Name.Equals(newAreaName, StringComparison.InvariantCultureIgnoreCase))) {
                newAreaNum++;
                newAreaName = $"New Area {newAreaNum}";
            }
            return newAreaName;
        }

        private void buttonCreateMissingAreas_Click(object sender, EventArgs e)
        {
            var missingLedstrips = GetMissingLedStrips();
            foreach (var ledstrip in missingLedstrips) {
                Settings.LedPreviewAreas.Add(new Settings.LedPreviewArea() { Name = ledstrip });
            }
            UpdatePreviewAreaListControl();
            listBoxPreviewAreas.SelectedIndex = listBoxPreviewAreas.Items.Count - 1;
            panelPreviewLedMatrix.SetupPreviewParts(Handler.Pinball.Cabinet, Settings);
            panelPreviewLedMatrix.Refresh();
        }

        private void buttonNewArea_Click(object sender, EventArgs e)
        {
            Settings.LedPreviewAreas.Add(new Settings.LedPreviewArea() { Name = GetNewAreaName() });
            UpdatePreviewAreaListControl();
            listBoxPreviewAreas.SelectedIndex = listBoxPreviewAreas.Items.Count - 1;
            panelPreviewLedMatrix.SetupPreviewParts(Handler.Pinball.Cabinet, Settings);
            panelPreviewLedMatrix.Refresh();
        }

        private void buttonDuplicateArea_Click(object sender, EventArgs e)
        {
            Settings.LedPreviewAreas.Add(new Settings.LedPreviewArea(listBoxPreviewAreas.SelectedItem as Settings.LedPreviewArea) { Name = GetNewAreaName() });
            UpdatePreviewAreaListControl();
            listBoxPreviewAreas.SelectedIndex = listBoxPreviewAreas.Items.Count - 1;
            panelPreviewLedMatrix.SetupPreviewParts(Handler.Pinball.Cabinet, Settings);
            panelPreviewLedMatrix.Refresh();
        }

        private void buttonDeleteArea_Click(object sender, EventArgs e)
        {
            var selectedArea = listBoxPreviewAreas.SelectedItem as Settings.LedPreviewArea;
            if (selectedArea != null && MessageBox.Show($"Do you really want to delete preview area {selectedArea.Name} ?", "Delete preview area", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                Settings.LedPreviewAreas.Remove(selectedArea);
                listBoxPreviewAreas.SelectedIndex = -1;
                UpdatePreviewAreaListControl();
                panelPreviewLedMatrix.SetupPreviewParts(Handler.Pinball.Cabinet, Settings);
                panelPreviewLedMatrix.Refresh();
            }
        }

        private void buttonSaveSettings_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        #endregion

        #region Property Grid
        private void propertyGridEffect_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            var propertyGrid = (s as PropertyGrid);
            if (propertyGrid.SelectedObject is Settings.LedPreviewArea selectedArea) {
                OnLedPreviewAreaChanged(selectedArea, e);
            } else if (propertyGrid.SelectedObject is TableConfigSettingTypeDescriptor TCSDesc) {
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
            propertyGridEffect.Refresh();
            if (treeViewEffect.SelectedNode is EditionTableTreeNode editionTableNode) {
                if (editionTableNode.EditionTable == TD.EditionTable) {
                    editionTableNode.Rebuild(Handler);
                    treeViewEffect.Refresh();
                }
            } 
        }

        private void OnTableElementChanged(TableElementTypeDescriptor TD, PropertyValueChangedEventArgs e)
        {
            var TE = TD.WrappedTE;
            TD.Refresh();
            propertyGridEffect.Refresh();
            if (treeViewEffect.SelectedNode is TableElementTreeNode editionTETreeNode) {
                if (editionTETreeNode.TE == TE) {
                    var EditionTable = Handler.GetTable(LedControlToolkitHandler.ETableType.EditionTable);
                    EditionTable.TableElements.RemoveAll(TE);
                    editionTETreeNode.Rebuild(Handler);
                    EditionTable.TableElements.Add(TE);
                    treeViewEffect.Refresh();
                }
            }
        }

        private void OnTableConfigSettingChanged(TableConfigSettingTypeDescriptor TD, PropertyValueChangedEventArgs e)
        {
            var TCS = TD.WrappedTCS;
            TD.Refresh();
            propertyGridEffect.Refresh();
            if (treeViewEffect.SelectedNode is EffectTreeNode editionEffectTreeNode) {
                if (editionEffectTreeNode.TCS == TCS) {
                    if (e.ChangedItem.Value == TCS.ColorConfig) {
                        TCS.ColorName = TCS.ColorConfig.Name;
                    }else if (e.ChangedItem.Value == TCS.ColorConfig2) {
                        TCS.ColorName2 = TCS.ColorConfig2.Name;
                    }
                    editionEffectTreeNode.Rebuild(Handler, null);
                    treeViewEffect.Refresh();
                }
            }
        }

        private void OnLedPreviewAreaChanged(Settings.LedPreviewArea selectedArea, PropertyValueChangedEventArgs e)
        {
            switch (e.ChangedItem.PropertyDescriptor.Name) {
                case "Name": {
                    //Check if there is no duplicate zone area names
                    foreach (var pArea in Settings.LedPreviewAreas) {
                        if (pArea != selectedArea && pArea.Name.Equals(selectedArea.Name, StringComparison.InvariantCultureIgnoreCase)) {
                            MessageBox.Show($"There is already another preview area named {selectedArea.Name}.\nPlease choose another name.", "Duplicate preview area names", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            selectedArea.Name = e.OldValue as string;
                            return;
                        }
                    }

                    UpdatePreviewAreaListControl();
                    break;
                }

                default: {
                    break;
                }
            }

            panelPreviewLedMatrix.SetupPreviewParts(Handler.Pinball.Cabinet, Settings);
            panelPreviewLedMatrix.Refresh();
        }

        #endregion

        #region Helpers
        private void SetCurrentSelectedNode(TreeNode node)
        {
            var value = 0;
            if (node is ITableElementTreeNode tableElementNode) {
                value = tableElementNode.GetTableElement().Value;
                if (tableElementNode.GetTableType() == LedControlToolkitHandler.ETableType.EditionTable) {
                    UpdateActivationButton(buttonActivationEdition, value);
                    UpdatePulseButton(buttonPulseEdition, value);
                    treeViewEffect.SelectedNode = node;
                    treeViewEffect.Refresh();
                } else {
                    UpdateActivationButton(buttonActivationTable, value);
                    UpdatePulseButton(buttonPulseTable, value);
                    treeViewTableLedEffects.SelectedNode = node;
                    treeViewTableLedEffects.Refresh();
                }
            } else {
                treeViewEffect.SelectedNode = node;
                treeViewEffect.Refresh();
            }

            //Update property grid
            if (node is EffectTreeNode effectNode) {
                propertyGridEffect.SelectedObject = new TableConfigSettingTypeDescriptor(effectNode, true, Handler);
                value = effectNode.GetTableElement().GetTableElementData().Value;
            } else if (node is TableElementTreeNode TENode) {
                propertyGridEffect.SelectedObject = new TableElementTypeDescriptor(TENode.TE, true);
            } else if (node is EditionTableTreeNode editionTableNode) {
                propertyGridEffect.SelectedObject = new EditionTableTypeDescriptor(editionTableNode.EditionTable, true);
            } else {
                propertyGridEffect.SelectedObject = null;
            }
            propertyGridEffect.Refresh();
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
            foreach(var node in treeViewTableLedEffects.Nodes) {
                ResetTreeNodeRecursive(node as TreeNode);
            }
            ResetTreeNodeRecursive(EditionTableNode);
        }
        #endregion
    }
}
