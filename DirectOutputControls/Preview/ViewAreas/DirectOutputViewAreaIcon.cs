using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectOutputControls
{
    public class DirectOutputViewAreaIcon : DirectOutputViewArea
    {

        public override bool SetValues(byte[] values)
        {
            return false;
        }

        public override void Display(Graphics gr, Font f, SolidBrush br, Pen p)
        {

        }
    }
}
