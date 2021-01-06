using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using DirectOutput;

namespace DirectOutputControls
{
    public partial class DirectOutputViewSetupControl : UserControl
    {
        private DirectOutputViewSetup _DirectOutputViewSetup = null;
        public DirectOutputViewSetup DirectOutputViewSetup
        {
            get {
                return _DirectOutputViewSetup;
            }
            set {
                _DirectOutputViewSetup = value;
                if (_DirectOutputViewSetup != null) {
                    RebuildTreeView();
                }
            }
        }

        public Action SetupChanged { get; set; }

        private void AddViewAreaToTreeView(TreeNodeCollection nodes, DirectOutputViewArea area)
        {
            var newNode = new TreeNodeArea(area);
            nodes.Add(newNode);
            foreach(var child in area.Children) {
                AddViewAreaToTreeView(newNode.Nodes, child);
            }
        }

        private void RebuildTreeView()
        {
            treeViewAreas.Nodes.Clear();
            if (_DirectOutputViewSetup != null) {
                foreach (var area in _DirectOutputViewSetup.ViewAreas) {
                    AddViewAreaToTreeView(treeViewAreas.Nodes, area);
                }
            }
        }

        public DirectOutputViewSetupControl()
        {
            InitializeComponent();
        }

        private void treeViewAreas_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) {
            }
        }

        private void OnSetupChanged()
        {
            if (_DirectOutputViewSetup != null) {
                _DirectOutputViewSetup.ComputeAreaDimensions();
                if (SetupChanged != null) {
                    SetupChanged.Invoke();
                }
            }
        }

        private void OnAddArea<T>(object sender, EventArgs e) where T : DirectOutputViewArea, new()
        {
            var item = (sender as MenuItem);
            var command = (item.Tag as TreeNodeCommand);

            if (command.Sender is TreeNodeArea nodeArea) {
                if (_DirectOutputViewSetup.HasArea(nodeArea.Area)) {
                    var newArea = new T();
                    newArea.Name = _DirectOutputViewSetup.FindUniqueAreaName(newArea.IsVirtual() ? "Virtual Area" : "New Area");
                    nodeArea.Area.Children.Add(newArea);
                    nodeArea.Nodes.Add(new TreeNodeArea(newArea));
                    nodeArea.Expand();
                    treeViewAreas.Invalidate();
                    OnSetupChanged();
                }
            } else {
                var newArea = new T();
                newArea.Name = _DirectOutputViewSetup.FindUniqueAreaName(newArea.IsVirtual() ? "Virtual Area" : "New Area");
                _DirectOutputViewSetup.ViewAreas.Add(newArea);
                treeViewAreas.Nodes.Add(new TreeNodeArea(newArea));
                treeViewAreas.Invalidate();
                OnSetupChanged();
            } 
        }

        private void OnAddVirtualArea(object sender, EventArgs e)
        {
            OnAddArea<DirectOutputViewAreaVirtual>(sender, e);
        }

        private void OnAddAnalogArea(object sender, EventArgs e)
        {
            OnAddArea<DirectOutputViewAreaAnalog>(sender, e);
        }

        private void OnAddRGBArea(object sender, EventArgs e)
        {
            OnAddArea<DirectOutputViewAreaRGB>(sender, e);
        }

        private void OnDeleteArea(object sender, EventArgs e)
        {
            var item = (sender as MenuItem);
            var command = (item.Tag as TreeNodeCommand);

            if (command.Sender is TreeNodeArea nodeArea) {
                if (_DirectOutputViewSetup.RemoveArea(nodeArea.Area)) {
                    if (nodeArea.Parent != null) {
                        nodeArea.Parent.Nodes.Remove(nodeArea);
                    } else {
                        treeViewAreas.Nodes.Remove(nodeArea);
                    }
                    treeViewAreas.Invalidate();
                    OnSetupChanged();
                }
            }
        }

        private void treeViewAreas_MouseDown(object sender, MouseEventArgs e)
        {
            if (_DirectOutputViewSetup == null) return;

            var hit = treeViewAreas.HitTest(e.X, e.Y);
            treeViewAreas.SelectedNode = hit.Node;

            if (e.Button == MouseButtons.Right) {

                if (hit.Node == null) {
                    ContextMenu areaMenu = new ContextMenu();

                    var addMenu = new MenuItem("Add area at root");
                    areaMenu.MenuItems.Add(addMenu);
                    addMenu.MenuItems.Add(new MenuItem("Virtual area", new EventHandler(this.OnAddVirtualArea)) { Tag = new TreeNodeCommand() { Sender = null, Target = null } });
                    addMenu.MenuItems.Add(new MenuItem("Analog area", new EventHandler(this.OnAddAnalogArea)) { Tag = new TreeNodeCommand() { Sender = null, Target = null } });
                    addMenu.MenuItems.Add(new MenuItem("RGB area", new EventHandler(this.OnAddRGBArea)) { Tag = new TreeNodeCommand() { Sender = null, Target = null } });

                    areaMenu.Show(treeViewAreas, e.Location);
                } else if (hit.Node is TreeNodeArea nodeArea) {
                    ContextMenu areaMenu = new ContextMenu();

                    if (nodeArea.Area.IsVirtual()) {
                        var addMenu = new MenuItem($"[{nodeArea.Text}] Add area");
                        areaMenu.MenuItems.Add(addMenu);
                        addMenu.MenuItems.Add(new MenuItem("Virtual area", new EventHandler(this.OnAddVirtualArea)) { Tag = new TreeNodeCommand() { Sender = hit.Node, Target = null } });
                        addMenu.MenuItems.Add(new MenuItem("Analog area", new EventHandler(this.OnAddAnalogArea)) { Tag = new TreeNodeCommand() { Sender = hit.Node, Target = null } });
                        addMenu.MenuItems.Add(new MenuItem("RGB area", new EventHandler(this.OnAddRGBArea)) { Tag = new TreeNodeCommand() { Sender = hit.Node, Target = null } });
                    }

                    areaMenu.MenuItems.Add(new MenuItem($"Delete [{nodeArea.Text}]", new EventHandler(this.OnDeleteArea)) { Tag = new TreeNodeCommand() { Sender = hit.Node, Target = null } });

                    areaMenu.Show(treeViewAreas, e.Location);
                }
            }

        }

        private void SetCurrentSelectedNode(TreeNodeArea node)
        {
            if (node == null) {
                propertyGridViewArea.SelectedObject = null;
                return;
            }

            if (node.Area is DirectOutputViewAreaRGB rgbArea) {
                propertyGridViewArea.SelectedObject = new ViewAreaRGBTypeDescriptor(rgbArea, true);
            } else if (node.Area is DirectOutputViewAreaVirtual vArea) {
                propertyGridViewArea.SelectedObject = new ViewAreaVirtualTypeDescriptor(vArea, true);
            } else {
                propertyGridViewArea.SelectedObject = node.Area;
            }
        }

        private void treeViewAreas_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SetCurrentSelectedNode(e.Node as TreeNodeArea);
        }

        private void propertyGridViewArea_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (propertyGridViewArea.SelectedObject is ViewAreaRGBTypeDescriptor rgbDesc) {
                rgbDesc.Refresh();
            }

            switch (e.ChangedItem.PropertyDescriptor.Name) {
                case "Name": {
                    (treeViewAreas.SelectedNode as TreeNodeArea).Refresh();
                    treeViewAreas.Refresh();
                    break;
                }

                default: {
                    break;
                }
            }

            propertyGridViewArea.Refresh();
            OnSetupChanged();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (DirectOutputViewSetup == null) {
                throw new ArgumentNullException("DirectOutputViewSetup");
            }

            SaveFileDialog fd = new SaveFileDialog() {
                Filter = "DirectOutputViewSetup Files|*.dovs|DirectOutputViewSetup XML Files|*.xml|All Files|*.*",
                DefaultExt = "dovs",
                Title = "Save DirectOutput View Setup"
            };

            fd.ShowDialog();

            if (!fd.FileName.IsNullOrEmpty()) {
                using (MemoryStream ms = new MemoryStream()) {
                    var serializer = new XmlSerializer(typeof(DirectOutputViewSetup));
                    serializer.Serialize(ms, DirectOutputViewSetup);
                    ms.Position = 0;
                    string Xml = string.Empty;
                    using (StreamReader sr = new StreamReader(ms, Encoding.Default)) {
                        Xml = sr.ReadToEnd();
                        sr.Dispose();
                    }
                    File.WriteAllText(fd.FileName, Xml);
                }
            }

        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            if (DirectOutputViewSetup == null) {
                throw new ArgumentNullException("DirectOutputViewSetup");
            }

            OpenFileDialog fd = new OpenFileDialog() {
                Filter = "DirectOutputViewSetup Files|*.dovs|DirectOutputViewSetup XML Files|*.xml|All Files|*.*",
                DefaultExt = "dovs",
                Title = "Load DirectOutput View Setup"
            };

            fd.ShowDialog();

            if (!fd.FileName.IsNullOrEmpty()) {

                string Xml;
                try {
                    Xml = DirectOutput.General.FileReader.ReadFileToString(fd.FileName);
                } catch (Exception E) {
                    Log.Exception("Could not load DirectOutput View Setup from {0}.".Build(fd.FileName), E);
                    throw new Exception("Could not read DirectOutput View Setup file {0}.".Build(fd.FileName), E);
                }

                byte[] xmlBytes = Encoding.Default.GetBytes(Xml);
                using (MemoryStream ms = new MemoryStream(xmlBytes)) {
                    try {
                        DirectOutputViewSetup.Init((DirectOutputViewSetup)new XmlSerializer(typeof(DirectOutputViewSetup)).Deserialize(ms));
                        RebuildTreeView();
                        OnSetupChanged();
                    } catch (Exception E) {
                        Exception Ex = new Exception("Could not deserialize DirectOutput View Setup from XML data.", E);
                        Ex.Data.Add("XML Data", Xml);
                        Log.Exception("Could not load DirectOutput View Setup from XML data.", Ex);
                        throw Ex;
                    }
                }
            }

        }
    }
}
