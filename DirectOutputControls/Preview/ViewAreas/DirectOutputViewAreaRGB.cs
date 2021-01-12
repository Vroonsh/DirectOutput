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

        private byte[] Values = null;

        public override bool SetValues(byte[] values)
        {
            if (Values == null) {
                Values = new byte[values.Length];
            }

            if (!values.CompareContents(Values)) {
                values.CopyTo(Values, 0);
                return true;
            } 
            return false;
        }

        private void DisplaySingleValue(Graphics gr, Font f, SolidBrush br)
        {
            var rect = ComputeDisplayRect();

            var icon = DofConfigToolResources.GetDofOutputIcon(DofOutput);
            if (icon == null) {
                if (Values != null) {
                    br.Color = Color.FromArgb(Values[0], Values[1], Values[2]);
                } else {
                    br.Color = Color.Black;
                }

                gr.FillEllipse(br, rect);
            }
        }

        public override void Display(Graphics gr, Font f, SolidBrush br)
        {
            if (ValueType == ValueTypeEnum.SingleValue) {
                DisplaySingleValue(gr, f, br);
            }
        }
    }
}
