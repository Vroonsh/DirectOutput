﻿using DirectOutputControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirectOutputToolkit
{
    public partial class DirectOutputToolkitPreviewForm : Form
    {
        public class AreaVisibilityTreeNode : TreeNode
        {
            private DirectOutputViewArea _Area = null;
            public DirectOutputViewArea Area
            {
                get { return _Area; }
                set {
                    _Area = value;
                    Name = _Area.Name;
                    Text = _Area.Name;
                    Checked = _Area.Visible;
                }
            }
        }

        public DirectOutputPreviewControl PreviewControl => directOutputPreviewControl1;

        public DirectOutputToolkitPreviewForm()
        {
            InitializeComponent();

            var privateDoubleBuffered = treeViewVisibility.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            privateDoubleBuffered.SetValue(treeViewVisibility, true);

            PreviewControl.SetupSet += OnViewSetupSet;
        }

        private void HierarchyFunc(DirectOutputViewArea Parent, DirectOutputViewArea Child)
        {
            if (Child == null || !Child.Enabled) return;

            if (Parent == null) {
                treeViewVisibility.Nodes.Add(new AreaVisibilityTreeNode() { Area = Child });
            } else {
                var parentNode = treeViewVisibility.Nodes.Find(Parent.Name, true).Cast<AreaVisibilityTreeNode>().FirstOrDefault(N => N.Area == Parent);
                if (parentNode != null) {
                    parentNode.Nodes.Add(new AreaVisibilityTreeNode() { Area = Child });
                }
            }
        }

        private void OnViewSetupSet(DirectOutputViewSetup setup)
        {
            if (setup == PreviewControl.DirectOutputViewSetup) {
                treeViewVisibility.Nodes.Clear();
                setup.ParseHierarchy(HierarchyFunc);
            }
        }

        public IEnumerable<T> GetChildren<T>(T Parent) where T : TreeNode
        {
            return Parent.Nodes.Cast<T>().Concat(
                   Parent.Nodes.Cast<T>().SelectMany(GetChildren));
        }

        private void treeViewVisibility_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node is AreaVisibilityTreeNode areaNode) {
                areaNode.Area.Visible = areaNode.Checked;
                var nodes = GetChildren<AreaVisibilityTreeNode>(areaNode);
                foreach(var node in nodes) {
                    node.Checked = areaNode.Checked;
                    node.Area.Visible = areaNode.Checked;
                }
                PreviewControl.Refresh();
            }
        }
    }
}