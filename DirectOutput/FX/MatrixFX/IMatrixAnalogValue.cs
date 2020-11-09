using DirectOutput.General.Analog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectOutput.FX.MatrixFX
{
    public interface IMatrixAnalogValue
    {
        AnalogAlpha ActiveValue { get; set; }
        AnalogAlpha InactiveValue { get; set; }
    }
}
