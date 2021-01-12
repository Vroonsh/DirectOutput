using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DirectOutputControls
{
    [Serializable]
    public class DirectOutputViewAreaAnalog : DirectOutputViewArea
    {
        public override bool IsVirtual() => false;

        [XmlIgnore]
        [Category("Analog")]
        public Color BackColor { get; set; } = Color.White;

        [Browsable(false)]
        [XmlElement("BackColor")]
        public int BackColorAsArgb
        {
            get { return BackColor.ToArgb(); }
            set { BackColor = Color.FromArgb(value); }
        }

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
                br.Color = Color.FromArgb((byte)((float)BackColor.R * Value), (byte)((float)BackColor.G * Value), (byte)((float)BackColor.B * Value));
                gr.FillEllipse(br, rect);
            }
        }
    }
}
