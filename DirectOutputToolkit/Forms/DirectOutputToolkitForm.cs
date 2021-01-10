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
    public partial class DirectOutputToolkitForm : Form
    {
        private Settings Settings = new Settings();
        public DirectOutputToolkitForm()
        {
            InitializeComponent();

            Settings = Settings.LoadSettings();
        }

        public bool LoadConfig()
        {
            OpenConfigDialog OCD = new OpenConfigDialog(Settings);
            if (OCD.ShowDialog() == DialogResult.OK) {
                return true;
            } else {
                return false;
            }
        }

        private void SaveSettings()
        {
            Settings.SaveSettings();
        }

        private void DirectOutputToolkitForm_Load(object sender, EventArgs e)
        {
            if (!LoadConfig()) {
                this.Close();
            }
        }

        private void DirectOutputToolkitForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }
    }
}
