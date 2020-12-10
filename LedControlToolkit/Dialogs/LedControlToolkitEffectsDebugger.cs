using DirectOutput.FX;
using DirectOutput.LedControl.Loader;
using DirectOutput.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LedControlToolkit
{
    public partial class LedControlToolkitEffectsDebugger : Form
    {
        public LedControlToolkitHandler Handler;
        int LastSelectedIndex = -1;
        private Table SelectedTable;

        enum NodeType
        {
            Pinball,
            Toys,

            Table,
            Elements,
            Effects
        }

        private Dictionary<NodeType, TreeNode> NodesDict = new Dictionary<NodeType, TreeNode>();

        public Delegate RefreshTreeView { get; private set; }

        public LedControlToolkitEffectsDebugger()
        {
            InitializeComponent();
        }

        private void LedControlToolkitEffectsDebugger_Activated(object sender, EventArgs e)
        {
            RefreshTreeView = new Action(DoRefreshTreeView);

            comboBoxTable.DataSource = Handler.TableDescriptors.Select(TD => $"{TD.Value.Table.TableName}").ToArray();
            LastSelectedIndex = LastSelectedIndex.Limit(0, comboBoxTable.Items.Count-1);
            comboBoxTable.SelectedIndex = LastSelectedIndex;

            RebuildTreeView();

            InitMainThread();
            MainThreadSignal();
        }

        private void LedControlToolkitEffectsDebugger_Deactivate(object sender, EventArgs e)
        {
            FinishMainThread();
            RefreshTreeView = null;
        }

        public void RebuildTreeView()
        {
            treeViewDebug.Nodes.Clear();

            NodesDict[NodeType.Pinball] = new TreeNode("Pinball");
            treeViewDebug.Nodes.Add(NodesDict[NodeType.Pinball]);
            NodesDict[NodeType.Toys] = new TreeNode("Toys");
            NodesDict[NodeType.Pinball].Nodes.Add(NodesDict[NodeType.Toys]);

            foreach(var Toy in Handler.Pinball.Cabinet.Toys) {
                NodesDict[NodeType.Toys].Nodes.Add(new TreeNode($"{Toy.Name}"));
            }

            NodesDict[NodeType.Table] = new TreeNode(SelectedTable != null ? $"Table { SelectedTable.TableName }[{ SelectedTable.RomName}]" : "Table");
            
            treeViewDebug.Nodes.Add(NodesDict[NodeType.Table]);
            NodesDict[NodeType.Elements] = new TreeNode("Elements");
            NodesDict[NodeType.Table].Nodes.Add(NodesDict[NodeType.Elements]);
            NodesDict[NodeType.Effects] = new TreeNode("Effects");
            NodesDict[NodeType.Table].Nodes.Add(NodesDict[NodeType.Effects]);

            if (SelectedTable != null) {
                var ColorList = Handler.LedControlConfigData.ColorConfigurations.GetCabinetColorList();

                foreach (var TE in SelectedTable.TableElements) {
                    var baseTENode = new TreeNode($"{TE.ToString()} [Value:{TE.Value}]");
                    NodesDict[NodeType.Elements].Nodes.Add(baseTENode);
                    foreach(var AE in TE.AssignedEffects) {
                        TableConfigSetting TCS = new TableConfigSetting();
                        TCS.FromEffect(AE.Effect);
                        var baseAENode = new TreeNode($"{TCS.ToConfigToolCommand(ColorList, false)}");
                        baseTENode.Nodes.Add(baseAENode);
                        IEffect cureffect = AE.Effect;
                        while (cureffect != null) {
                            baseAENode.Nodes.Add($"{cureffect.Name}");
                            if (cureffect is EffectEffectBase effectWithTarget) {
                                cureffect = effectWithTarget.TargetEffect;
                            } else {
                                cureffect = null;
                            }
                        }
                    }

                }

                foreach (var E in SelectedTable.Effects) {
                    NodesDict[NodeType.Effects].Nodes.Add(new TreeNode($"{E.Name}"));
                }
            }

            NodesDict[NodeType.Pinball].ExpandAll();
            NodesDict[NodeType.Table].Expand();
            NodesDict[NodeType.Elements].Expand();
            NodesDict[NodeType.Effects].Collapse();

            treeViewDebug.Refresh();
        }

        private void DoRefreshTreeView()
        {
            bool needRebuild =  (Handler.Pinball.Cabinet.Toys.Count != NodesDict[NodeType.Toys].Nodes.Count) ||
                                (SelectedTable.TableElements.Count != NodesDict[NodeType.Elements].Nodes.Count) ||
                                (SelectedTable.Effects.Count != NodesDict[NodeType.Effects].Nodes.Count);

            if (needRebuild) {
                RebuildTreeView();
            }
        }

        private void comboBoxTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedTable = Handler.TableDescriptors.Select(TD => TD.Value.Table).FirstOrDefault(T => T.TableName.Equals(comboBoxTable.Text, StringComparison.InvariantCultureIgnoreCase));
            LastSelectedIndex = comboBoxTable.SelectedIndex;
            RebuildTreeView();
        }

        #region MainThread
        private void InitMainThread()
        {

            if (!MainThreadIsActive) {
                KeepMainThreadAlive = true;
                try {
                    MainThread = new Thread(MainThreadDoIt);
                    MainThread.Name = "LedControlToolkitEffectsDebugger MainThread ";
                    MainThread.Start();
                } catch (Exception E) {
                    throw new Exception("LedControlToolkitEffectsDebugger MainThread could not start.", E);
                }
            }
        }

        private void FinishMainThread()
        {
            if (MainThread != null) {
                try {
                    KeepMainThreadAlive = false;
                    lock (MainThreadLocker) {
                        Monitor.Pulse(MainThreadLocker);
                    }
                    if (!MainThread.Join(1000)) {
                        MainThread.Abort();
                    }
                    MainThread = null;
                } catch (Exception E) {
                    throw new Exception("A error occured during termination of LedControlToolkitEffectsDebugger MainThread", E);
                }
            }
        }


        public bool MainThreadIsActive
        {
            get {
                if (MainThread != null) {
                    if (MainThread.IsAlive) {
                        return true;
                    }
                }
                return false;
            }
        }

        public void MainThreadSignal()
        {
            lock (MainThreadLocker) {
                Monitor.Pulse(MainThreadLocker);
            }
        }


        private Thread MainThread { get; set; }
        private object MainThreadLocker = new object();
        private bool KeepMainThreadAlive = true;
        const int MinDelayTimeMs = 10;
        private DateTime LastProcess = DateTime.MinValue;

        private void MainThreadDoIt()
        {
            try {
                while (KeepMainThreadAlive) {
                    //Display Pinbal status
                    if (KeepMainThreadAlive && RefreshTreeView != null) {
                        treeViewDebug.Invoke(RefreshTreeView);
                    }

                    if (KeepMainThreadAlive) {
                        lock (MainThreadLocker) {
                            while ((DateTime.Now - LastProcess).Milliseconds <= MinDelayTimeMs && KeepMainThreadAlive) {
                                Monitor.Wait(MainThreadLocker, 50);  // Lock is released while we’re waiting
                            }
                            LastProcess = DateTime.Now;
                        }
                    }
                }
            } catch (Exception E) {
                throw new Exception("A unexpected exception occured in the LedControlToolkitEffectsDebugger MainThread", E);
            }
        }
        #endregion

    }
}
