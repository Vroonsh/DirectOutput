using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DirectOutputControls
{
    [Serializable]
    public class DirectOutputViewAreaAnalog : DirectOutputViewAreaUpdatable
    {
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

        [XmlIgnore]
        public override int OutputSize => 1;

        private int Value = -1;

        public override bool SetValues(byte[] values)
        {
            if (Value != values[0] || (FirstUpdate == false && values[0] != 0)) {
                if (FirstUpdate) {
                    Value = values[0];
                    FirstUpdate = false;
                } else {
                    Value += values[0];
                }
                return true;
            }
            return false;
        }

        public override void Display(Graphics gr, Font f, SolidBrush br)
        {
            if (DofOutputs.Count == 0) return;

            var rect = ComputeDisplayRect();

            var icon = DofConfigToolResources.GetDofOutputIcon(DofOutputs[0]);
            if (Value < 0) {
                br.Color = BackColor;
            } else {
                br.Color = Color.FromArgb((byte)((float)BackColor.R * Value / 255.0f), (byte)((float)BackColor.G * Value / 255.0f), (byte)((float)BackColor.B * Value / 255.0f));
            }
            if (icon == null) {
                gr.FillEllipse(br, rect);
            } else {
                ColorMatrix matrix = new ColorMatrix();
                matrix.Matrix00 = br.Color.R / 255.0f;
                matrix.Matrix11 = br.Color.G / 255.0f;
                matrix.Matrix22 = br.Color.B / 255.0f;
                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                gr.DrawImage(icon, rect, 0, 0, icon.Width, icon.Height, GraphicsUnit.Pixel, attributes);
            }
        }

        public DirectOutputViewAreaAnalog() : base() { }

        internal DirectOutputViewAreaAnalog(DirectOutputViewAreaAnalog src) : base(src)
        {
            BackColor = src.BackColor;
        }

        internal override DirectOutputViewArea Clone()
        {
            return new DirectOutputViewAreaAnalog(this);
        }

    }
}
