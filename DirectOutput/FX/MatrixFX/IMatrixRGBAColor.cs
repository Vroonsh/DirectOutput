using DirectOutput.General.Color;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectOutput.FX.MatrixFX
{
    public interface IMatrixRGBAColor
    {
        RGBAColor ActiveColor { get; set; }
        RGBAColor InactiveColor { get; set; }
    }

    public interface IMatrixRGBAColor2 : IMatrixRGBAColor
    {
        RGBAColor ActiveColor2 { get; set; }
    }
}
