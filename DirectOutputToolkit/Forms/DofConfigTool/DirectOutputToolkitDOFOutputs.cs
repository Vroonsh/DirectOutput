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
            comboBoxOutput.DataSource = DofConfigToolOutputs.GetPublicDofOutputNames(false);
        }

        private void UpdateOutputCommands()
        {
            var Toys = Handler.GetToysFromOutput(DofConfigToolOutputs.GetOutput(comboBoxOutput.Text));
            var ColorList = Handler.ColorConfigurations.GetCabinetColorList();

            Dictionary<TableConfigSetting, List<TableElement>> TCSDict = new Dictionary<TableConfigSetting, List<TableElement>>();

            foreach (var eff in TableNode.EditionTable.AssignedStaticEffects.Select(AE => AE.Effect).ToArray()) {
                if (Toys.Contains(eff.GetAssignedToy())) {
                    TableConfigSetting TCS = new TableConfigSetting();
                    TCS.FromEffect(eff);
                    if (!TCSDict.Keys.Any(T => T == TCS)) {
                        TCSDict[TCS] = new List<TableElement>();
                    }
                }
            }

            foreach (var TE in TableNode.EditionTable.TableElements) {
                foreach (var eff in TE.AssignedEffects.Select(AE => AE.Effect).ToArray()) {
                    if (Toys.Contains(eff.GetAssignedToy())) {
                        TableConfigSetting TCS = new TableConfigSetting();
                        TCS.FromEffect(eff);
                        if (!TCSDict.Keys.Any(T => T == TCS)) {
                            TCSDict[TCS] = new List<TableElement>();
                        }
                        TCSDict[TCS].Add(TE);
                    }
                }
            }

            richTextBoxDOFCommand.Text = string.Empty;

            foreach (var pair in TCSDict) {
                if (!richTextBoxDOFCommand.Text.IsNullOrEmpty()) {
                    richTextBoxDOFCommand.Text += "/";
                }
                if (pair.Value.Count > 0) {
                    richTextBoxDOFCommand.Text += string.Join("|", pair.Value.Where(TE => !TE.Name.StartsWith(EffectTreeNode.TableElementTestName, StringComparison.InvariantCultureIgnoreCase)).Select(TE => TE.ToString()).ToArray());
                } else {
                    richTextBoxDOFCommand.Text += pair.Key.OutputControl == OutputControlEnum.FixedOn ? "ON" : "OFF";
                }
                richTextBoxDOFCommand.Text += $" {pair.Key.ToConfigToolCommand(ColorList, exportTE: false, fullRangeIntensity: checkBoxFullRangeIntensity.Checked)}";
            }

            richTextBoxDOFCommand.Refresh();
        }

        private void comboBoxOutput_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateOutputCommands();
        }

        private void checkBoxFullRangeIntensity_CheckedChanged(object sender, EventArgs e)
        {
            UpdateOutputCommands();
        }
    }
}
