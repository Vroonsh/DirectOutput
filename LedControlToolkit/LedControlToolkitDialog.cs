using DirectOutput;
using DirectOutput.Cab.Toys.Hardware;
using DirectOutput.FX;
using DirectOutput.FX.MatrixFX;
using DirectOutput.General.Color;
using DirectOutput.GlobalConfiguration;
using DirectOutput.LedControl.Loader;
using DirectOutput.Table;
using DirectOutputControls;
using DofConfigToolWrapper;
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
        private Dictionary<string, string> ToyOuputMappings = new Dictionary<string, string>();

        EditionTableTreeNode EditionTableNode;

        LedControlToolkitEffectsDebugger EffectsDebuggerDialog;

        #region Main Dialog
        public LedControlToolkitDialog()
        {
            InitializeComponent();

            Settings = Settings.LoadSettings();
            numericUpDownPulseDuration.Value = Settings.PulseDurationMs;
            numericUpDownMinDuration.Value = Settings.EffectMinDurationMs;
            numericUpDownMatrixMinDuration.Value = Settings.EffectRGBMinDurationMs;
            checkBoxPreviewArea.Checked = Settings.ShowPreviewAreas;
            checkBoxPreviewGrid.Checked = Settings.ShowMatrixGrid;

            Handler = new LedControlToolkitHandler(Settings, panelPreviewLedMatrix);
            EffectsDebuggerDialog = new LedControlToolkitEffectsDebugger() { Handler = Handler };

            treeViewTableLedEffects.ImageList = imageListIcons;
            treeViewTableLedEffects.FullRowSelect = true;
            treeViewTableLedEffects.HideSelection = false;
            treeViewEditionTable.ImageList = imageListIcons;
            treeViewEditionTable.FullRowSelect = true;
            treeViewEditionTable.HideSelection = false;

            Handler.Start();
            UpdateToyOutputMappings();
        }

        private void SaveSettings()
        {
            Settings.PulseDurationMs = (int)numericUpDownPulseDuration.Value;
            Settings.EffectMinDurationMs = (int)numericUpDownMinDuration.Value;
            Settings.EffectRGBMinDurationMs = (int)numericUpDownMatrixMinDuration.Value;
            Settings.ShowMatrixGrid = panelPreviewLedMatrix.ShowMatrixGrid;
            Settings.ShowPreviewAreas = panelPreviewLedMatrix.ShowPreviewAreas;
            Settings.SaveSettings();
        }

        private void LedControlToolkit_FormClosing(object sender, FormClosingEventArgs e)
        {
            EffectsDebuggerDialog.Close();

            Handler.Finish();

            SaveSettings();
        }

        public bool LoadConfig()
        {
            OpenConfigDialog OCD = new OpenConfigDialog(Settings);
            if (OCD.ShowDialog() == DialogResult.OK) {

                RomNameComboBox.Text = Settings.LastRomName;

                RomNameComboBox.Items.Clear();
                RomNameComboBox.Items.Add("");
                Handler.LoadLedControlConfigData();
                RomNameComboBox.Items.AddRange(Handler.LedControlConfigData?.TableConfigurations.Select(TC => TC.ShortRomName).ToArray());

                UpdatePreviewAreaListControl();

                SetupPinball();

                EditionTableNode = new EditionTableTreeNode(Handler.GetTable(LedControlToolkitHandler.ETableType.EditionTable));

                treeViewEditionTable.Nodes.Add(EditionTableNode);
                treeViewEditionTable.Refresh();

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
        private void ResetEditionTable()
        {
            Handler.ResetEditionTable();
            EditionTableNode.EditionTable = Handler.GetTable(LedControlToolkitHandler.ETableType.EditionTable);
            EditionTableNode.Rebuild(Handler);
            ResetAllTableElements();
            UpdateActivationButton(buttonActivationEdition, 0);
            UpdateActivationButton(buttonActivationTable, 0);
            UpdatePulseButton(buttonPulseEdition, 0);
            UpdatePulseButton(buttonPulseTable, 0);
        }

        private void buttonNewEditionTable_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Do you really want to start a new Table ?\n" +
                                $"The table {Handler.GetTable(LedControlToolkitHandler.ETableType.EditionTable).TableName} will be deleted.",
                                "Create New Table",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning) == DialogResult.Yes) {
                ResetEditionTable();
                SetCurrentSelectedNode(EditionTableNode);
            }
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
            }
        }

        private void buttonPulseEdition_Click(object sender, EventArgs e)
        {
            if (treeViewEditionTable.SelectedNode is ITableElementTreeNode) {
                var value = Handler.SwitchTableElement(treeViewEditionTable.SelectedNode);
                SetEffectTreeNodeActive(treeViewEditionTable.SelectedNode, value > 0 ? 1 : 0);
                UpdatePulseButton(buttonPulseEdition, value);
                Thread.Sleep((int)numericUpDownPulseDuration.Value);
                value = Handler.SwitchTableElement(treeViewEditionTable.SelectedNode);
                SetEffectTreeNodeActive(treeViewEditionTable.SelectedNode, value > 0 ? 1 : 0);
                UpdatePulseButton(buttonPulseEdition, value);
            }
        }

        private void DeleteEffectNode(EffectTreeNode node, bool silent = false)
        {
            if (silent || MessageBox.Show($"Do you want to delete effect {node.Text} from {(node.Parent as TableElementTreeNode)?.TE.Name} ?", "Delete Effect", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                var TENode = (node.Parent as TableElementTreeNode);
                if (TENode != null) {
                    TENode.TE.AssignedEffects.RemoveAll(AE => AE.Effect == node.Effect);
                    Handler.RemoveEffects(new List<IEffect>{ node.Effect }, (node.Parent as TableElementTreeNode)?.TE, node.GetTableType());
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
                Handler.RemoveTableElement(node.GetTableElement(), node.GetTableType());
                EditionTableNode.Rebuild(Handler);
                SetCurrentSelectedNode(EditionTableNode);
            }
        }

        private void treeViewEffect_KeyDown(object sender, KeyEventArgs e)
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

        private void buttonLoadEffect_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog() {
                Filter = "LedControl Toolkit Files|*.lctk|LCTK XML Files|*.xml|All Files|*.*",
                DefaultExt = "lctk",
                Title = "Load Table Effects"
            };

            fd.ShowDialog();
            if (!fd.FileName.IsNullOrEmpty()) {
                ResetEditionTable();
                var serializer = new LedControlToolkitSerializer();
                if (serializer.Deserialize(EditionTableNode, fd.FileName, Handler, ToyOuputMappings)) {
                    SetCurrentSelectedNode(EditionTableNode);
                    //MessageBox.Show($"Table [{EditionTableNode.EditionTable.TableName}] loaded", "Load Table Effects", MessageBoxButtons.OK);
                }
            }
        }

        private void buttonSaveEffect_Click(object sender, EventArgs e)
        {
            SaveFileDialog fd = new SaveFileDialog() {
                Filter = "LedControl Toolkit Files|*.lctk|LCTK XML Files|*.xml|All Files|*.*",
                DefaultExt = "lctk",
                Title = "Save Table Effects"
            };

            fd.ShowDialog();

            if (!fd.FileName.IsNullOrEmpty()) {
                var serializer = new LedControlToolkitSerializer();
                if (serializer.Serialize(EditionTableNode, fd.FileName, ToyOuputMappings)) {
                    SetCurrentSelectedNode(EditionTableNode);
                    MessageBox.Show($"Table [{EditionTableNode.EditionTable.TableName}] descriptor\n" +
                                    $"is saved in {fd.FileName}.", "Save Table Effects", MessageBoxButtons.OK);
                }
            }
        }

        private void buttonImportDOF_Click(object sender, EventArgs e)
        {
            LedControlToolkitDOFCommandsDialog dlg = new LedControlToolkitDOFCommandsDialog() { AvailableToys = ToyOuputMappings.Select(M => M.Key).ToArray() };
            dlg.ShowDialog(this);

            if (dlg.CommandLines != null) {

                Dictionary<TableElement, List<TableConfigSetting>> TableConfigSettings = new Dictionary<TableElement, List<TableConfigSetting>>();

                int TCCNumber = EditionTableNode.EditionTable.TableElements.Count;
                foreach (var line in dlg.CommandLines) {
                    CreateEffectsFromDofCommand(EditionTableNode, TCCNumber, line, dlg.ToyName, Handler);
                    TCCNumber++;
                }
            }
            EditionTableNode.Rebuild(Handler);
        }

        private void buttonExportDOF_Click(object sender, EventArgs e)
        {
            LedControlToolkitDOFOutputs dlg = new LedControlToolkitDOFOutputs() { TableNode = EditionTableNode, OutputMappings = ToyOuputMappings, Handler = Handler };
            dlg.ShowDialog(this);
        }

        #endregion

        #region Table Effects
        private void PopulateTableElements()
        {
            Handler.Pinball.Table.TableElements.Sort((TE1, TE2) => (TE1.TableElementType == TE2.TableElementType ? TE1.Number.CompareTo(TE2.Number) : TE1.TableElementType.CompareTo(TE2.TableElementType)));

            treeViewTableLedEffects.Nodes.Clear();
            var ledstrips = Handler.Pinball.Cabinet.Toys.Where(T => T is LedStrip).Select(T => (T as LedStrip).Name).ToArray();
//            var effects = Handler.Pinball.Table.Effects.Where(E => E.ActOnAnyToys(ledstrips)).ToList();

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
            SetCurrentSelectedNode(treeViewTableLedEffects.SelectedNode);
        }

        #endregion

        #region Effect Debug
        private void checkBoxDebugEffects_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxDebugEffects.Checked) {
                EffectsDebuggerDialog.StartPosition = FormStartPosition.Manual;
                EffectsDebuggerDialog.DesktopLocation = new Point(this.Bounds.Right, this.Bounds.Top);
                EffectsDebuggerDialog.Show();
            } else {
                EffectsDebuggerDialog.Hide();
            }
        }
        #endregion

        #region Settings Editor
        private void UpdateToyOutputMappings()
        {
            ToyOuputMappings.Clear();
            foreach(var previewArea in Settings.LedPreviewAreas) {
                ToyOuputMappings[previewArea.Name] = previewArea.ConfigToolOutput.ToString();
            }
        }

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
            UpdateToyOutputMappings();
        }

        private void buttonNewArea_Click(object sender, EventArgs e)
        {
            Settings.LedPreviewAreas.Add(new Settings.LedPreviewArea() { Name = GetNewAreaName() });
            UpdatePreviewAreaListControl();
            listBoxPreviewAreas.SelectedIndex = listBoxPreviewAreas.Items.Count - 1;
            panelPreviewLedMatrix.SetupPreviewParts(Handler.Pinball.Cabinet, Settings);
            panelPreviewLedMatrix.Refresh();
            UpdateToyOutputMappings();
        }

        private void buttonDuplicateArea_Click(object sender, EventArgs e)
        {
            Settings.LedPreviewAreas.Add(new Settings.LedPreviewArea(listBoxPreviewAreas.SelectedItem as Settings.LedPreviewArea) { Name = GetNewAreaName() });
            UpdatePreviewAreaListControl();
            listBoxPreviewAreas.SelectedIndex = listBoxPreviewAreas.Items.Count - 1;
            panelPreviewLedMatrix.SetupPreviewParts(Handler.Pinball.Cabinet, Settings);
            panelPreviewLedMatrix.Refresh();
            UpdateToyOutputMappings();
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
                UpdateToyOutputMappings();
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
            EffectsDebuggerDialog.RebuildTreeView();
        }

        private void OnEditionTableChanged(EditionTableTypeDescriptor TD, PropertyValueChangedEventArgs e)
        {
            TD.Refresh();
            propertyGridEffect.Refresh();
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
            propertyGridEffect.Refresh();
            if (treeViewEditionTable.SelectedNode is TableElementTreeNode editionTETreeNode) {
                if (editionTETreeNode.TE == TE) {
                    Handler.RemoveTableElement(TE, LedControlToolkitHandler.ETableType.EditionTable);
                    editionTETreeNode.Rebuild(Handler);
                    Handler.AddTableElement(TE, LedControlToolkitHandler.ETableType.EditionTable);
                    treeViewEditionTable.Refresh();
                }
            }
        }

        private void OnTableConfigSettingChanged(TableConfigSettingTypeDescriptor TD, PropertyValueChangedEventArgs e)
        {
            var TCS = TD.WrappedTCS;
            TD.Refresh();
            propertyGridEffect.Refresh();
            if (treeViewEditionTable.SelectedNode is EffectTreeNode editionEffectTreeNode) {
                if (editionEffectTreeNode.TCS == TCS) {
                    if (e.ChangedItem.Value == TCS.ColorConfig) {
                        TCS.ColorName = TCS.ColorConfig.Name;
                    }else if (e.ChangedItem.Value == TCS.ColorConfig2) {
                        TCS.ColorName2 = TCS.ColorConfig2.Name;
                    }
                    editionEffectTreeNode.Rebuild(Handler, null);
                    treeViewEditionTable.Refresh();
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
            UpdateToyOutputMappings();
        }

        #endregion

        #region Preview
        private void panelPreviewLedMatrix_ControlRemoved(object sender, ControlEventArgs e)
        {
            panelPreviewLedMatrix.OnClose();
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
                    treeViewEditionTable.SelectedNode = node;
                    treeViewEditionTable.Refresh();
                } else {
                    UpdateActivationButton(buttonActivationTable, value);
                    UpdatePulseButton(buttonPulseTable, value);
                    treeViewTableLedEffects.SelectedNode = node;
                    treeViewTableLedEffects.Refresh();
                }
            } else {
                treeViewEditionTable.SelectedNode = node;
                treeViewEditionTable.Refresh();
            }

            //Update property grid
            if (node is EffectTreeNode effectNode) {
                propertyGridEffect.SelectedObject = new TableConfigSettingTypeDescriptor(effectNode, node.TreeView == treeViewEditionTable, Handler);
            } else if (node is TableElementTreeNode TENode) {
                propertyGridEffect.SelectedObject = new TableElementTypeDescriptor(TENode.TE, node.TreeView == treeViewEditionTable);
            } else if (node is EditionTableTreeNode editionTableNode) {
                propertyGridEffect.SelectedObject = new EditionTableTypeDescriptor(editionTableNode.EditionTable, node.TreeView == treeViewEditionTable);
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

        private static string NewTableElementName = "Table_Element_";

        private void OnCopyEffectToEditor(object sender, EventArgs e)
        {
            var item = (sender as MenuItem);
            var command = (item.Tag as TreeNodeCommand);
            var SrcEffectNode = (command.Sender as EffectTreeNode);
            var TENode = (command.Target as TableElementTreeNode);
            var parentTE = TENode?.TE ?? null;
            var EditionTable = Handler.GetTable(LedControlToolkitHandler.ETableType.EditionTable);

            if (TENode == null) {
                parentTE = new TableElement() { TableElementType = TableElementTypeEnum.NamedElement, Name = $"{NewTableElementName}{EditionTable.TableElements.Count}" };
                parentTE.AssignedEffects = new AssignedEffectList();
                Handler.GetTable(LedControlToolkitHandler.ETableType.EditionTable).TableElements.Add(parentTE);
                TENode = new TableElementTreeNode(parentTE, LedControlToolkitHandler.ETableType.EditionTable);
                EditionTableNode.Nodes.Add(TENode);
                EditionTableNode.Refresh();
            }

            var newEffectNode = new EffectTreeNode(parentTE, LedControlToolkitHandler.ETableType.EditionTable, SrcEffectNode.Effect, Handler.LedControlConfigData);
            newEffectNode.Rebuild(Handler, SrcEffectNode.Effect);
            parentTE.AssignedEffects.Init(Handler.GetTable(LedControlToolkitHandler.ETableType.EditionTable));
            TENode.Nodes.Add(newEffectNode);
            TENode.Rebuild(Handler);
            SetCurrentSelectedNode(TENode);
            EffectsDebuggerDialog.RebuildTreeView();
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
                parentTE = new TableElement() { TableElementType = TableElementTypeEnum.NamedElement, Name = $"{NewTableElementName}{EditionTable.TableElements.Count}" };
                parentTE.AssignedEffects = new AssignedEffectList();
                EditionTable.TableElements.Add(parentTE);
                TargetTENode = new TableElementTreeNode(parentTE, LedControlToolkitHandler.ETableType.EditionTable);
                EditionTableNode.Nodes.Add(TargetTENode);
                EditionTableNode.Refresh();
            }

            foreach (var node in SrcTENode.Nodes) {
                var effectNode = node as EffectTreeNode;
                var newEffectNode = new EffectTreeNode(parentTE, LedControlToolkitHandler.ETableType.EditionTable, effectNode.Effect, Handler.LedControlConfigData);
                newEffectNode.Rebuild(Handler, effectNode.Effect);
            }
            parentTE.AssignedEffects.Init(EditionTable);
            TargetTENode.Rebuild(Handler);
            SetCurrentSelectedNode(TargetTENode);
            EffectsDebuggerDialog.RebuildTreeView();
        }

        private void OnInsertTableElementToEditor(object sender, EventArgs e)
        {
            var item = (sender as MenuItem);
            var command = (item.Tag as TreeNodeCommand);
            var SrcTENode = (command.Sender as TableElementTreeNode);
            var TargetTENode = (command.Target as TableElementTreeNode);
            var EditionTable = Handler.GetTable(LedControlToolkitHandler.ETableType.EditionTable);

            var parentTE = new TableElement() { TableElementType = TableElementTypeEnum.NamedElement, Name = $"{NewTableElementName}{EditionTable.TableElements.Count}" };
            parentTE.AssignedEffects = new AssignedEffectList();
            EditionTable.TableElements.Add(parentTE);
            var index = EditionTableNode.Nodes.IndexOf(TargetTENode);
            var newTENode = new TableElementTreeNode(parentTE, LedControlToolkitHandler.ETableType.EditionTable);
            EditionTableNode.Nodes.Insert(index, newTENode);
            EditionTableNode.Refresh();

            foreach (var node in SrcTENode.Nodes) {
                var effectNode = node as EffectTreeNode;
                parentTE.AssignedEffects.Add(effectNode.Effect.Name);
            }
            parentTE.AssignedEffects.Init(EditionTable);
            newTENode.Rebuild(Handler);
            SetCurrentSelectedNode(newTENode);
            EffectsDebuggerDialog.RebuildTreeView();
        }

        public void CreateEffectsFromDofCommand(EditionTableTreeNode TableNode, int TCCNumber, string DofCommand, string ToyName, LedControlToolkitHandler Handler)
        {
            TableConfigSetting TCS = new TableConfigSetting();
            TCS.ParseSettingData(DofCommand);
            TCS.ResolveColorConfigs(Handler.LedControlConfigData.ColorConfigurations);

            int LastEffectsCount = EditionTableNode.EditionTable.Effects.Count;

            var Toy = Handler.Pinball.Cabinet.Toys.FirstOrDefault(T => T.Name.Equals(ToyName, StringComparison.InvariantCultureIgnoreCase));

            var newEffect = Handler.RebuildConfigurator.CreateEffect(TCS, TCCNumber, TCCNumber, TableNode.EditionTable
                                                                        , Toy
                                                                        , Handler.LedControlConfigData.LedWizNumber
                                                                        , Handler.LedControlConfigData.LedControlIniFile.DirectoryName, TableNode.EditionTable.RomName);

            for (var num = LastEffectsCount; num < EditionTableNode.EditionTable.Effects.Count; ++num) {
                EditionTableNode.EditionTable.Effects[num].Init(EditionTableNode.EditionTable);
            }

            foreach (var te in TableNode.EditionTable.TableElements) {
                te.AssignedEffects.Init(TableNode.EditionTable);
                var teNode = TableNode.Nodes.OfType<TableElementTreeNode>().Cast<TableElementTreeNode>().FirstOrDefault(N => N.TE == te);
                if (teNode == null) {
                    teNode = new TableElementTreeNode(te, LedControlToolkitHandler.ETableType.EditionTable);
                    TableNode.Nodes.Add(teNode);
                }
                if (teNode.TE.AssignedEffects.Any(AE=>AE.Effect == newEffect)) {
                    teNode.Rebuild(Handler);
                }
            }

            EffectsDebuggerDialog.RebuildTreeView();
        }


        private void ShowContextMenu(TreeNodeMouseClickEventArgs e)
        {
            if (e.Node is EffectTreeNode effectNode) {
                ContextMenu effectMenu = new ContextMenu();

                var addMenu = new MenuItem("Add effect to");
                effectMenu.MenuItems.Add(addMenu);

                addMenu.MenuItems.Add(new MenuItem("<New Table Element>", new EventHandler(this.OnCopyEffectToEditor)) { Tag = new TreeNodeCommand() { Sender = e.Node, Target = null } });
                foreach (var node in EditionTableNode.Nodes) {
                    addMenu.MenuItems.Add(new MenuItem($"{(node as TreeNode).Text}", new EventHandler(this.OnCopyEffectToEditor)) { Tag = new TreeNodeCommand() { Sender = e.Node, Target = (node as TreeNode) } });
                }

                effectMenu.Show(effectNode.GetTableType() == LedControlToolkitHandler.ETableType.EditionTable ? treeViewEditionTable:  treeViewTableLedEffects, e.Location);
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
                teMenu.Show(tableElementNode.GetTableType() == LedControlToolkitHandler.ETableType.EditionTable ? treeViewEditionTable : treeViewTableLedEffects, e.Location);
            } 
        }
        #endregion
    }
}
