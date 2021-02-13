using DirectOutput.Cab.Toys;
using DirectOutput.Table;
using DirectOutputControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirectOutputToolkit
{
    public class StaticEffectsTreeNode : TreeNode
    {
        public Table Table { get; set; }
        private DirectOutputToolkitHandler.ETableType _TableType = DirectOutputToolkitHandler.ETableType.EditionTable;

        public StaticEffectsTreeNode(Table table, DirectOutputToolkitHandler.ETableType tableType) : base()
        {
            Table = table;
            _TableType = tableType;
            Text = ToString();
        }

        public override string ToString()
        {
            return $"Static effects [{Nodes.Count} effects]";
        }

        internal void Refresh()
        {
            Text = ToString();
        }

        internal void Rebuild(DirectOutputToolkitHandler Handler, IToy[] ToysFilter = null)
        {
            Nodes.Clear();
            foreach (var eff in Table.AssignedStaticEffects) {
                Handler.InitEffect(eff, _TableType);
                if (ToysFilter == null || ToysFilter.Contains(eff.Effect.GetAssignedToy())) {
                    var effNode = new EffectTreeNode(null, _TableType, eff.Effect, Handler);
                    Nodes.Add(effNode);
                }
            }

            Refresh();
        }

    }
}
