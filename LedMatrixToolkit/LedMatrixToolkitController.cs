using DirectOutput.Cab.Out;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedMatrixToolkit
{
    public class LedMatrixToolkitController : OutputControllerCompleteBase
    {
        protected override void ConnectToController()
        {
        }

        protected override void DisconnectFromController()
        {
        }

        protected override int GetNumberOfConfiguredOutputs()
        {
            return 24;
        }

        protected override void UpdateOutputs(byte[] OutputValues)
        {
        }

        protected override bool VerifySettings()
        {
            return true;
        }
    }
}
