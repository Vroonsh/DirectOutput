using DirectOutput;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LedMatrixToolkit
{
    public partial class LedMatrixToolkit : Form
    {
        private Pinball Pinball;
        private Settings Settings = new Settings();

        public LedMatrixToolkit()
        {
            InitializeComponent();

            Settings = Settings.LoadSettings();

            panelPreview.BackboardNbLines = Settings.BackboardNbLines;
            panelPreview.BackboardNbLedsPerLine = (Settings.BackboardDensity == BackboardDensity.LPM_144 ? 144 : Settings.BackboardDensity == BackboardDensity.LPM_60 ? 60 : 30) / 2;

            Pinball = new Pinball();
            var globalconfigpath = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "LedMatrixToolkit", "GlobalConfig.xml");
            Pinball.Setup(globalconfigpath, RomName: "afm"/*Settings.LastRomName*/);
            Pinball.Init();
        }

        private void LedMatrixToolkit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Pinball != null) {
                Pinball.Finish();
            }
            Settings.SaveSettings();
        }
    }
}
