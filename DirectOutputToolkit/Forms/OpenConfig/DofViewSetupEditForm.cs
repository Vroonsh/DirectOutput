using DirectOutputControls;
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
    public partial class DofViewSetupEditForm : Form
    {
        public string FileName
        {
            get => directOutputViewSetupControl1.FileName;
            set {
                directOutputViewSetupControl1.FileName = value;
            }
        }

        public DofViewSetupEditForm(string fileName)
        {
            InitializeComponent();

            directOutputViewSetupControl1.SetupChanged += directOutputPreviewControl1.OnSetupChanged;
            FileName = fileName;

            directOutputPreviewControl1.DrawViewAreasInfos = checkBoxDrawAreasInfos.Checked;
            directOutputPreviewControl1.Invalidate();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DofViewSetupEditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            while (directOutputViewSetupControl1.Dirty) {
                if (MessageBox.Show("DofConfigTool setup was modified do you want to save before closing ?", "DofConfigTool setup modified", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) {
                    directOutputViewSetupControl1.SaveSetup();
                } else {
                    break;
                }
            }
        }

        private void checkBoxDrawAreasInfos_CheckedChanged(object sender, EventArgs e)
        {
            directOutputPreviewControl1.DrawViewAreasInfos = checkBoxDrawAreasInfos.Checked;
            directOutputPreviewControl1.Invalidate();
        }
    }
}
