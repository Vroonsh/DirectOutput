using DirectOutputControls;
using DofConfigToolWrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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

            InitDofResources();
        }

        private void InitDofResources()
        {
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.StartButton, imageListDefaultIcons.Images["StartButton"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.LaunchButton, imageListDefaultIcons.Images["LaunchButton"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.AuthenticLaunchBall, imageListDefaultIcons.Images["AuthLaunchButton"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.ZBLaunchBall, imageListDefaultIcons.Images["ZBLaunchButton"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.FireButton, imageListDefaultIcons.Images["FireButton"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Coin, imageListDefaultIcons.Images["Coin"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Exit, imageListDefaultIcons.Images["Exit"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.ExtraBall, imageListDefaultIcons.Images["ExtraBall"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Genre, imageListDefaultIcons.Images["Genre"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.HowToPlay, imageListDefaultIcons.Images["HowToPlay"]);

            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Bumper10BackLeft, imageListDefaultIcons.Images["BumperCap"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Bumper10BackCenter, imageListDefaultIcons.Images["BumperCap"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Bumper10BackRight, imageListDefaultIcons.Images["BumperCap"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Bumper10MiddleLeft, imageListDefaultIcons.Images["BumperCap"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Bumper10MiddleCenter, imageListDefaultIcons.Images["BumperCap"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Bumper10MiddleRight, imageListDefaultIcons.Images["BumperCap"]);

            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Bumper8Back, imageListDefaultIcons.Images["BumperCapKiss4"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Bumper8Left, imageListDefaultIcons.Images["BumperCapKiss2"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Bumper8Right, imageListDefaultIcons.Images["BumperCapKiss1"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Bumper8Center, imageListDefaultIcons.Images["BumperCapKiss3"]);

            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.SlingshotLeft, imageListDefaultIcons.Images["SlingshotLeft"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.SlingshotRight, imageListDefaultIcons.Images["SlingshotRight"]);

            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.FlipperLeft, imageListDefaultIcons.Images["LeftFlipper"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.FlipperRight, imageListDefaultIcons.Images["RightFlipper"]);

            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Shaker, imageListDefaultIcons.Images["Shaker"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Gear, imageListDefaultIcons.Images["Gear"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Fan, imageListDefaultIcons.Images["Fan"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Knocker, imageListDefaultIcons.Images["Knocker"]);

            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Beacon, imageListDefaultIcons.Images["BeaconRed"]);

            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.ChimeUnitHighTone, imageListDefaultIcons.Images["Chime"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.ChimeUnitMidTone, imageListDefaultIcons.Images["Chime"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.ChimeUnitLowTone, imageListDefaultIcons.Images["Chime"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.ChimeUnitExtraLowTone, imageListDefaultIcons.Images["Chime"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Chime5, imageListDefaultIcons.Images["Chime"]);

            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.TopperBell, imageListDefaultIcons.Images["Bell"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.ShellBellLarge, imageListDefaultIcons.Images["Bell"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.ShellBellSmall, imageListDefaultIcons.Images["Bell"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.RepeatingBell, imageListDefaultIcons.Images["Bell"]);
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

        private void treeViewVisibility_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node is AreaVisibilityTreeNode areaNode) {
                areaNode.Area.Visible = areaNode.Checked;
                var nodes = areaNode.GetChildren<AreaVisibilityTreeNode>();
                foreach(var node in nodes) {
                    node.Checked = areaNode.Checked;
                    node.Area.Visible = areaNode.Checked;
                }
                PreviewControl.Refresh();
            }
        }

        private void checkBoxDisplayAreas_CheckedChanged(object sender, EventArgs e)
        {
            PreviewControl.DrawViewAreasInfos = checkBoxDisplayAreas.Checked;
            PreviewControl.Invalidate();
        }
    }
}
