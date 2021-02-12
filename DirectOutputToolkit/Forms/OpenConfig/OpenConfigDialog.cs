using DirectOutput;
using DirectOutputControls;
using DofConfigToolWrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DirectOutputToolkit
{
    public partial class OpenConfigDialog : Form
    {
        private Settings Settings = new Settings();

        private DofConfigToolSetup DofSetup = new DofConfigToolSetup();
        private DirectOutputViewSetup ViewSetup = new DirectOutputViewSetup();

        public bool ForceDofConfigToolUpdate { get; private set; } = false;

        public OpenConfigDialog(Settings Settings = null)
        {
            if (Settings != null)
            {
                this.Settings = Settings;
            }
            InitializeComponent();

            this.OpenDofConfigSetupFileDialog.Filter = DofConfigToolSetupControl.FileFilter;
            this.OpenDofConfigSetupFileDialog.DefaultExt = DofConfigToolSetupControl.FileDefaultExt;
            this.OpenDofViewSetupFileDialog.Filter = DirectOutputViewSetupControl.FileFilter;
            this.OpenDofViewSetupFileDialog.DefaultExt = DirectOutputViewSetupControl.FileDefaultExt;

            LoadData();
        }

        private void LoadData()
        {
            DofConfigSetupFilename = Settings.LastDofConfigSetup;
            DofViewSetupFilename = Settings.LastDirectOutputViewSetup;

            DofConfigSetupFilenameComboBox.Items.Clear();
            DofConfigSetupFilenameComboBox.Items.AddRange(Settings.DofConfigSetups.ToArray());

            DofViewSetupFilenameComboBox.Items.Clear();
            DofViewSetupFilenameComboBox.Items.Add("");
            DofViewSetupFilenameComboBox.Items.AddRange(Settings.DirectOutputViewSetups.ToArray());
        }

        private void SaveData()
        {
            Settings.LastDofConfigSetup = DofConfigSetupFilename;
            Settings.LastDirectOutputViewSetup = DofViewSetupFilename;

            Settings.DofConfigSetups.Remove(DofConfigSetupFilename);
            Settings.DofConfigSetups.Insert(0, DofConfigSetupFilename);

            Settings.DirectOutputViewSetups.Remove(DofViewSetupFilename);
            Settings.DirectOutputViewSetups.Insert(0, DofViewSetupFilename);
        }


        private void DofConfigSetupFileSelectButton_Click(object sender, EventArgs e)
        {
            if (OpenDofConfigSetupFileDialog.ShowDialog(this) == DialogResult.OK) {
                DofConfigSetupFilename = OpenDofConfigSetupFileDialog.FileName;
            }
        }



        private void DofViewSetupFileSelectButton_Click(object sender, EventArgs e)
        {
            if (OpenDofViewSetupFileDialog.ShowDialog(this) == DialogResult.OK) {
                DofViewSetupFilename = OpenDofViewSetupFileDialog.FileName;
            }
        }

        public string DofConfigSetupFilename
        {
            get { return DofConfigSetupFilenameComboBox.Text; }
            set { DofConfigSetupFilenameComboBox.Text = value; }
        }

        public string DofViewSetupFilename
        {
            get { return DofViewSetupFilenameComboBox.Text; }
            set { DofViewSetupFilenameComboBox.Text = value; }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        private void buttonDofConfigSetupEdit_Click(object sender, EventArgs e)
        {
            DofConfigSetupEditForm editForm = new DofConfigSetupEditForm(DofConfigSetupFilename);
            editForm.ShowDialog(this);
            DofConfigSetupFilename = editForm.FileName;
        }

        private void buttonDofViewSetupEdit_Click(object sender, EventArgs e)
        {
            DofViewSetupEditForm editForm = new DofViewSetupEditForm(DofViewSetupFilename);
            editForm.ShowDialog(this);
            DofViewSetupFilename = editForm.FileName;
        }

        private void checkBoxForceDownload_CheckedChanged(object sender, EventArgs e)
        {
            ForceDofConfigToolUpdate = checkBoxForceDownload.Checked;
        }
    }
}
