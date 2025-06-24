using DirectOutput;
using DirectOutputControls;
using DofConfigToolWrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace DirectOutputToolkit
{
    public partial class OpenConfigDialog : Form
    {
        private Settings Settings = new Settings();

        private DofConfigToolSetup DofSetup = new DofConfigToolSetup();
        private DirectOutputViewSetup ViewSetup = new DirectOutputViewSetup();

        public DofConfigToolFilesHandler.EDofConfigToolConnectMethod DofConfigToolConnectMethod { get; set; } = DofConfigToolFilesHandler.EDofConfigToolConnectMethod.PullVBScript;
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
            this.OpenDofConfigSetupFileDialog.InitialDirectory = Application.StartupPath;
            this.OpenDofViewSetupFileDialog.Filter = DirectOutputViewSetupControl.FileFilter;
            this.OpenDofViewSetupFileDialog.DefaultExt = DirectOutputViewSetupControl.FileDefaultExt;
            this.OpenDofViewSetupFileDialog.InitialDirectory = Application.StartupPath;

            LoadData();
        }

        private void LoadData()
        {
            DofConfigToolConnectMethod = Settings.DofConfigToolConnectMethod;
            ForceDofConfigToolUpdate = Settings.ForceDofConfigToolUpdate;

            DofConfigSetupFilename = Settings.LastDofConfigSetup;
            DofViewSetupFilename = Settings.LastDirectOutputViewSetup;

            checkBoxForceDownload.Checked = ForceDofConfigToolUpdate;

            ConnectionMethodComboBox.Items.Clear();
            foreach(var enumVal in Enum.GetValues(typeof(DofConfigToolFilesHandler.EDofConfigToolConnectMethod)))
                ConnectionMethodComboBox.Items.Add(enumVal);
            ConnectionMethodComboBox.SelectedIndex = (int)DofConfigToolConnectMethod;

            DofConfigSetupFilenameComboBox.Items.Clear();
            DofConfigSetupFilenameComboBox.Items.AddRange(Settings.DofConfigSetups.ToArray());

            DofViewSetupFilenameComboBox.Items.Clear();
            DofViewSetupFilenameComboBox.Items.Add("");
            DofViewSetupFilenameComboBox.Items.AddRange(Settings.DirectOutputViewSetups.ToArray());
        }

        private void SaveData()
        {
            Settings.ForceDofConfigToolUpdate = ForceDofConfigToolUpdate;
            Settings.DofConfigToolConnectMethod = DofConfigToolConnectMethod;

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

        private void ConnectionMethodComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox combo) {
                DofConfigToolConnectMethod = (DofConfigToolFilesHandler.EDofConfigToolConnectMethod)combo.SelectedIndex;
                checkBoxForceDownload.Visible = (DofConfigToolConnectMethod == DofConfigToolFilesHandler.EDofConfigToolConnectMethod.InternalHttpRequest);
            }
        }
    }
}
