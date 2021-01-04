using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectOutputControls
{
    [Serializable]
    public class DirectOutputViewAreaRGB : DirectOutputViewArea
    {
        public override bool IsVirtual() => false;

        public enum ValueTypeEnum
        {
            SingleValue,
            Adressable,
        }

        public enum RenderTypeEnum
        {
            Invalid,
            Simple,
            Matrix,
            Ring,
            Frame
        }

        [Category("RGB")]
        public ValueTypeEnum ValueType { get; set; } = ValueTypeEnum.SingleValue;

        [Category("RGB")]
        public int MxWidth { get; set; } = 0;
        [Category("RGB")]
        public int MxHeight { get; set; } = 0;

        [Category("RGB")]
        public RenderTypeEnum RenderType { get; set; } = RenderTypeEnum.Simple;

        [Category("RGB")]
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
