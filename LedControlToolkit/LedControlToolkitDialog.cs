using DirectOutput;
using DirectOutput.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LedControlToolkit
{
    public partial class LedControlToolkitDialog : Form
    {
        private Pinball Pinball;
        private Settings Settings = new Settings();

        public LedControlToolkitDialog()
        {
            InitializeComponent();

            Settings = Settings.LoadSettings();
        }

        private void LedControlToolkit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Pinball != null) {
                Pinball.Finish();
            }

            Settings.SaveSettings();
        }

        private Random r = new Random();

        private void button1_Click(object sender, EventArgs e)
        {
            TableElement TE = (TableElement)Pinball.Table.TableElements[r.Next(0, Pinball.Table.TableElements.Count-1)];
            TableElementData D = TE.GetTableElementData();
            D.Value = 1;
            Pinball.ReceiveData(D);
        }

        private bool OutputActive = false;

        private void TableElements_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int OrgValue;
            if (e.ColumnIndex >= 0 && e.ColumnIndex < TableElements.ColumnCount) {
                if (e.RowIndex >= 0 && e.RowIndex < TableElements.RowCount) {
                    if (TableElements.Columns[e.ColumnIndex].Name == TEActivate.Name) {
                        OrgValue = 0;
                        int.TryParse(TableElements[TEValue.Name, e.RowIndex].Value.ToString(), out OrgValue);
                        int NewValue = (OrgValue > 0 ? 0 : 1);
                        TableElements[TEValue.Name, e.RowIndex].Value = NewValue;
                    } else if (TableElements.Columns[e.ColumnIndex].Name == TEPulse.Name) {
                        OrgValue = 0;
                        int.TryParse(TableElements[TEValue.Name, e.RowIndex].Value.ToString(), out OrgValue);
                        int PulseValue = (OrgValue > 0 ? 0 : 1);

                        TableElements[TEValue.Name, e.RowIndex].Value = PulseValue;
                        Thread.Sleep(300);
                        TableElements[TEValue.Name, e.RowIndex].Value = OrgValue;
                    }
                }
            }
        }

        private void TableElements_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.ColumnIndex < TableElements.ColumnCount) {
                if (e.RowIndex >= 0 && e.RowIndex < TableElements.RowCount) {
                    if (TableElements.Columns[e.ColumnIndex].Name == TEValue.Name) {
                        object Value = TableElements[TEValue.Name, e.RowIndex].Value;
                        int NumericValue = 0;
                        if (!int.TryParse(Value.ToString(), out NumericValue)) {
                            MessageBox.Show("The value entered is not a valid number.\nWill set the value to 0.", "Invalid value entered", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            TableElements[TEValue.Name, e.RowIndex].Value = 0;
                            NumericValue = 0;
                        }
                        if (OutputActive) {
                            TableElement TE = (TableElement)TableElements.Rows[e.RowIndex].Tag;

                            TableElementData D = TE.GetTableElementData();
                            D.Value = NumericValue;
                            Pinball.ReceiveData(D);
                        }

                        TableElements[TEActivate.Name, e.RowIndex].Value = (NumericValue > 0 ? "Deactivate" : "Activate");
                        TableElements[TEPulse.Name, e.RowIndex].Value = (NumericValue > 0 ? @"Pulse ¯\_/¯" : @"Pulse _/¯\_");

                    }
                }
            }
        }

        private void DisplayTableElements()
        {
            OutputActive = false;
            Pinball.Table.TableElements.Sort((TE1, TE2) => (TE1.TableElementType == TE2.TableElementType ? TE1.Number.CompareTo(TE2.Number) : TE1.TableElementType.CompareTo(TE2.TableElementType)));

            TableElements.Rows.Clear();

            foreach (TableElement TE in Pinball.Table.TableElements) {
                int RowIndex = TableElements.Rows.Add();
                TableElements.Rows[RowIndex].Tag = TE;
                TableElements[TEType.Name, RowIndex].Value = TE.TableElementType.ToString();
                TableElements[TEName.Name, RowIndex].Value = (TE.Name.IsNullOrWhiteSpace() ? "" : TE.Name);
                TableElements[TENumber.Name, RowIndex].Value = TE.Number;
                TableElements[TEValue.Name, RowIndex].Value = TE.Value;
                TableElements[TEActivate.Name, RowIndex].Value = (TE.Value > 0 ? "Deactivate" : "Activate");
                TableElements[TEPulse.Name, RowIndex].Value = (TE.Value > 0 ? @"Pulse ¯\_/¯" : @"Pulse _/¯\_");

            }

            OutputActive = true;


        }

        private void SetupPinball()
        {
            if (Pinball != null) {
                Pinball.Finish();
            }

            LedControlToolkitControllerAutoConfigurator.LastLedControlIniFilename = Settings.LastLedControlIniFilename;

            Pinball = new Pinball();
            Pinball.Setup(GlobalConfigFilename: Settings.LastGlobalConfigFilename, RomName: Settings.LastRomName);

            var controllers = Pinball.Cabinet.OutputControllers.Where(c => c is LedControlToolkitController).ToArray();
            if (controllers.Length > 0) {
                (controllers[0] as LedControlToolkitController).OutputControl = panelPreviewLedMatrix;
            } else {
                var ledControlFilename = Path.GetFileNameWithoutExtension(Settings.LastLedControlIniFilename);
                var ledWizNumber = 30;
                if (ledControlFilename.Contains("directoutputconfig")) {
                    ledWizNumber = Int32.Parse(ledControlFilename.Replace("directoutputconfig", ""));
                }
                var previewController = new LedControlToolkitController() { Name = "LedControlToolkitController", LedWizNumber = ledWizNumber };
                previewController.Init(Pinball.Cabinet);
                Pinball.Cabinet.OutputControllers.Add(previewController);
            }

            panelPreviewLedMatrix.SetupPreviewParts(Pinball.Cabinet, Settings);

            Pinball.Init();
            DisplayTableElements();
        }

        private void RomNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var combo = (sender as ComboBox);
            Settings.LastRomName = combo.Text;
            SetupPinball();
        }

        public bool LoadConfig()
        {
            OpenConfigDialog OCD = new OpenConfigDialog(Settings);
            if (OCD.ShowDialog() == DialogResult.OK) {
                SetupPinball();
                return true;
            } else {
                return false;
            }
        }

        private void LedControlToolkit_Load(object sender, EventArgs e)
        {
            if (!LoadConfig()) {
                this.Close();
            }
        }
    }
}
