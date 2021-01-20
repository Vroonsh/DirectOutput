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
            var imagePath = ".\\DOTK\\images";

            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.StartButton, Image.FromFile(Path.Combine(imagePath, "StartButton.png")));
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.LaunchButton, Image.FromFile(Path.Combine(imagePath, "LaunchButton.png")));
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.AuthenticLaunchBall, Image.FromFile(Path.Combine(imagePath, "AuthLaunchButton.png")));
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.ZBLaunchBall, Image.FromFile(Path.Combine(imagePath, "ZBLaunchButton.png")));
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.FireButton, Image.FromFile(Path.Combine(imagePath, "FireButton.png")));
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Coin, Image.FromFile(Path.Combine(imagePath, "Coin.png")));
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Exit, Image.FromFile(Path.Combine(imagePath, "Exit.png")));
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.ExtraBall, Image.FromFile(Path.Combine(imagePath, "ExtraBall.png")));
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Genre, Image.FromFile(Path.Combine(imagePath, "Genre.png")));
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.HowToPlay, Image.FromFile(Path.Combine(imagePath, "HowToPlay.png")));

            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Bumper10BackLeft, Image.FromFile(Path.Combine(imagePath, "BumperCap.png")));
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Bumper10BackCenter, Image.FromFile(Path.Combine(imagePath, "BumperCap.png")));
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Bumper10BackRight, Image.FromFile(Path.Combine(imagePath, "BumperCap.png")));
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Bumper10MiddleLeft, Image.FromFile(Path.Combine(imagePath, "BumperCap.png")));
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Bumper10MiddleCenter, Image.FromFile(Path.Combine(imagePath, "BumperCap.png")));
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Bumper10MiddleRight, Image.FromFile(Path.Combine(imagePath, "BumperCap.png")));

            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Bumper8Back, Image.FromFile(Path.Combine(imagePath, "BumperCapKiss4.png")));
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Bumper8Left, Image.FromFile(Path.Combine(imagePath, "BumperCapKiss2.png")));
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Bumper8Right, Image.FromFile(Path.Combine(imagePath, "BumperCapKiss1.png")));
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Bumper8Center, Image.FromFile(Path.Combine(imagePath, "BumperCapKiss3.png")));

            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.SlingshotLeft, Image.FromFile(Path.Combine(imagePath, "SlingshotLeft.png")));
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.SlingshotRight, Image.FromFile(Path.Combine(imagePath, "SlingshotRight.png")));

            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.FlipperLeft, Image.FromFile(Path.Combine(imagePath, "LeftFlipper.png")));
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.FlipperRight, Image.FromFile(Path.Combine(imagePath, "RightFlipper.png")));

            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Shaker, Image.FromFile(Path.Combine(imagePath, "Shaker.png")));
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Gear, Image.FromFile(Path.Combine(imagePath, "Gear.png")));
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Fan, Image.FromFile(Path.Combine(imagePath, "Fan.png")));
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Knocker, Image.FromFile(Path.Combine(imagePath, "Knocker.png")));

            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Beacon, Image.FromFile(Path.Combine(imagePath, "GyroRed.png")));

            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.ChimeUnitHighTone, Image.FromFile(Path.Combine(imagePath, "Chime.png")));
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.ChimeUnitMidTone, Image.FromFile(Path.Combine(imagePath, "Chime.png")));
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.ChimeUnitLowTone, Image.FromFile(Path.Combine(imagePath, "Chime.png")));
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.ChimeUnitExtraLowTone, Image.FromFile(Path.Combine(imagePath, "Chime.png")));
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.Chime5, Image.FromFile(Path.Combine(imagePath, "Chime.png")));

            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.TopperBell, Image.FromFile(Path.Combine(imagePath, "Bell.png")));
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.ShellBellLarge, Image.FromFile(Path.Combine(imagePath, "Bell.png")));
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.ShellBellSmall, Image.FromFile(Path.Combine(imagePath, "Bell.png")));
            DofConfigToolResources.AddDofOutputIcon(DofConfigToolOutputEnum.RepeatingBell, Image.FromFile(Path.Combine(imagePath, "Bell.png")));
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
