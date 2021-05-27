using DirectOutput.FX;
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

        private class ExportTCS
        {
            public IEffect Effect;
            public TableConfigSetting TCS;
            public string DofCommand;
        }

        private void UpdateOutputCommands()
        {
            var output = DofConfigToolOutputs.GetOutput(comboBoxOutput.Text);
            var Toys = Handler.GetToysFromOutput(output);
            var ColorList = Handler.ColorConfigurations.GetCabinetColorList();

            Dictionary<ExportTCS, List<TableElement>> TCSDict = new Dictionary<ExportTCS, List<TableElement>>();

            foreach (var eff in TableNode.EditionTable.AssignedStaticEffects.Select(AE => AE.Effect).ToArray()) {
                var effToy = eff.GetAssignedToy();
                if (Toys.Contains(effToy)) {
                    var TCS = Handler.TCSFromEffect(eff);
                    var dofCommand = Handler.ToConfigToolCommand(TCS, eff, exportTE: false, fullRangeIntensity: checkBoxFullRangeIntensity.Checked);
                    var matchingExport = TCSDict.Keys.FirstOrDefault(T => T.DofCommand.Equals(dofCommand, StringComparison.InvariantCultureIgnoreCase));
                    if (matchingExport == null) {
                        matchingExport = new ExportTCS() { Effect = eff, TCS = TCS, DofCommand = dofCommand };
                        TCSDict[matchingExport] = new List<TableElement>();
                    }
                }
            }

            foreach (var TE in TableNode.EditionTable.TableElements) {
                if (!TE.Name.StartsWith(EffectTreeNode.TableElementTestName, StringComparison.InvariantCultureIgnoreCase)) {
                    foreach (var eff in TE.AssignedEffects.Select(AE => AE.Effect).ToArray()) {
                        var effToy = eff.GetAssignedToy();
                        if (Toys.Contains(effToy)) {
                            var TCS = Handler.TCSFromEffect(eff);
                            var dofCommand = Handler.ToConfigToolCommand(TCS, eff, exportTE: false, fullRangeIntensity: checkBoxFullRangeIntensity.Checked);
                            var matchingExport = TCSDict.Keys.FirstOrDefault(T => T.DofCommand.Equals(dofCommand, StringComparison.InvariantCultureIgnoreCase));
                            if (matchingExport == null) {
                                matchingExport = new ExportTCS() { Effect = eff, TCS = TCS, DofCommand = dofCommand };
                                TCSDict[matchingExport] = new List<TableElement>();
                            }
                            if (!TCSDict[matchingExport].Contains(TE)) {
                                TCSDict[matchingExport].Add(TE);
                            }
                        }
                    }
                }
            }

            richTextBoxDOFCommand.Text = string.Empty;

            if (checkBoxSortEffects.Checked) {
                var sortedTCSDict = TCSDict.OrderBy(x => x.Key.TCS.WaitDurationMs);
                TCSDict = sortedTCSDict.ToDictionary(x => x.Key, x => x.Value);
            }

            foreach (var pair in TCSDict) {
                if (!richTextBoxDOFCommand.Text.IsNullOrEmpty()) {
                    richTextBoxDOFCommand.Text += "/";
                }
                if (pair.Value.Count > 0) {
                    richTextBoxDOFCommand.Text += string.Join("|", pair.Value.Where(TE => !TE.Name.StartsWith(EffectTreeNode.TableElementTestName, StringComparison.InvariantCultureIgnoreCase)).Select(TE => TE.ToString()).ToArray());
                } else {
                    richTextBoxDOFCommand.Text += pair.Key.TCS.OutputControl == OutputControlEnum.FixedOn ? "ON" : "OFF";
                }
                richTextBoxDOFCommand.Text += $" {pair.Key.DofCommand}";
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

        private void checkBoxSortEffects_CheckedChanged(object sender, EventArgs e)
        {
            UpdateOutputCommands();
        }
    }
}
