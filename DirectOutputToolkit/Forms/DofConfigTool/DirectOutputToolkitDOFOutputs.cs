using DirectOutput.LedControl.Loader;
using DirectOutput.Table;
using DofConfigToolWrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirectOutputToolkit
{
    public partial class DirectOutputToolkitDOFOutputs : Form
    {
        public EditionTableTreeNode TableNode = null;
        public DirectOutputToolkitHandler Handler = null;

        public DirectOutputToolkitDOFOutputs()
        {
            InitializeComponent();
        }

        private void LedControlToolkitDOFOutputs_Load(object sender, EventArgs e)
        {
            comboBoxOutput.DataSource = DofConfigToolOutputs.GetPublicDofOutputNames();
        }

        private void comboBoxOutput_SelectedIndexChanged(object sender, EventArgs e)
        {
            var Toys = Handler.GetToysFromOutput(DofConfigToolOutputs.GetOutput(comboBoxOutput.Text));
            var ColorList = Handler.ColorConfigurations.GetCabinetColorList();

            Dictionary<string, List<TableElement>> TCSDict = new Dictionary<string, List<TableElement>>();
            foreach(var TE in TableNode.EditionTable.TableElements) {
                foreach(var eff in TE.AssignedEffects.Select(AE => AE.Effect).ToArray()) {
                    if (Toys.Contains(eff.GetAssignedToy())) {
                        TableConfigSetting TCS = new TableConfigSetting();
                        TCS.FromEffect(eff);

                        var TCSCommand = TCS.ToConfigToolCommand(ColorList, false);
                        if (!TCSDict.Keys.Any(C=>C.Equals(TCSCommand, StringComparison.InvariantCultureIgnoreCase))) {
                            TCSDict[TCSCommand] = new List<TableElement>();
                        }
                        TCSDict[TCSCommand].Add(TE);
                    }
                }
            }

            richTextBoxDOFCommand.Text = string.Empty;

            foreach(var pair in TCSDict) {
                if (!richTextBoxDOFCommand.Text.IsNullOrEmpty()) {
                    richTextBoxDOFCommand.Text += "/";
                }
                richTextBoxDOFCommand.Text += string.Join("|", pair.Value.Where(TE => !TE.Name.StartsWith(EffectTreeNode.TableElementTestName, StringComparison.InvariantCultureIgnoreCase)).Select(TE => TE.ToString()).ToArray());
                richTextBoxDOFCommand.Text += $" {pair.Key}";
            }

            richTextBoxDOFCommand.Refresh();
        }
    }
}
