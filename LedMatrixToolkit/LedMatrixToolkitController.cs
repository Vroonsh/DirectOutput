using DirectOutput;
using DirectOutput.Cab;
using DirectOutput.Cab.Out;
using DirectOutput.Cab.Out.LW;
using DirectOutput.Cab.Toys.LWEquivalent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using DirectOutput.Cab.Toys.Hardware;

namespace LedMatrixToolkit
{
    /// <summary>
    /// This class is used to detect and configure LedWiz output controllers automatically.
    /// </summary>
    public class LedMatrixToolkitControllerAutoConfigurator : IAutoConfigOutputController
    {
        public static string LastLedControlIniFilename = string.Empty;

        #region IAutoConfigOutputController Member

        /// <summary>
        /// This method detects and configures LedMatrixToolkitController output controllers automatically.
        /// </summary>
        /// <param name="Cabinet">The cabinet object to which the automatically detected IOutputController objects are added if necessary.</param>
        public void AutoConfig(Cabinet Cabinet)
        {
            List<LedMatrixToolkitController> Controllers = new List<LedMatrixToolkitController>(Cabinet.OutputControllers.Where(OC => OC is LedMatrixToolkitController).Select(LW => (LedMatrixToolkitController)LW));

            if (Controllers.Count == 0) {
                var ledControlFilename = Path.GetFileNameWithoutExtension(LastLedControlIniFilename);
                var ledWizNumber = 30;
                if (ledControlFilename.Contains("directoutputconfig")) {
                    ledWizNumber = Int32.Parse(ledControlFilename.Replace("directoutputconfig", ""));
                }
                var previewController = new LedMatrixToolkitController() { Name = "LedMatrixToolkitController", LedWizNumber = ledWizNumber };
                Controllers.Add(previewController);
                Cabinet.OutputControllers.Add(previewController);
            }

            var C = Controllers[0];
            Log.Write("Detected LedMatrixToolkitController Nr. {0} with name {1}".Build(C.LedWizNumber, C.Name));

            var ledWizEquivalents = Cabinet.Toys.Where(T => T is LedWizEquivalent && ((LedWizEquivalent)T).LedWizNumber == C.LedWizNumber).ToArray();
            if (ledWizEquivalents.Length == 0) {
                //Create needed LWE
                LedWizEquivalent LWE = new LedWizEquivalent();
                LWE.LedWizNumber = C.LedWizNumber;
                LWE.Name = "{0} Equivalent".Build(C.Name);

                for (int i = 1; i < 33; i++) {
                    LedWizEquivalentOutput LWEO = new LedWizEquivalentOutput() { OutputName = "{0}\\{0}.{1:00}".Build(C.Name, i), LedWizEquivalentOutputNumber = i };
                    LWE.Outputs.Add(LWEO);
                }

                if (!Cabinet.Toys.Contains(LWE.Name)) {
                    Cabinet.Toys.Add(LWE);
                    Log.Write("Added LedwizEquivalent Nr. {0} with name {1} for Ledwiz Nr. {2}".Build(LWE.LedWizNumber, LWE.Name, C.LedWizNumber));
                }
            } else {
                //Override Ledstrips with LedMatrixToolkitController name
                var ledStrips = Cabinet.Toys.Where(T => T is LedStrip);
                foreach (var ledstrip in ledStrips) {
                    (ledstrip as LedStrip).OutputControllerName = C.Name;
                }
            }

        }

        #endregion
    }

    public class LedMatrixToolkitController : OutputControllerCompleteBase
    {
        public int LedWizNumber = 0;

        [XmlIgnoreAttribute]
        public LedMatrixPreviewControl OutputControl = null;

        protected override void ConnectToController()
        {
        }

        protected override void DisconnectFromController()
        {
        }

        protected override int GetNumberOfConfiguredOutputs()
        {
            if (OutputControl != null) {
                return OutputControl.TotalNbValues;
            }
            return 1;
        }

        protected override void UpdateOutputs(byte[] OutputValues)
        {
            if (OutputControl != null) {
                OutputControl.SetValues(OutputValues);
            }
        }

        protected override bool VerifySettings()
        {
            return true;
        }
    }
}
