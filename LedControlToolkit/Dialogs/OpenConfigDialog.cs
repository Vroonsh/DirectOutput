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

namespace LedControlToolkit
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

            var viewsetup = new DirectOutputViewSetup();
            viewsetup.ViewAreas.Add(new DirectOutputViewArea() { Name = "Area1", DofOutputs = { DofConfigToolOutputEnum.Invalid } });
            viewsetup.ViewAreas[0].Children.Add(new DirectOutputViewAreaIcon() { Name = "Icon1", DofOutputs = { DofConfigToolOutputEnum.StartButton, DofConfigToolOutputEnum.Beacon }, Dimensions = RectangleF.FromLTRB(0.1f, 0.2f, 0.5f, 0.5f) });
            viewsetup.ViewAreas[0].Children[0].Children.Add(new DirectOutputViewAreaIcon() { Name = "Icon2", DofOutputs = { DofConfigToolOutputEnum.Strobe, DofConfigToolOutputEnum.Coin }, Dimensions = RectangleF.FromLTRB(0.5f, 0.5f, 1.0f, 1.0f) });
            viewsetup.ViewAreas.Add(new DirectOutputViewAreaRGB() { Name = "RGB1", DofOutputs = { DofConfigToolOutputEnum.PFBackEffectsMX, DofConfigToolOutputEnum.PFBackFlashersMX }, Dimensions = RectangleF.FromLTRB(0.5f, 0.5f, 1.0f, 1.0f) });
            viewsetup.ViewAreas[1].Children.Add(new DirectOutputViewArea() { Name = "Area2", DofOutputs = { DofConfigToolOutputEnum.Invalid }, Dimensions = RectangleF.FromLTRB(0.25f, 0.25f, 0.5f, 0.5f) });
            this.directOutputPreviewControl1.DirectOutputViewSetup = viewsetup;

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
            Pinball p = new Pinball();
            p.Setup(GlobalConfigFilename);

            var setup = DofConfigToolSetup.ReadFromXml("./DofToolkit/DofConfigSetup_DofToolkit.dofsetup");
            var handler = new DofConfigToolFilesHandler() { RootDirectory = "DofToolkit\\setups", DofSetup = setup };
            handler.UpdateConfigFiles(true);

            DirectOutputPreviewController controller = new DirectOutputPreviewController();
            controller.DofSetup = setup;
            controller.Name = "DirectOutputPreviewController_1";
            controller.PreviewControl = this.directOutputPreviewControl1;
            controller.Setup(p);
            p.Finish();

            SaveData();
        }
    }
}
