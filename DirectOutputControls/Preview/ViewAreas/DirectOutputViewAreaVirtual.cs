using DofConfigToolWrapper;
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
    public class DirectOutputViewAreaVirtual : DirectOutputViewArea
    {
        [Browsable(false)]
        public override string DisplayName => string.Empty;

        public bool Draw { get; set; } = false;

        public override void Display(Graphics gr, Font f, SolidBrush br)
        {
            if (!Enabled || !Visible) return;

            if (Draw) {
                gr.DrawRectangle(new Pen(new SolidBrush(Color.Black)), DisplayRect);
            }
            base.Display(gr, f, br);
        }

        internal DirectOutputViewAreaVirtual(DirectOutputViewAreaVirtual src) : base(src)
        {
            Draw = src.Draw;
        }

        public DirectOutputViewAreaVirtual() : base() { }

        internal override DirectOutputViewArea Clone()
        {
            return new DirectOutputViewAreaVirtual(this);
        }
    }
}
