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
    public partial class DofConfigSetupEditForm : Form
    {
        public string FileName
        {
            get => dofConfigToolSetupControl1.FileName;
            set {
                dofConfigToolSetupControl1.FileName = value;
            }
        }

        public DofConfigSetupEditForm(string filename)
        {
            InitializeComponent();
            FileName = filename;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DofConfigSetupEditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            while (dofConfigToolSetupControl1.Dirty) {
                if (MessageBox.Show("DofConfigTool setup was modified do you want to save before closing ?", "DofConfigTool setup modified", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) {
                    dofConfigToolSetupControl1.SaveSetup();
                } else {
                    break;
                }
            }
        }
    }
}
