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

        TreeNode CurrentTableSelectedNode = null;

        EditionTableTreeNode EditionTableNode;
        TreeNode CurrentEditionSelectedNode = null;

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

        private void LedControlToolkit_FormClosing(object sender, FormClosingEventArgs e)
        {
            Handler.Finish();

            Settings.PulseDurationMs = (int)numericUpDownPulseDuration.Value;
            Settings.ShowMatrixGrid = panelPreviewLedMatrix.ShowMatrixGrid;
            Settings.ShowPreviewAreas = panelPreviewLedMatrix.ShowPreviewAreas;

            Settings.SaveSettings();
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

                EditionTableNode = new EditionTableTreeNode(Handler.EditionTable);

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
        private void treeViewEffect_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left) {

                CurrentTableSelectedNode = e.Node;
                Handler.SetCurrentTableElement(e.Node, true);
                var value = 0;
                if (e.Node is EffectTreeNode effectNode) {
                    propertyGridEffect.SelectedObject = new TableConfigSettingTypeDescriptor(effectNode.TCS, true);
                    value = Handler.GetCurrentTableElementValue();
                } else if (e.Node is TableElementTreeNode tableElementNode) {
                    propertyGridEffect.SelectedObject = new TableElementTypeDescriptor(tableElementNode.TE, true);
                    value = Handler.GetCurrentTableElementValue();
                } else if (e.Node is EditionTableTreeNode editionTableNode ){
                    propertyGridEffect.SelectedObject = new EditionTableTypeDescriptor(editionTableNode.EditionTable, true);
                } else {
                    propertyGridEffect.SelectedObject = null;
                }

                UpdateActivationButton(buttonActivationEdition, value);
                UpdatePulseButton(buttonPulseEdition, value);
            }
        }

        private void buttonActivationEdition_Click(object sender, EventArgs e)
        {
            if (CurrentTableSelectedNode != null) {
                var value = Handler.SwitchTableElement(CurrentTableSelectedNode);
                SetEffectTreeNodeActive(CurrentTableSelectedNode, value > 0 ? 1 : 0);
                UpdateActivationButton(buttonActivationEdition, value);
            }
        }

        private void buttonPulseEdition_Click(object sender, EventArgs e)
        {
            if (CurrentTableSelectedNode != null) {
                var value = Handler.SwitchTableElement(CurrentTableSelectedNode);
                SetEffectTreeNodeActive(CurrentTableSelectedNode, value > 0 ? 1 : 0);
                UpdatePulseButton(buttonPulseEdition, value);
                Thread.Sleep((int)numericUpDownPulseDuration.Value);
                value = Handler.SwitchTableElement(CurrentTableSelectedNode);
                SetEffectTreeNodeActive(CurrentTableSelectedNode, value > 0 ? 1 : 0);
                UpdatePulseButton(buttonPulseEdition, value);
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
                var listNode = new TableElementTreeNode(tableElement);

                foreach (var effect in tableElement.AssignedEffects) {
                    var effectTypeName = effect.Effect.GetType().ToString();
                    var effectNode = new EffectTreeNode(tableElement, effect.Effect, Handler.LedControlConfigData);
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
            if (CurrentTableSelectedNode != null) {
                var value = Handler.SwitchTableElement(CurrentTableSelectedNode);
                SetEffectTreeNodeActive(CurrentTableSelectedNode, value > 0 ? 1 : 0);
                UpdateActivationButton(buttonActivationTable, value);
            }
        }

        private void buttonPulseTable_Click(object sender, EventArgs e)
        {
            if (CurrentTableSelectedNode != null) {
                var value = Handler.SwitchTableElement(CurrentTableSelectedNode);
                SetEffectTreeNodeActive(CurrentTableSelectedNode, value > 0 ? 1 : 0);
                UpdatePulseButton(buttonPulseTable, value);
                Thread.Sleep((int)numericUpDownPulseDuration.Value);
                value = Handler.SwitchTableElement(CurrentTableSelectedNode);
                SetEffectTreeNodeActive(CurrentTableSelectedNode, value > 0 ? 1 : 0);
                UpdatePulseButton(buttonPulseTable, value);
            }
        }

        private void OnCopyEffectToEditor(object sender, EventArgs e)
        {
            var item = (sender as MenuItem);
            var command = (item.Tag as TreeNodeCommand);
            var effectNode = (command.Sender as EffectTreeNode);
            var TENode = (command.Target as TableElementTreeNode);
            var parentTE = TENode?.TE ?? null;

            if (TENode == null) {
                parentTE = new TableElement() { TableElementType = TableElementTypeEnum.NamedElement, Name = $"Table Element #{Handler.EditionTable.TableElements.Count}"};
                parentTE.AssignedEffects = new AssignedEffectList();
                Handler.EditionTable.TableElements.Add(parentTE);
                TENode = new TableElementTreeNode(parentTE);
                EditionTableNode.Nodes.Add(TENode);
            } 

            var newEffectNode = new EffectTreeNode(parentTE, effectNode.Effect, Handler.LedControlConfigData);
            Handler.SetCurrentTableElement(newEffectNode, true);
            newEffectNode.Rebuild(Handler);
            parentTE.AssignedEffects.Add(newEffectNode.Effect.Name);
            parentTE.AssignedEffects.Init(Handler.EditionTable);
            TENode.Nodes.Add(newEffectNode);
            TENode.Rebuild(Handler);
            treeViewEffect.SelectedNode = newEffectNode;
            treeViewEffect.Refresh();
        }

        private void OnCopyTableElementToEditor(object sender, EventArgs e)
        {
            var item = (sender as MenuItem);
            var command = (item.Tag as TreeNodeCommand);
            var SrcTENode = (command.Sender as TableElementTreeNode);
            var TargetTENode = (command.Target as TableElementTreeNode);
            var parentTE = TargetTENode?.TE ?? null;

            if (TargetTENode == null) {
                parentTE = new TableElement() { TableElementType = TableElementTypeEnum.NamedElement, Name = $"Table Element #{Handler.EditionTable.TableElements.Count}" };
                parentTE.AssignedEffects = new AssignedEffectList();
                Handler.EditionTable.TableElements.Add(parentTE);
                TargetTENode = new TableElementTreeNode(parentTE);
                EditionTableNode.Nodes.Add(TargetTENode);
            }

            foreach (var node in SrcTENode.Nodes) {
                var effectNode = node as EffectTreeNode;
                var newEffectNode = new EffectTreeNode(parentTE, effectNode.Effect, Handler.LedControlConfigData);
                Handler.SetCurrentTableElement(newEffectNode, true);
                newEffectNode.Rebuild(Handler);
                parentTE.AssignedEffects.Add(newEffectNode.Effect.Name);
                TargetTENode.Nodes.Add(newEffectNode);
            }
            parentTE.AssignedEffects.Init(Handler.EditionTable);
            TargetTENode.Rebuild(Handler);
            Handler.SetCurrentTableElement(TargetTENode, true);
            treeViewEffect.SelectedNode = TargetTENode;
            treeViewEffect.Refresh();
        }

        private void OnInsertTableElementToEditor(object sender, EventArgs e)
        {
            var item = (sender as MenuItem);
            var command = (item.Tag as TreeNodeCommand);
            var SrcTENode = (command.Sender as TableElementTreeNode);
            var TargetTENode = (command.Target as TableElementTreeNode);

            var parentTE = new TableElement() { TableElementType = TableElementTypeEnum.NamedElement, Name = $"Table Element #{Handler.EditionTable.TableElements.Count}" };
            parentTE.AssignedEffects = new AssignedEffectList();
            Handler.EditionTable.TableElements.Add(parentTE);
            var index = EditionTableNode.Nodes.IndexOf(TargetTENode);
            var newTENode = new TableElementTreeNode(parentTE);
            EditionTableNode.Nodes.Insert(index, newTENode);

            foreach (var node in SrcTENode.Nodes) {
                var effectNode = node as EffectTreeNode;
                var newEffectNode = new EffectTreeNode(parentTE, effectNode.Effect, Handler.LedControlConfigData);
                Handler.SetCurrentTableElement(newEffectNode, true);
                newEffectNode.Rebuild(Handler);
                newTENode.Nodes.Add(newEffectNode);
            }
            parentTE.AssignedEffects.Init(Handler.EditionTable);
            newTENode.Rebuild(Handler);
            Handler.SetCurrentTableElement(newTENode, true);
            treeViewEffect.SelectedNode = TargetTENode;
            treeViewEffect.Refresh();
        }

        private void treeViewTableLedEffects_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.None) {
                CurrentTableSelectedNode = e.Node;
                Handler.SetCurrentTableElement(e.Node, false);

                var value = 0;
                if (e.Node is EffectTreeNode effectNode) {
                    propertyGridEffect.SelectedObject = new TableConfigSettingTypeDescriptor(effectNode.TCS, false);
                    value = Handler.GetCurrentTableElementValue();
                    //Contextmenu to copy effect to edition
                    if (e.Button == MouseButtons.Right) {
                        ContextMenu effectMenu = new ContextMenu();

                        var addMenu = new MenuItem("Add effect to");
                        effectMenu.MenuItems.Add(addMenu);

                        addMenu.MenuItems.Add(new MenuItem("<New Table Element>", new EventHandler(this.OnCopyEffectToEditor)) { Tag = new TreeNodeCommand() { Sender = e.Node, Target = null } });
                        foreach (var node in EditionTableNode.Nodes) {
                            addMenu.MenuItems.Add(new MenuItem($"{(node as TreeNode).Text}", new EventHandler(this.OnCopyEffectToEditor)) { Tag = new TreeNodeCommand() { Sender = e.Node, Target = (node as TreeNode) } });
                        }
                        effectMenu.Show(treeViewTableLedEffects, e.Location);
                    }
                } else if (e.Node is TableElementTreeNode tableElementNode) {
                    propertyGridEffect.SelectedObject = new TableElementTypeDescriptor(tableElementNode.TE, false);
                    value = Handler.GetCurrentTableElementValue();

                    //Contextmenu to copy table element to edition
                    if (e.Button == MouseButtons.Right) {
                        ContextMenu teMenu = new ContextMenu();

                        teMenu.MenuItems.Add(new MenuItem($"Add [{tableElementNode.ToString()}] to {Handler.EditionTable.TableName}", new EventHandler(this.OnCopyTableElementToEditor)) { Tag = new TreeNodeCommand() { Sender = e.Node, Target = null } });

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

                } else {
                    propertyGridEffect.SelectedObject = null;
                }

                UpdateActivationButton(buttonActivationTable, value);
                UpdatePulseButton(buttonPulseTable, value);

                if (e.Button == MouseButtons.Right) {
                    if (e.Node is EffectTreeNode) {
                    } else if (e.Node is TableElementTreeNode) {
                        ContextMenu effectMenu = new ContextMenu();
                        effectMenu.MenuItems.Add(new MenuItem($"Add {CurrentTableSelectedNode.Nodes.Count} Effect(s) to new editin table element", new EventHandler(this.OnCopyTableElementToEditor)));
                        effectMenu.Show(treeViewTableLedEffects, e.Location);
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
            Settings.SaveSettings();
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
                    Handler.EditionTable.TableElements.RemoveAll(TE);
                    editionTETreeNode.Rebuild(Handler);
                    Handler.EditionTable.TableElements.Add(TE);
                    treeViewEffect.Refresh();
                }
            }
        }

        private void OnTableConfigSettingChanged(TableConfigSettingTypeDescriptor TD, PropertyValueChangedEventArgs e)
        {
            var TCS = TD.WrappedTCS;
            TD.Refresh();
            propertyGridEffect.Refresh();
            if (treeViewTableLedEffects.SelectedNode is EffectTreeNode effectTreeNode) {
                if (effectTreeNode.TCS == TCS) {
                    effectTreeNode.Rebuild(Handler);
                    treeViewTableLedEffects.Refresh();
                }
            } else if (treeViewEffect.SelectedNode is EffectTreeNode editionEffectTreeNode) {
                if (editionEffectTreeNode.TCS == TCS) {
                    editionEffectTreeNode.Rebuild(Handler);
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
        #endregion
    }
}
