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
    public partial class SettingsForm : Form
    {
        public Settings Settings { get; set; } = null;

        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            numericUpDownPulseDuration.Value = Settings.PulseDurationMs;
            numericUpDownMinDuration.Value = Settings.EffectMinDurationMs;
            numericUpDownMatrixMinDuration.Value = Settings.EffectRGBMinDurationMs;

            checkBoxDofAutoUpdate.Checked = Settings.DofFilesAutoUpdate;

            checkBoxAutoSave.Checked = Settings.AutoSaveOnQuit;
        }

        private void checkBoxDofAutoUpdate_CheckedChanged(object sender, EventArgs e)
        {
            Settings.DofFilesAutoUpdate = checkBoxDofAutoUpdate.Checked;
        }

        private void checkBoxAutoSave_CheckedChanged(object sender, EventArgs e)
        {
            Settings.AutoSaveOnQuit = checkBoxAutoSave.Checked;
        }

        private void numericUpDownPulseDuration_ValueChanged(object sender, EventArgs e)
        {
            Settings.PulseDurationMs = (int)numericUpDownPulseDuration.Value;
        }

        private void numericUpDownMinDuration_ValueChanged(object sender, EventArgs e)
        {
            Settings.EffectMinDurationMs = (int)numericUpDownMinDuration.Value;
        }

        private void numericUpDownMatrixMinDuration_ValueChanged(object sender, EventArgs e)
        {
            Settings.EffectRGBMinDurationMs = (int)numericUpDownMatrixMinDuration.Value;
        }

        private void buttonSaveSettings_Click(object sender, EventArgs e)
        {
            Settings.SaveSettings();
        }

    }
}
