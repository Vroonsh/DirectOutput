using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirectOutputControls
{
    public static class TreeViewExtensions
    {
        public static IEnumerable<T> GetChildren<T>(this TreeNode Parent) where T : TreeNode
        {
            return Parent.Nodes.OfType<T>().Concat(
                   Parent.Nodes.OfType<T>().SelectMany(GetChildren<T>));
        }

        public static IEnumerable<T> GetChildren<T>(this TreeNode Parent, Func<T, bool> Match) where T : TreeNode
        {
            return Parent.Nodes.OfType<T>().Concat(
                   Parent.Nodes.OfType<T>().SelectMany(GetChildren<T>).Where(Match));
        }
    }
}
