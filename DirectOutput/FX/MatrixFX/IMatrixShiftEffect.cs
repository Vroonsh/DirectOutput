using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectOutput.FX.MatrixFX
{
    public interface IMatrixShiftEffect
    {
        MatrixShiftDirectionEnum ShiftDirection { get; set; }
        float ShiftSpeed { get; set; }
        float ShiftAcceleration { get; set; }
    }
}
