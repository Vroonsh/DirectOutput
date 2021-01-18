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
    public class DirectOutputViewAreaRGB : DirectOutputViewAreaUpdatable
    {
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

        [Category("RGB")]
        public bool ShowMatrixGrid { get; set; } = true;

        private byte[] Values = null;

        //Adressable Management
        private Rectangle LedRectangle = new Rectangle();

        public override bool SetValues(byte[] values)
        {
            if (Values == null && FirstUpdate) {
                Values = new byte[values.Length];
            }

            if (FirstUpdate) {
                FirstUpdate = false;
                if (!values.CompareContents(Values)) {
                    values.CopyTo(Values, 0);
                    return true;
                }
            } else {
                if (values.Any(V=>V != 0)) {
                    var minSize = Math.Min(values.Length, Values.Length);
                    for (var num = 0; num < minSize; num++) {
                        Values[num] += values[num];
                    }
                    return true;
                }
            }

            return false;
        }

        private void DisplaySingleValue(Graphics gr, Font f, SolidBrush br)
        {
            if (DofOutputs.Count == 0) return;

            var rect = ComputeDisplayRect();

            var icon = DofConfigToolResources.GetDofOutputIcon(DofOutputs[0]);
            if (icon == null) {
                if (Values != null) {
                    br.Color = Color.FromArgb(Values[0], Values[1], Values[2]);
                } else {
                    br.Color = Color.Black;
                }

                gr.FillEllipse(br, rect);
            }
        }

        private void DisplayLedStrip(Graphics gr, SolidBrush br, Point origin, Point increment, byte[] values)
        {
            LedRectangle.X = origin.X;
            LedRectangle.Y = origin.Y;
            LedRectangle.Width = LedRectangle.Height = Math.Abs(increment.X) > 0 ? Math.Abs(increment.X) - 1 : Math.Abs(increment.Y) - 1;
            for (int i = 0; i < values.Length; i += 3) {
                br.Color = Color.FromArgb(values[i], values[i + 1], values[i + 2]);
                if (!br.Color.IsEmpty) {
                    gr.FillRectangle(br, LedRectangle);
                }
                LedRectangle.X += increment.X;
                LedRectangle.Y += increment.Y;
            }
        }

        private void DisplayMatrix(Graphics gr, SolidBrush br)
        {
            if (MxWidth == 0 || MxHeight == 0) return;

            var center = new Point(DisplayRect.X + DisplayRect.Width / 2, DisplayRect.Y + DisplayRect.Height / 2);

            //Round width & height to ledstrip matrix dimensions
            var width = (DisplayRect.Width / MxWidth) * MxWidth;
            var height = (DisplayRect.Height / MxHeight) * MxHeight;

            //Keep leds as square
            var ledWidth = width / MxWidth;
            var ledHeight = height / MxHeight;
            var ledSize = 0;
            if (ledWidth <= ledHeight) {
                ledSize = ledWidth;
                height = ledSize * MxHeight;
            } else {
                ledSize = ledHeight;
                width = ledSize * MxWidth;
            }

            //Matrix Background
            var origin = new Point(center.X - (int)(width / 2), center.Y - (int)(height / 2));
            LedRectangle.X = origin.X;
            LedRectangle.Y = origin.Y;
            LedRectangle.Width = width;
            LedRectangle.Height = height;
            br.Color = Color.Black;
            gr.FillRectangle(br, LedRectangle);

            if (Values == null) return;

            //Matrix Colors
            LedRectangle.Width = LedRectangle.Height = ShowMatrixGrid ? ledSize - 1 : ledSize;
            for (int x = 0; x < MxWidth; ++x) {
                for (int y = 0; y < MxHeight; ++y) {
                    var index = ((y * MxWidth) + x)*3;
                    br.Color = Color.FromArgb(Values[index], Values[index+1], Values[index+2]);
                    if (br.Color.GetBrightness() != 0.0f) {
                        LedRectangle.X = origin.X + (x * ledSize);
                        LedRectangle.Y = origin.Y + (y * ledSize);
                        gr.FillRectangle(br, LedRectangle);
                    }
                }
            }
        }

        private void DisplayRing(Graphics gr, SolidBrush br)
        {
            var maxDim = Math.Max(MxWidth, MxHeight);
            var center = new Point(DisplayRect.X + DisplayRect.Width / 2, DisplayRect.Y + DisplayRect.Height / 2);
            var radius = (int)(Math.Min(DisplayRect.Width, DisplayRect.Height) / 2 * 0.9f);
            LedRectangle.Width = LedRectangle.Height = radius / (maxDim / 4);
            var startAngleRad = StartAngle * Math.PI / 180.0f;
            for (int i = 0; i < maxDim; ++i) {
                br.Color = Values != null ? Color.FromArgb(Values[i * 3], Values[(i * 3) + 1], Values[(i * 3) + 2]) : Color.Black;
                if (!br.Color.IsEmpty) {
                    var angle = (float)i * (2 * Math.PI) / maxDim;
                    angle = (angle + startAngleRad) % (Math.PI * 2.0f);
                    LedRectangle.X = center.X + (int)((Math.Cos(angle) * radius) + 0.5f);
                    LedRectangle.Y = center.Y + (int)((Math.Sin(angle) * radius) + 0.5f);
                    gr.FillRectangle(br, LedRectangle);
                }
            }
        }

        private void DisplayAdressable(Graphics gr, Font f, SolidBrush br)
        {
            switch (RenderType) {
                case RenderTypeEnum.Matrix:
                    DisplayMatrix(gr, br);
                    break;

                case RenderTypeEnum.Ring:
                    DisplayRing(gr, br);
                    break;

                default:
                    break;
            }
        }

        public override void Display(Graphics gr, Font f, SolidBrush br)
        {
            if (!Visible || !Enabled) return;

            if (ValueType == ValueTypeEnum.SingleValue) {
                DisplaySingleValue(gr, f, br);
            } else {
                DisplayAdressable(gr, f, br);
            }
        }

        public DirectOutputViewAreaRGB() : base() { }

        internal DirectOutputViewAreaRGB(DirectOutputViewAreaRGB src) : base(src)
        {
            ValueType = src.ValueType;
            MxWidth = src.MxWidth;
            MxHeight = src.MxHeight;
            RenderType = src.RenderType;
            StartAngle = src.StartAngle;
            ShowMatrixGrid = src.ShowMatrixGrid;
        }

        internal override DirectOutputViewArea Clone()
        {
            return new DirectOutputViewAreaRGB(this);
        }

    }
}
