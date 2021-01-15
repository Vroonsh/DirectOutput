using System;
using System.Windows.Forms;

namespace DirectOutputControls
{
    public class TreeNodeArea : TreeNode
    {
        public DirectOutputViewArea Area { get; }

        public TreeNodeArea(DirectOutputViewArea area)
        {
            Area = area;
            Refresh();
        }

        internal void Refresh()
        {
            Text = Area.Name;
            Checked = Area.Visible;
        }
    }
}
