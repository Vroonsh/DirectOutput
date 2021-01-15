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

        public static IEnumerable<T> GetNodes<T>(this TreeView TreeView) where T : TreeNode
        {
            List<T> nodes = new List<T>();
            foreach(var node in TreeView.Nodes) {
                if (node is T) {
                    nodes.Add(node as T);
                }
                nodes.AddRange((node as TreeNode).GetChildren<T>());
            }
            return nodes.ToArray();
        }
    }
}
