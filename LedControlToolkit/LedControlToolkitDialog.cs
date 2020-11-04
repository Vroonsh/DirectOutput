﻿using DirectOutput;
using DirectOutput.Cab.Toys.Hardware;
using DirectOutput.FX;
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
        private Pinball Pinball;
        private Settings Settings = new Settings();

        public LedControlToolkitDialog()
        {
            InitializeComponent();

            Settings = Settings.LoadSettings();

            treeViewTableLedEffects.ImageList = imageListIcons;
            treeViewTableLedEffects.FullRowSelect = true;
            treeViewTableLedEffects.HideSelection = false;
            treeViewEffect.ImageList = imageListIcons;
        }

        private void LedControlToolkit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Pinball != null) {
                Pinball.Finish();
            }

            Settings.PulseDurationMs = (int)numericUpDownPulseDuration.Value;
            Settings.ShowMatrixGrid = panelPreviewLedMatrix.ShowMatrixGrid;
            Settings.ShowPreviewAreas = panelPreviewLedMatrix.ShowPreviewAreas;

            Settings.SaveSettings();
        }

        private class EffectTreeNode : TreeNode
        {
            public static readonly string TableElementTestName = "LedControlToolkit_Test";

            public EffectTreeNode(string name, IEffect[] effects) : base(name)
            {
                Effects = effects.ToArray();
                TE.AssignedEffects.Clear();
                foreach(var effect in Effects) {
                    TE.AssignedEffects.Add(new AssignedEffect("TestEffect") { Effect = effect });
                }
                ImageIndex = TE.GetTableElementData().Value > 0 ? 1 : 0;
            }

            public IEffect[] Effects { get; set; }
            public TableElement TE = new TableElement(TableElementTestName, 0);
        }

        private void DisplayTableElements()
        {
            Pinball.Table.TableElements.Sort((TE1, TE2) => (TE1.TableElementType == TE2.TableElementType ? TE1.Number.CompareTo(TE2.Number) : TE1.TableElementType.CompareTo(TE2.TableElementType)));

            treeViewTableLedEffects.Nodes.Clear();
            var ledstrips = Pinball.Cabinet.Toys.Where(T => T is LedStrip).Select(T => (T as LedStrip).Name).ToArray();
            var effects = Pinball.Table.Effects.Where(E => E.ActOnAnyToys(ledstrips)).ToList();

            List<TableElement> assignedeffects = new List<TableElement>();
            foreach (TableElement TE in Pinball.Table.TableElements) {
                foreach (var effect in TE.AssignedEffects) {
                    if (effects.Contains(effect.Effect)) {
                        assignedeffects.Add(TE);
                        break;
                    }
                }
            }

            foreach(var tableElement in assignedeffects) {
                var elementName = tableElement.Name.IsNullOrEmpty() ? $"{tableElement.TableElementType}[{tableElement.Number}]" : tableElement.Name;
                var listNode = new EffectTreeNode($"{elementName} [{tableElement.AssignedEffects.Count} effects]", tableElement.AssignedEffects.Select(A => A.Effect).ToArray()); 

                foreach(var effect in tableElement.AssignedEffects) {
                    var effectTypeName = effect.Effect.GetType().ToString();
                    var effectNode = new EffectTreeNode(effectTypeName.Substring(effectTypeName.LastIndexOf('.') + 1), new IEffect[] { effect.Effect });

                    var effectWithTarget = effect.Effect as EffectEffectBase;
                    var curEffectNode = effectNode;
                    while (effectWithTarget != null) {
                        var targetEffect = effectWithTarget.TargetEffect;

                        if (targetEffect != null) {
                            effectTypeName = targetEffect.GetType().ToString();
                            var targetEffectNode = new EffectTreeNode(effectTypeName.Substring(effectTypeName.LastIndexOf('.') + 1), new IEffect[] { targetEffect });

                            curEffectNode.Nodes.Add(targetEffectNode);

                            curEffectNode = targetEffectNode;
                            effectWithTarget = targetEffect as EffectEffectBase;
                        }
                    }

                    listNode.Nodes.Add(effectNode);
                }

                treeViewTableLedEffects.Nodes.Add(listNode);
            }
        }

        private void SetupPinball()
        {
            if (Pinball != null) {
                Pinball.Finish();
            }

            LedControlToolkitControllerAutoConfigurator.LastLedControlIniFilename = Settings.LastLedControlIniFilename;

            Pinball = new Pinball();
            Pinball.Setup(GlobalConfigFilename: Settings.LastGlobalConfigFilename, RomName: Settings.LastRomName);

            var controllers = Pinball.Cabinet.OutputControllers.Where(c => c is LedControlToolkitController).ToArray();
            if (controllers.Length > 0) {
                (controllers[0] as LedControlToolkitController).OutputControl = panelPreviewLedMatrix;
            } else {
                var ledControlFilename = Path.GetFileNameWithoutExtension(Settings.LastLedControlIniFilename);
                var ledWizNumber = 30;
                if (ledControlFilename.Contains("directoutputconfig")) {
                    ledWizNumber = Int32.Parse(ledControlFilename.Replace("directoutputconfig", ""));
                }
                var previewController = new LedControlToolkitController() { Name = "LedControlToolkitController", LedWizNumber = ledWizNumber };
                previewController.Init(Pinball.Cabinet);
                Pinball.Cabinet.OutputControllers.Add(previewController);
            }

            panelPreviewLedMatrix.SetupPreviewParts(Pinball.Cabinet, Settings);

            Pinball.Init();
            DisplayTableElements();
        }

        private void RomNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var combo = (sender as ComboBox);
            Settings.LastRomName = combo.Text;
            SetupPinball();
        }

        public bool LoadConfig()
        {
            OpenConfigDialog OCD = new OpenConfigDialog(Settings);
            if (OCD.ShowDialog() == DialogResult.OK) {

                RomNameComboBox.Text = Settings.LastRomName;
                numericUpDownPulseDuration.Value = Settings.PulseDurationMs;
                checkBoxPreviewArea.Checked = Settings.ShowPreviewAreas;
                checkBoxPreviewGrid.Checked = Settings.ShowMatrixGrid;

                UpdatePreviewAreaListControl();

                SetupPinball();
                CheckPreviewAreaMissmatch();
                return true;
            } else {
                return false;
            }
        }

        private void LedControlToolkit_Load(object sender, EventArgs e)
        {
            if (!LoadConfig()) {
                this.Close();
            }
        }

        private void CheckPreviewAreaMissmatch()
        {
            var ledstrips = Pinball.Cabinet.Toys.Where(T => T is LedStrip).Select(T => T as LedStrip).ToArray();
            List<string> missingLedstrip = new List<string>();
            foreach (var ledstrip in ledstrips) {
                if (!Settings.LedPreviewAreas.Any(A => A.Name.Equals(ledstrip.Name, StringComparison.InvariantCultureIgnoreCase))) {
                    missingLedstrip.Add(ledstrip.Name);
                }
            }

            if (missingLedstrip.Count != 0) {
                if (MessageBox.Show("There are missing ledstrips from cabinet in preview areas settings.\n" +
                                $"Missing ledstrips :\n\t{string.Join(", ", missingLedstrip.ToArray())}\n\n" +
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

        private void tabPageEffectEditor_Enter(object sender, EventArgs e)
        {
            propertyGridEffect.SelectedObject = (treeViewTableLedEffects.SelectedNode as EffectTreeNode)?.Effects[0];
        }

        private void tabPageSettings_Enter(object sender, EventArgs e)
        {
            propertyGridEffect.SelectedObject = listBoxPreviewAreas.SelectedItem;
        }


        private void listBoxPreviewAreas_SelectedIndexChanged(object sender, EventArgs e)
        {
            var listCtrl = (sender as ListControl);
            if (listCtrl.SelectedIndex != -1) {
                propertyGridEffect.SelectedObject = listBoxPreviewAreas.SelectedItem;
            }
        }

        private void propertyGridEffect_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            var propertyGrid = (s as PropertyGrid);
            if (propertyGrid.SelectedObject is Settings.LedPreviewArea selectedArea) {
                if (e.ChangedItem.PropertyDescriptor.Name == "Name") {
                    //Check if there is no duplicate zone area names
                    foreach(var pArea in Settings.LedPreviewAreas) {
                        if (pArea != selectedArea && pArea.Name.Equals(selectedArea.Name, StringComparison.InvariantCultureIgnoreCase)) {
                            MessageBox.Show($"There is already another preview area named {selectedArea.Name}.\nPlease choose another name.", "Duplicate preview area names", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            selectedArea.Name = e.OldValue as string;
                            return;
                        }
                    }

                    UpdatePreviewAreaListControl();
                }
                panelPreviewLedMatrix.SetupPreviewParts(Pinball.Cabinet, Settings);
                panelPreviewLedMatrix.Refresh();
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

        private void buttonNewArea_Click(object sender, EventArgs e)
        {
            Settings.LedPreviewAreas.Add(new Settings.LedPreviewArea() { Name = GetNewAreaName() });
            UpdatePreviewAreaListControl();
            listBoxPreviewAreas.SelectedIndex = listBoxPreviewAreas.Items.Count-1;
            panelPreviewLedMatrix.SetupPreviewParts(Pinball.Cabinet, Settings);
            panelPreviewLedMatrix.Refresh();
        }

        private void buttonDuplicateArea_Click(object sender, EventArgs e)
        {
            Settings.LedPreviewAreas.Add(new Settings.LedPreviewArea(listBoxPreviewAreas.SelectedItem as Settings.LedPreviewArea) { Name = GetNewAreaName() });
            UpdatePreviewAreaListControl();
            listBoxPreviewAreas.SelectedIndex = listBoxPreviewAreas.Items.Count - 1;
            panelPreviewLedMatrix.SetupPreviewParts(Pinball.Cabinet, Settings);
            panelPreviewLedMatrix.Refresh();
        }

        private void buttonDeleteArea_Click(object sender, EventArgs e)
        {
            var selectedArea = listBoxPreviewAreas.SelectedItem as Settings.LedPreviewArea;
            if (selectedArea != null && MessageBox.Show($"Do you really want to delete preview area {selectedArea.Name} ?", "Delete preview area", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                Settings.LedPreviewAreas.Remove(selectedArea);
                listBoxPreviewAreas.SelectedIndex = -1;
                UpdatePreviewAreaListControl();
                panelPreviewLedMatrix.SetupPreviewParts(Pinball.Cabinet, Settings);
                panelPreviewLedMatrix.Refresh();
            }
        }

        private void buttonSaveSettings_Click(object sender, EventArgs e)
        {
            Settings.SaveSettings();
        }

        EffectTreeNode CurrentTableSelectedNode = null;

        private void SetEffectTreeNodeAvtive(EffectTreeNode node, int active)
        {
            node.ImageIndex = active;
            node.SelectedImageIndex = active;
        }

        private void treeViewTableLedEffects_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left) {
                var te = Pinball.Table.TableElements.FirstOrDefault(T => T.Name.Equals(EffectTreeNode.TableElementTestName, StringComparison.InvariantCultureIgnoreCase));
                if (te != null) {
                    Pinball.Table.TableElements.Remove(te);
                }

                CurrentTableSelectedNode = null;
                var effectNode = (e.Node as EffectTreeNode);
                if (effectNode == null) {
                    propertyGridEffect.SelectedObject = null;
                    return;
                }
                propertyGridEffect.SelectedObject = effectNode.Effects.Length == 1 ? effectNode.Effects[0] : null;

                Pinball.Table.TableElements.Add(effectNode.TE);
                CurrentTableSelectedNode = effectNode;
                if (CurrentTableSelectedNode != null) {
                    var D = CurrentTableSelectedNode.TE.GetTableElementData();
                    buttonActivationTable.Text = D.Value > 0 ? "Dectivate" : "Activate";
                    buttonPulseTable.Text = D.Value > 0 ? @"Pulse ¯\_/¯" : @"Pulse _/¯\_";
                }
            }
        }

        private void buttonActivationTable_Click(object sender, EventArgs e)
        {
            if (CurrentTableSelectedNode != null) {
                TableElementData D = CurrentTableSelectedNode.TE.GetTableElementData();
                D.Value = D.Value > 0 ? 0 : 1;
                Pinball.ReceiveData(D);
                SetEffectTreeNodeAvtive(CurrentTableSelectedNode, D.Value > 0 ? 1 : 0);
                buttonActivationTable.Text = D.Value > 0 ? "Dectivate" : "Activate";
            }
        }

        private void buttonPulseTable_Click(object sender, EventArgs e)
        {
            if (CurrentTableSelectedNode != null) {
                TableElementData D = CurrentTableSelectedNode.TE.GetTableElementData();
                D.Value = D.Value > 0 ? 0 : 1;
                Pinball.ReceiveData(D);
                SetEffectTreeNodeAvtive(CurrentTableSelectedNode, D.Value > 0 ? 1 : 0);
                buttonPulseTable.Text = D.Value > 0 ? @"Pulse ¯\_/¯" : @"Pulse _/¯\_";
                Thread.Sleep((int)numericUpDownPulseDuration.Value);
                D.Value = D.Value > 0 ? 0 : 1;
                Pinball.ReceiveData(D);
                SetEffectTreeNodeAvtive(CurrentTableSelectedNode, D.Value > 0 ? 1 : 0);
                buttonPulseTable.Text = D.Value > 0 ? @"Pulse ¯\_/¯" : @"Pulse _/¯\_";
            }
        }

    }
}
