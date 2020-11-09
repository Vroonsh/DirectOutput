using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectOutput.FX.MatrixFX
{
    public interface IMatrixFlickerEffect
    {
        int Density { get; set; }
        int MinFlickerDurationMs { get; set; }
        int MaxFlickerDurationMs { get; set; }
        int FlickerFadeUpDurationMs { get; set; }
        int FlickerFadeDownDurationMs { get; set; }
    }
}
