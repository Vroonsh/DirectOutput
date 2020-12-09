using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LedControlToolkit
{
    public partial class LedControlToolkitDOFCommandsDialog : Form
    {
        public string[] AvailableToys = new string[0];
        public string[] CommandLines = null;
        public string ToyName => comboBoxOutput.Text;

        public LedControlToolkitDOFCommandsDialog()
        {
            InitializeComponent();
        }
        private void LedControlToolkitDOFCommandsDialog_Load(object sender, EventArgs e)
        {
            comboBoxOutput.DataSource = AvailableToys;
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            List<string> lines = new List<string>();
            foreach(var item in listBoxCommandLines.Items) {
                lines.Add(item.ToString());
            }
            CommandLines = lines.ToArray();
            Close();
        }

        private void textBoxDofCommands_TextChanged(object sender, EventArgs e)
        {
            //Parse Lines from entered DOF Commands
            listBoxCommandLines.Items.Clear();

            var lines = textBoxDofCommands.Text.Split('/');

            foreach (var line in lines) {
                listBoxCommandLines.Items.Add(line);
            }
            listBoxCommandLines.Refresh();
        }

    }
}
