using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectOutputControls
{
    [Serializable]
    public class DirectOutputViewAreaAnalog : DirectOutputViewArea
    {
        public override bool IsVirtual() => false;

        private byte Value = 0;

        public override bool SetValues(byte[] values)
        {
            if (Value != values[0]) {
                Value = values[0];
                return true;
            }
            return false;
        }

        public override void Display(Graphics gr, Font f, SolidBrush br)
        {
            var rect = ComputeDisplayRect();

            var icon = DofConfigToolResources.GetDofOutputIcon(DofOutput);
            if (icon == null) {
                br.Color = Color.FromArgb(Value, Value, Value);
                gr.FillEllipse(br, rect);
            }
        }
    }
}
