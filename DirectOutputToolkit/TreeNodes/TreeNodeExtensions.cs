using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirectOutputToolkit
{
    public static class TreeNodeExtensions
    {
        private static void FindAllNodesRecursive<T>(TreeNode node, Predicate<T> Match, List<T> matchingNodes) where T: TreeNode
        {
            if (node is T && Match((T)node)) {
                matchingNodes.Add((T)node);
            }
            foreach(TreeNode child in node.Nodes) {
                FindAllNodesRecursive(child, Match, matchingNodes);
            }
        }

        public static IEnumerable<T> FindAllNodes<T>(this TreeNode rootNode, Predicate<T> Match) where T : TreeNode
        {
            var matchingNodes = new List<T>();
            FindAllNodesRecursive(rootNode, Match, matchingNodes);
            return matchingNodes;
        }
    }
}
