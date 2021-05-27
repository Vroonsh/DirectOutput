using DirectOutput;
using DirectOutput.FX;
using DirectOutput.FX.MatrixFX;
using DirectOutput.LedControl.Loader;
using DirectOutput.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirectOutputToolkit
{
    public class TableElementTreeNode : TreeNode, ITableElementTreeNode
    {
        public virtual TableElement GetTableElement() => TE;
        private DirectOutputToolkitHandler.ETableType _TableType = DirectOutputToolkitHandler.ETableType.EditionTable;
        public DirectOutputToolkitHandler.ETableType GetTableType() => _TableType;
        public bool HasNoBoolEffects()
        {
            foreach (var node in Nodes) {
                if (node is EffectTreeNode effectNode) {
                    if (effectNode.TCS.NoBool) {
                        return true;
                    }
                }
            }
            return false;
        }

        public TableElement TE { get; private set; } = null;

        public TableElementTreeNode(TableElement tableElement, DirectOutputToolkitHandler.ETableType TableType) : base()
        {
            _TableType = TableType;
            TE = tableElement;
            TE.ValueChanged += TE_ValueChanged;
            Refresh();
        }

        ~TableElementTreeNode()
        {
            TE.ValueChanged -= TE_ValueChanged;
            TE = null;
        }

        private void TE_ValueChanged(object sender, TableElementValueChangedEventArgs e)
        {
            TreeView?.Invoke((Action)(() => this.Refresh()));
        }

        public override string ToString()
        {
            if (TE.TableElementType == DirectOutput.TableElementTypeEnum.NamedElement) {
                return $"{TE.Name} : {Nodes.Count} effects";
            }
            return $"{TE.TableElementType} {(char)TE.TableElementType}{TE.Number} : {Nodes.Count} effects";
        }

        internal void Refresh()
        {
            ImageIndex = TE.Value > 0 ? 1 : 0;
            SelectedImageIndex = ImageIndex;
            Text = ToString();
        }

        internal void Rebuild(DirectOutputToolkitHandler handler)
        {
            List<TreeNode> newNodes = new List<TreeNode>();
            foreach (var eff in TE.AssignedEffects) {
                handler.InitEffect(eff, _TableType);
                var effNode = new EffectTreeNode(TE, _TableType, eff.Effect, handler);
                effNode.UpdateFromTableElement(TE);
                newNodes.Add(effNode);
            }

            Nodes.Clear();
            Nodes.AddRange(newNodes.ToArray());
            Refresh();
        }
    }
}
