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
            return $"Edition Table [{EditionTable.RomName}]";
        }

        public Table EditionTable { get; set; }
    }
}
