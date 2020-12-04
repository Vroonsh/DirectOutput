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

namespace LedControlToolkit
{
    public class TableElementTreeNode : TreeNode, ITableElementTreeNode
    {
        public virtual TableElement GetTableElement() => TE;
        private LedControlToolkitHandler.ETableType _TableType = LedControlToolkitHandler.ETableType.EditionTable;
        public LedControlToolkitHandler.ETableType GetTableType() => _TableType;

        public TableElement TE = null;

        public TableElementTreeNode(TableElement tableElement, LedControlToolkitHandler.ETableType TableType) : base()
        {
            _TableType = TableType;
            TE = tableElement;
            Refresh();
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
            ImageIndex = TE.GetTableElementData().Value > 0 ? 1 : 0;
            SelectedImageIndex = ImageIndex;
            Text = ToString();
        }

        internal void Rebuild(LedControlToolkitHandler handler)
        {
            Nodes.Clear();
            foreach(var eff in TE.AssignedEffects) {
                eff.Init(handler.GetTable(_TableType));
                var effNode = new EffectTreeNode(TE, _TableType, eff.Effect, handler.LedControlConfigData);
                effNode.UpdateFromTableElement(TE);
                Nodes.Add(effNode);
            }
            Refresh();
        }
    }
}
