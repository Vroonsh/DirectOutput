using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LedMatrixToolkit
{
    public partial class OpenConfigDialog : Form
    {
        private Settings Settings = new Settings();

        public OpenConfigDialog(Settings Settings = null)
        {
            if (Settings != null)
            {
                this.Settings = Settings;
            }
            InitializeComponent();

            LoadData();

        }

        private void LoadData()
        {
            GlobalConfigFilename = Settings.LastGlobalConfigFilename;
            LedControlIniFilename = Settings.LastLedControlIniFilename;

            GlobalConfigFilenameComboBox.Items.Clear();
            GlobalConfigFilenameComboBox.Items.AddRange(Settings.GlobalConfigFilenames.ToArray());

            LedControlIniFilenameComboBox.Items.Clear();
            LedControlIniFilenameComboBox.Items.Add("");
            LedControlIniFilenameComboBox.Items.AddRange(Settings.LedControlIniFileNames.ToArray());
        }

        private void SaveData()
        {
            Settings.LastGlobalConfigFilename = GlobalConfigFilename;
            Settings.LastLedControlIniFilename = LedControlIniFilename;

            Settings.GlobalConfigFilenames.Remove(GlobalConfigFilename);
            Settings.GlobalConfigFilenames.Insert(0, GlobalConfigFilename);

            Settings.LedControlIniFileNames.Remove(LedControlIniFilename);
            Settings.LedControlIniFileNames.Insert(0, LedControlIniFilename);
        }


        private void GlobalConfigFileSelectButton_Click(object sender, EventArgs e)
        {
            SelectGlobalConfigFile();
        }



        private void TableFileSelectButton_Click(object sender, EventArgs e)
        {
            SelectTableFile();
        }


        private void SelectTableFile()
        {
            if (OpenLedControlIniFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                LedControlIniFilename = OpenLedControlIniFileDialog.FileName;
            }
        }
        private void SelectGlobalConfigFile()
        {

            if (OpenGlobalConfigFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                GlobalConfigFilename = OpenGlobalConfigFileDialog.FileName;
            }
        }



        public string GlobalConfigFilename
        {
            get { return GlobalConfigFilenameComboBox.Text; }
            set { GlobalConfigFilenameComboBox.Text = value; }
        }

        public string LedControlIniFilename
        {
            get { return LedControlIniFilenameComboBox.Text; }
            set { LedControlIniFilenameComboBox.Text = value; }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            
            SaveData();
        }
    }
}
