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

        public Action PreviewBackColorChanged = null;

        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            numericUpDownMinDuration.Value = Settings.EffectMinDurationMs;
            numericUpDownMatrixMinDuration.Value = Settings.EffectRGBMinDurationMs;

            checkBoxDofAutoUpdate.Checked = Settings.DofFilesAutoUpdate;

            checkBoxAutoSave.Checked = Settings.AutoSaveOnQuit;

            labelBckColor.BackColor = Settings.PreviewBackgroundColor;
        }

        private void checkBoxDofAutoUpdate_CheckedChanged(object sender, EventArgs e)
        {
            Settings.DofFilesAutoUpdate = checkBoxDofAutoUpdate.Checked;
        }

        private void checkBoxAutoSave_CheckedChanged(object sender, EventArgs e)
        {
            Settings.AutoSaveOnQuit = checkBoxAutoSave.Checked;
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
            this.Close();
        }

        private void buttonBckColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            if (dlg.ShowDialog() == DialogResult.OK) {
                Settings.PreviewBackgroundColor = dlg.Color;
                labelBckColor.BackColor = dlg.Color;
                if (PreviewBackColorChanged != null) {
                    PreviewBackColorChanged.Invoke();
                }
            }
        }
    }
}
