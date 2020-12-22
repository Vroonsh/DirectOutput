using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectOutputControls
{
    public class DirectOutputViewAreaRGB : DirectOutputViewArea
    {
        public enum ValueTypeEnum
        {
            Single,
            Adressable,
        }

        public enum RenderTypeEnum
        {
            Invalid,
            Matrix,
            Ring,
            Frame
        }

        public ValueTypeEnum ValueType { get; set; } = ValueTypeEnum.Single;

        public int Width { get; set; } = 0;
        public int Height { get; set; } = 0;

        public RenderTypeEnum RenderType { get; set; } = RenderTypeEnum.Invalid;

        public int StartAngle { get; set; } = 90;

        public override bool SetValues(byte[] values)
        {
            if (Values.Length != values.Length || !values.CompareContents(Values)) {
                values = new byte[values.Length];
                values.CopyTo(Values, 0);
                return true;
            } 
            return false;
        }

        public override void Display(Graphics gr, Font f, SolidBrush br, Pen p)
        {

        }
    }
}
