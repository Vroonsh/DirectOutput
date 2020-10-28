using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LedMatrixToolkit
{
    public class LedMatrixPreviewControl : Control
    {
        private int _BackboardNbLines = 8;
        public int BackboardNbLines
        {
            get { return _BackboardNbLines; }
            set {
                _BackboardNbLines = value;
                RecomputePartDimensions(false);
            }
        }

        private int _LedsStripDensity = 144;
        public int LedsStripDensity
        {
            get { return _LedsStripDensity; }
            set {
                _LedsStripDensity = value;
                RecomputePartDimensions(false);
            }
        }

        public enum PreviewPart
        {
            BackBoard,
            LeftSide,
            RightSide,
            LeftRing,
            RightRing,
            Undercab,
            Count
        }

        private class PreviewPartDescriptor
        {
            public int LedsStripSize;
            public int NbLedsStrips;
            public RectangleF Area;
            public Rectangle Rect = new Rectangle();
            public int Offset;
            public byte[] Values;
            public bool Cleared = false;
        }

        //Preview Panel Layout in ratio
        private PreviewPartDescriptor[] PreviewParts = new PreviewPartDescriptor[(int)PreviewPart.Count];
        //private Dictionary<PreviewPart, PreviewPartDescriptor> PreviewParts = new Dictionary<PreviewPart, PreviewPartDescriptor>();

        public int TotalNbValues
        {
            get; private set;
        }

        private SolidBrush previewPanelBrush = new SolidBrush(Color.Red);

        private bool _Inited = false;

        private Rectangle _LedRectangle = new Rectangle();

        private Random r = new Random();

        public LedMatrixPreviewControl() : base()
        {
            PreviewParts[(int)PreviewPart.BackBoard] = new PreviewPartDescriptor() { LedsStripSize = _LedsStripDensity, NbLedsStrips = _BackboardNbLines, Area = new RectangleF(0.05f, 0.2f, 0.9f, 0.15f) };
            PreviewParts[(int)PreviewPart.LeftRing] = new PreviewPartDescriptor() { LedsStripSize = _LedsStripDensity / 2, NbLedsStrips = 1, Area = new RectangleF(0.0f, 0.0f, 0.2f, 0.2f) };
            PreviewParts[(int)PreviewPart.RightRing] = new PreviewPartDescriptor() { LedsStripSize = _LedsStripDensity / 2, NbLedsStrips = 1, Area = new RectangleF(0.8f, 0.0f, 0.2f, 0.2f) };
            PreviewParts[(int)PreviewPart.LeftSide] = new PreviewPartDescriptor() { LedsStripSize = _LedsStripDensity, NbLedsStrips = 1, Area = new RectangleF(0.0f, 0.35f, 0.1f, 0.6f) };
            PreviewParts[(int)PreviewPart.RightSide] = new PreviewPartDescriptor() { LedsStripSize = _LedsStripDensity, NbLedsStrips = 1, Area = new RectangleF(0.9f, 0.35f, 0.1f, 0.6f) };
            PreviewParts[(int)PreviewPart.Undercab] = new PreviewPartDescriptor() { LedsStripSize = _LedsStripDensity * 3, NbLedsStrips = 1, Area = new RectangleF(0.3f, 0.4f, 0.4f, 0.5f) };
        }

        public Rectangle SetValues(byte[] values, PreviewPart part)
        {
            var pPart = PreviewParts[(int)part];
            var pValues = values.Skip(pPart.Offset).Take(pPart.Values.Length).ToArray();

            foreach (var val in pValues) {
                if (val != 0) {
                    pValues.CopyTo(pPart.Values, 0);
                    pPart.Cleared = false;
                    return pPart.Rect;
                }
            }

            //All values are 0, if it wasn't cleared, clear it now
            if (!pPart.Cleared) {
                pValues.CopyTo(pPart.Values, 0);
                pPart.Cleared = true;
                return pPart.Rect;
            }

            return new Rectangle();
        }

        private Rectangle InvalidRect;

        public void SetValues(byte[] values)
        {
            if (values.Length == TotalNbValues) {
                InvalidRect = new Rectangle();

                for (PreviewPart part = PreviewPart.BackBoard; part < PreviewPart.Count; part++) {
                    Rectangle pInvalidRect = SetValues(values, part);
                    if (!pInvalidRect.IsEmpty) {
                        if (InvalidRect.IsEmpty) {
                            InvalidRect = pInvalidRect;
                        } else {
                            InvalidRect = Rectangle.Union(InvalidRect, pInvalidRect);
                        }
                    }
                }

                if (!InvalidRect.IsEmpty) {
                    Invalidate(InvalidRect);
                }
            }
        }

        private void RecomputePartDimensions(bool onlyResize)
        {
            if (!onlyResize) {
                PreviewParts[(int)PreviewPart.BackBoard].LedsStripSize = _LedsStripDensity / 2;
                PreviewParts[(int)PreviewPart.BackBoard].NbLedsStrips = _BackboardNbLines;
                PreviewParts[(int)PreviewPart.LeftRing].LedsStripSize = _LedsStripDensity / 2;
                PreviewParts[(int)PreviewPart.RightRing].LedsStripSize = _LedsStripDensity / 2;
                PreviewParts[(int)PreviewPart.LeftSide].LedsStripSize = _LedsStripDensity;
                PreviewParts[(int)PreviewPart.RightSide].LedsStripSize = _LedsStripDensity;
                PreviewParts[(int)PreviewPart.Undercab].LedsStripSize = _LedsStripDensity * 3;
                TotalNbValues = 0;
                foreach (var dim in PreviewParts) {
                    dim.Values = new byte[dim.LedsStripSize * dim.NbLedsStrips * 3];
                    dim.Rect.X = (int)(dim.Area.X * Width);
                    dim.Rect.Y = (int)(dim.Area.Y * Height);
                    dim.Rect.Width = (int)(dim.Area.Width * Width);
                    dim.Rect.Height = (int)(dim.Area.Height * Height);
                    dim.Offset = TotalNbValues;
                    TotalNbValues += dim.Values.Length;
                }
            }

            //Resizing rectangle with Area
            foreach (var dim in PreviewParts) {
                dim.Rect.X = (int)(dim.Area.X * Width);
                dim.Rect.Y = (int)(dim.Area.Y * Height);
                dim.Rect.Width = (int)(dim.Area.Width * Width);
                dim.Rect.Height = (int)(dim.Area.Height * Height);
            }

            _Inited = true;
            Invalidate();
        }

        private void DisplayPreviewAreas(PaintEventArgs e)
        {
            previewPanelBrush.Color = Color.Red;
            foreach (var dim in PreviewParts) {
                e.Graphics.DrawRectangle(new Pen(previewPanelBrush), dim.Rect);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            RecomputePartDimensions(true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (!_Inited)
                return;

            //DisplayPreviewAreas(e);
            DisplayBackboard(e);
            DisplaySide(e, PreviewPart.LeftSide);
            DisplaySide(e, PreviewPart.RightSide);
            DisplayRing(e, PreviewPart.LeftRing);
            DisplayRing(e, PreviewPart.RightRing);
            DisplayUndercab(e);
        }

        private void DrawLedStrip(PaintEventArgs e, Point origin, Point increment, byte[] values)
        {
            _LedRectangle.X = origin.X;
            _LedRectangle.Y = origin.Y;
            _LedRectangle.Width = _LedRectangle.Height = Math.Abs(increment.X) > 0 ? Math.Abs(increment.X )- 1 : Math.Abs(increment.Y) -1;
            for (int i = 0; i < values.Length; i += 3) {
                previewPanelBrush.Color = Color.FromArgb(values[i], values[i+1], values[i+2]);
                e.Graphics.FillRectangle(previewPanelBrush, _LedRectangle);
                _LedRectangle.X += increment.X;
                _LedRectangle.Y += increment.Y;
            }
        }

        private void DisplayUndercab(PaintEventArgs e)
        {
            //Start vertically at top left corner

            var dim = PreviewParts[(int)PreviewPart.Undercab];

            var center = new Point(dim.Rect.X + dim.Rect.Width / 2, dim.Rect.Y + dim.Rect.Height / 2);
            var widthInLeds = dim.LedsStripSize / 6;
            var heightInLeds = dim.LedsStripSize / 3;
            var width = (dim.Rect.Width / widthInLeds) * widthInLeds;
            var height = (dim.Rect.Height / heightInLeds) * heightInLeds;


            //Left vertical (first strip)
            var ledsize = height / heightInLeds;
            var origin = new Point(dim.Rect.X, center.Y - height / 2);
            var increment = new Point(0, ledsize);
            DrawLedStrip(e, origin, increment, dim.Values.Take(heightInLeds*3).ToArray());
            //right vertical (third strip)
            origin.X = dim.Rect.Right - ledsize;
            origin.Y = center.Y + height / 2;
            increment.Y = -increment.Y;
            DrawLedStrip(e, origin, increment, dim.Values.Skip((heightInLeds+widthInLeds)*3).Take(heightInLeds*3).ToArray());

            //Bottom Horizontal (second strip)
            ledsize = width / widthInLeds;
            origin.X = center.X - width / 2;
            origin.Y = dim.Rect.Bottom - ledsize;
            increment.X = ledsize;
            increment.Y = 0;
            DrawLedStrip(e, origin, increment, dim.Values.Skip(heightInLeds * 3).Take(widthInLeds * 3).ToArray());
            //Top Horizontal (fourth strip)
            ledsize = width / widthInLeds;
            origin.X = center.X + width / 2;
            origin.Y = dim.Rect.Y;
            increment.X = -increment.X;
            DrawLedStrip(e, origin, increment, dim.Values.Skip((heightInLeds * 2 + widthInLeds) * 3).Take(widthInLeds * 3).ToArray());
        }

        private void DisplayRing(PaintEventArgs e, PreviewPart ring)
        {
            if (ring == PreviewPart.LeftRing || ring == PreviewPart.RightRing) {
                var dim = PreviewParts[(int)ring];

                var center = new Point(dim.Rect.X + dim.Rect.Width / 2, dim.Rect.Y + dim.Rect.Height / 2);
                var radius = (int)(Math.Min(dim.Rect.Width, dim.Rect.Height) / 2 * 0.9f);
                _LedRectangle.Width = _LedRectangle.Height = radius / (dim.LedsStripSize / 4);
                for (int i = 0; i < dim.LedsStripSize; ++i) {
                    var angle = (float)i / Math.PI * 2;
                    _LedRectangle.X = center.X + (int)((Math.Cos(angle) * radius) + 0.5f);
                    _LedRectangle.Y = center.Y + (int)((Math.Sin(angle) * radius) + 0.5f);
                    previewPanelBrush.Color = Color.FromArgb(dim.Values[i*3], dim.Values[(i*3) + 1], dim.Values[(i*3) + 2]);
                    e.Graphics.FillRectangle(previewPanelBrush, _LedRectangle);
                }
            }
        }

        private void DisplaySide(PaintEventArgs e, PreviewPart side)
        {
            if (side == PreviewPart.LeftSide || side == PreviewPart.RightSide) {
                var dim = PreviewParts[(int)side];

                var center = new Point(dim.Rect.X + dim.Rect.Width / 2, dim.Rect.Y + dim.Rect.Height / 2);
                var height = (dim.Rect.Height / dim.LedsStripSize) * dim.LedsStripSize;
                var increment = new Point(0, dim.Rect.Height / dim.LedsStripSize);
                var origin = new Point(center.X - increment.Y / 2, center.Y - height / 2);
                DrawLedStrip(e, origin, increment, dim.Values);
            }
        }

        private void DisplayBackboard(PaintEventArgs e)
        {
            var dim = PreviewParts[(int)PreviewPart.BackBoard];

            var center = new Point(dim.Rect.X + dim.Rect.Width / 2, dim.Rect.Y + dim.Rect.Height / 2);
            var aspectratio = (float)dim.LedsStripSize / dim.NbLedsStrips;
            var bbwidth = (dim.Rect.Width / dim.LedsStripSize) * dim.LedsStripSize;
            var bbheight = (int)(dim.Rect.Width / aspectratio);
            var origin = new Point(center.X - bbwidth / 2, center.Y - bbheight / 2);
            var increment = new Point(dim.Rect.Width / dim.LedsStripSize, 0);
            for (int line = 0; line < BackboardNbLines; ++line) {
                DrawLedStrip(e, origin, increment, dim.Values.Skip(line * dim.LedsStripSize * 3).Take(dim.LedsStripSize * 3).ToArray());
                origin.Y += bbheight / dim.NbLedsStrips;
            }
        }
    }
}
