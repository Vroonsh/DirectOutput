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
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.StartButton, imageListIcons.Images["StartButton"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.LaunchButton, imageListIcons.Images["LaunchButton"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.AuthenticLaunchBall, imageListIcons.Images["AuthLaunchButton"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.ZBLaunchBall, imageListIcons.Images["ZBLaunchButton"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.FireButton, imageListIcons.Images["FireButton"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Coin, imageListIcons.Images["Coin"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Exit, imageListIcons.Images["Exit"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.ExtraBall, imageListIcons.Images["ExtraBall"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Genre, imageListIcons.Images["Genre"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.HowToPlay, imageListIcons.Images["HowToPlay"]);

            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Bumper10BackLeft, imageListIcons.Images["BumperCap"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Bumper10BackCenter, imageListIcons.Images["BumperCap"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Bumper10BackRight, imageListIcons.Images["BumperCap"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Bumper10MiddleLeft, imageListIcons.Images["BumperCap"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Bumper10MiddleCenter, imageListIcons.Images["BumperCap"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Bumper10MiddleRight, imageListIcons.Images["BumperCap"]);

            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Bumper8Back, imageListIcons.Images["BumperCapKiss4"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Bumper8Left, imageListIcons.Images["BumperCapKiss2"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Bumper8Right, imageListIcons.Images["BumperCapKiss1"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Bumper8Center, imageListIcons.Images["BumperCapKiss3"]);

            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.SlingshotLeft, imageListIcons.Images["SlingshotLeft"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.SlingshotRight, imageListIcons.Images["SlingshotRight"]);

            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.FlipperLeft, imageListIcons.Images["LeftFlipper"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.FlipperRight, imageListIcons.Images["RightFlipper"]);

            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Shaker, imageListIcons.Images["Shaker"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Gear, imageListIcons.Images["Gear"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Fan, imageListIcons.Images["Fan"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Knocker, imageListIcons.Images["Knocker"]);

            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Beacon, imageListIcons.Images["GyroRed"]);

            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.ChimeUnitHighTone, imageListIcons.Images["Chime"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.ChimeUnitMidTone, imageListIcons.Images["Chime"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.ChimeUnitLowTone, imageListIcons.Images["Chime"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.ChimeUnitExtraLowTone, imageListIcons.Images["Chime"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Chime5, imageListIcons.Images["Chime"]);

            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.TopperBell, imageListIcons.Images["Bell"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.ShellBellLarge, imageListIcons.Images["Bell"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.ShellBellSmall, imageListIcons.Images["Bell"]);
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.RepeatingBell, imageListIcons.Images["Bell"]);
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
    }
}
