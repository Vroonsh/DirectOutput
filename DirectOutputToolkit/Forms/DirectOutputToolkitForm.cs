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
                Handler.SetupPinball();

                PreviewForm.Show(this);
                PreviewForm.PreviewControl.OnSetupChanged(DofViewSetup);


                RomNameComboBox.Items.Clear();
                RomNameComboBox.Items.Add("");
                RomNameComboBox.Items.AddRange(Handler.LedControlConfigList[0].TableConfigurations.Select(TC => TC.ShortRomName).ToArray());

                EditionTableNode = new EditionTableTreeNode(Handler.GetTable(DirectOutputToolkitHandler.ETableType.EditionTable));
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
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
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

            foreach (TableElement tableElement in table.TableElements) {
                var elementName = tableElement.Name.IsNullOrEmpty() ? $"{tableElement.TableElementType}[{tableElement.Number}]" : tableElement.Name;
                var listNode = new TableElementTreeNode(tableElement, DirectOutputToolkitHandler.ETableType.ReferenceTable);

                foreach (var effect in tableElement.AssignedEffects) {
                    var effectNode = new EffectTreeNode(tableElement, DirectOutputToolkitHandler.ETableType.ReferenceTable, effect.Effect, Handler.ColorConfigurations);
                    listNode.Nodes.Add(effectNode);
                }

                listNode.Refresh();
                treeViewTableEffects.Nodes.Add(listNode);
            }
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
            var parentTE = TENode?.TE ?? null;
            var EditionTable = Handler.GetTable(DirectOutputToolkitHandler.ETableType.EditionTable);

            if (TENode == null) {
                parentTE = new TableElement() { TableElementType = TableElementTypeEnum.NamedElement, Name = $"{NewTableElementName}{EditionTable.TableElements.Count}" };
                parentTE.AssignedEffects = new AssignedEffectList();
                Handler.GetTable(DirectOutputToolkitHandler.ETableType.EditionTable).TableElements.Add(parentTE);
                TENode = new TableElementTreeNode(parentTE, DirectOutputToolkitHandler.ETableType.EditionTable);
                EditionTableNode.Nodes.Add(TENode);
                EditionTableNode.Refresh();
            }

            var newEffectNode = new EffectTreeNode(parentTE, DirectOutputToolkitHandler.ETableType.EditionTable, SrcEffectNode.Effect, Handler.ColorConfigurations);
            newEffectNode.Rebuild(Handler, SrcEffectNode.Effect);
            parentTE.AssignedEffects.Init(Handler.GetTable(DirectOutputToolkitHandler.ETableType.EditionTable));
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
            var EditionTable = Handler.GetTable(DirectOutputToolkitHandler.ETableType.EditionTable);

            if (TargetTENode == null) {
                parentTE = new TableElement() { TableElementType = TableElementTypeEnum.NamedElement, Name = $"{NewTableElementName}{EditionTable.TableElements.Count}" };
                parentTE.AssignedEffects = new AssignedEffectList();
                EditionTable.TableElements.Add(parentTE);
                TargetTENode = new TableElementTreeNode(parentTE, DirectOutputToolkitHandler.ETableType.EditionTable);
                EditionTableNode.Nodes.Add(TargetTENode);
                EditionTableNode.Refresh();
            }

            foreach (var node in SrcTENode.Nodes) {
                var effectNode = node as EffectTreeNode;
                var newEffectNode = new EffectTreeNode(parentTE, DirectOutputToolkitHandler.ETableType.EditionTable, effectNode.Effect, Handler.ColorConfigurations);
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
                        insertMenu.MenuItems.Add(new MenuItem($"{(node as TreeNode).Text}", new EventHandler(this.OnInsertTableElementToEditor)) { Tag = new TreeNodeCommand() { Sender = e.Node, Target = (node as TreeNode) } });
                    }
                }
                teMenu.Show(tableElementNode.GetTableType() == DirectOutputToolkitHandler.ETableType.EditionTable ? treeViewEditionTable : treeViewTableEffects, e.Location);
            }
        }
        #endregion

    }
}
