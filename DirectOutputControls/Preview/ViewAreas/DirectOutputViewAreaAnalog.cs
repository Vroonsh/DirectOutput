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

        public bool Squarred { get; set; } = true;

        public override bool SetValues(byte[] values)
        {
            if (Value != values[0]) {
                Value = values[0];
                return true;
            }
            return false;
        }

        public override void Display(Graphics gr, Font f, SolidBrush br, Pen p)
        {
            var rect = DisplayRect;
            if (Squarred) {
                var minDim = Math.Min(rect.Width, rect.Height);
                rect.X += (rect.Width - minDim) / 2;
                rect.Y += (rect.Height - minDim) / 2;
                rect.Width = rect.Height = minDim;
            }

            var icon = DofConfigToolResources.GetDofOutputIcon(DofOutput);
            if (icon == null) {
                br.Color = Color.FromArgb(Value, Value, Value);
                gr.FillEllipse(br, rect);
            }
        }
    }
}
