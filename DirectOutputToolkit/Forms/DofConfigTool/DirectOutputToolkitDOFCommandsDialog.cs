﻿using DirectOutput.LedControl.Loader;
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
    public partial class DirectOutputToolkitDOFCommandsDialog : Form
    {
        public string[] CommandLines = null;
        public string OutputName => comboBoxOutput.Text;
        public DirectOutputToolkitHandler Handler { get; set; } = null;

        public DirectOutputToolkitDOFCommandsDialog()
        {
            InitializeComponent();
        }
        private void LedControlToolkitDOFCommandsDialog_Load(object sender, EventArgs e)
        {
            comboBoxOutput.DataSource = DofConfigToolOutputs.GetPublicDofOutputNames(false);
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

            if (textBoxDofCommands.Text.Contains(",")) {
                MessageBox.Show("Paste only single output lines directly from DofConfigTool site.\nThis line contains commas, maybe you got a full table DofConfig line", "Invalid DofConfigTool command line", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var lines = textBoxDofCommands.Text.Split('/');

            foreach (var line in lines) {
                if (!line.Equals("0")) {
                    listBoxCommandLines.Items.Add(line);
                }
            }
            listBoxCommandLines.Refresh();
        }

        private void listBoxCommandLines_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete) {
                for (var i = listBoxCommandLines.SelectedIndices.Count - 1; i >= 0; i--) {
                    listBoxCommandLines.Items.RemoveAt(listBoxCommandLines.SelectedIndices[i]);
                }
            }
        }
    }
}
