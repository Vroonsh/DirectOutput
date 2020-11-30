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
    public class EditionTableTreeNode : TreeNode
    {
        public EditionTableTreeNode(Table Table) : base()
        {
            EditionTable = Table;
            Text = ToString();
        }

        public override string ToString()
        {
            return $"{EditionTable?.TableName} [{EditionTable?.RomName}] [{Nodes.Count} table elements]";
        }

        public Table EditionTable { get; set; }

        internal void Refresh()
        {
            Text = ToString();
        }

        internal void Rebuild(LedControlToolkitHandler Handler)
        {
            Refresh();
        }
    }
}
