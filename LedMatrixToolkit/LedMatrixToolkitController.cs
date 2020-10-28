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

namespace LedMatrixToolkit
{
    public class LedMatrixToolkitController : OutputControllerCompleteBase
    {
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
