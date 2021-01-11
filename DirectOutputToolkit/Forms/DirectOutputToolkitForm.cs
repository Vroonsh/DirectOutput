using DirectOutputControls;
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
    public partial class DirectOutputToolkitForm : Form
    {
        private Settings Settings = new Settings();

        private DofConfigToolSetup DofConfigToolSetup = null;
        private DirectOutputViewSetup DofViewSetup = null;
        private DirectOutputToolkitPreviewForm PreviewForm = new DirectOutputToolkitPreviewForm();

        private DirectOutputToolkitHandler Handler = new DirectOutputToolkitHandler();

        public DirectOutputToolkitForm()
        {
            InitializeComponent();

            Settings = Settings.LoadSettings();
        }

        public bool LoadConfig()
        {
            OpenConfigDialog OCD = new OpenConfigDialog(Settings);
            if (OCD.ShowDialog() == DialogResult.OK) {

                DofConfigToolSetup = DofConfigToolSetup.ReadFromXml(Settings.LastDofConfigSetup);
                DofViewSetup = DirectOutputViewSetupSerializer.ReadFromXml(Settings.LastDofViewSetup);

                PreviewForm.Show(this);
                PreviewForm.PreviewControl.OnSetupChanged(DofViewSetup);

                Handler.DofConfigToolSetup = DofConfigToolSetup;
                Handler.DofViewSetup = DofViewSetup;
                Handler.SetupPinball();

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
            Handler.FinishPinball();
            SaveSettings();
        }

        #region Pinball
        #endregion

        #region Main Menu
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Coucou");
        }
        #endregion
    }
}
