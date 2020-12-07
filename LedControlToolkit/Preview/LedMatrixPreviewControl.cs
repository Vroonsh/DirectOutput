using DirectOutput.Cab;
using DirectOutput.Cab.Toys.Hardware;
using DirectOutput.Cab.Toys.Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LedControlToolkit
{
    public class LedMatrixPreviewControl : Panel
    {
        public enum PreviewType
        {
            Invalid,
            Matrix,
            Ring
        }

        public enum ConfigToolOutput
        {
            [Description("PF Back Effects MX")]
            PFBackEffectsMX,
            [Description("PF Left Effects MX")]
            PFLeftEffectsMX,
            [Description("PF Right Effects MX")]
            PFRightEffectsMX,
            [Description("RGB Undercab Complex MX")]
            RGBUndercabComplexMX
        }

        private class PreviewPartDescriptor
        {
            public LedStrip LedStrip;
            public PreviewType Type = PreviewType.Invalid;
            public RectangleF Area;
            public Rectangle Rect = new Rectangle();
            public int Offset;
            public byte[] Values;
        }

        public int TotalNbValues => _Inited ? PreviewParts.Values.Sum(d => d.LedStrip.NumberOfOutputs) : 0;

        //Preview Panel Layout in ratio
        private Dictionary<string, PreviewPartDescriptor> PreviewParts = new Dictionary<string, PreviewPartDescriptor>();
        private List<string> MissingLedstrips = new List<string>();
        private List<string> MissingAreas = new List<string>();

        private SolidBrush previewPanelBrush = new SolidBrush(Color.Red);
        private Font previewPanelFont = new Font(FontFamily.GenericSansSerif, 10.0f);

        private bool _Inited = false;

        private Rectangle _LedRectangle = new Rectangle();

        public bool ShowMatrixGrid = false;
        public bool ShowPreviewAreas = false;

        public LedMatrixPreviewControl() : base()
        {
            this.DoubleBuffered = true;
        }

        public void SetupPreviewParts(Cabinet cabinet, Settings settings)
        {
            PreviewParts.Clear();
            MissingLedstrips.Clear();
            MissingAreas.Clear();

            var ledStrips = cabinet.Toys.Where(T => T is LedStrip).Select(T => T as LedStrip).ToArray();

            foreach (var ledstrip in ledStrips) {
                var areas = settings.LedPreviewAreas.Where(A => A.Name.Equals(ledstrip.Name, StringComparison.InvariantCultureIgnoreCase)).ToArray();

                PreviewParts[ledstrip.Name.ToLower()] = new PreviewPartDescriptor() {
                    LedStrip = ledstrip,
                    Offset = (ledstrip.FirstLedNumber - 1) * 3,
                    Values = new byte[ledstrip.NumberOfOutputs],

                    Area = areas.Length > 0 ? new RectangleF(areas[0].Left, areas[0].Top, areas[0].Width, areas[0].Height) : new RectangleF(),
                    Type = areas.Length > 0 ? areas[0].PreviewType : PreviewType.Invalid
                };

                if (areas.Length == 0) {
                    MissingLedstrips.Add(ledstrip.Name);
                }
            }

            foreach (var area in settings.LedPreviewAreas) {
                if (!PreviewParts.Values.Any(P => P.LedStrip.Name.Equals(area.Name, StringComparison.InvariantCultureIgnoreCase))){
                    MissingAreas.Add(area.Name);
                }
            }

            RecomputePreviewParts();
            _Inited = true;
        }

        public void SetValues(byte[] values)
        {
            foreach(var part in PreviewParts.Values) {
                var pValues = values.Skip(part.Offset).Take(part.Values.Length).ToArray();

                if (pValues.CompareContents(part.Values)) {
                    continue;
                }

                pValues.CopyTo(part.Values, 0);
                Invalidate(part.Rect);
            }
        }

        private void DisplayAreaMissmatch(PaintEventArgs e)
        {
            previewPanelBrush.Color = Color.Red;
            var y = 10;
            if (MissingLedstrips.Count > 0) {
                e.Graphics.DrawString("Ledstrips w/o area", previewPanelFont, previewPanelBrush, new Point(10, y));
                y += previewPanelFont.Height;
                foreach (var ls in MissingLedstrips) {
                    e.Graphics.DrawString(ls, previewPanelFont, previewPanelBrush, new Point(20, y));
                    y += previewPanelFont.Height;
                }
            }

            if (MissingAreas.Count > 0) {
                e.Graphics.DrawString("Areas w/o ledstrip", previewPanelFont, previewPanelBrush, new Point(10, y));
                y += previewPanelFont.Height;
                foreach (var area in MissingAreas) {
                    e.Graphics.DrawString(area, previewPanelFont, previewPanelBrush, new Point(20, y));
                    y += previewPanelFont.Height;
                }
            }
        }

        private void DisplayPreviewAreas(PaintEventArgs e)
        {
            if (!ShowPreviewAreas) return;

            previewPanelBrush.Color = Color.Green;
            foreach (var dim in PreviewParts.Values) {
                var sizeName = e.Graphics.MeasureString(dim.LedStrip.Name, previewPanelFont);
                e.Graphics.DrawString(dim.LedStrip.Name, previewPanelFont, previewPanelBrush, new Point(Math.Min(dim.Rect.X, Width - (int)sizeName.Width), Math.Max(0, dim.Rect.Y - (int)(previewPanelFont.Height * 1.05f))));
                e.Graphics.DrawRectangle(new Pen(previewPanelBrush), dim.Rect);
            }
        }

        protected void RecomputePreviewParts()
        {
            //Resizing rectangle with Area
            foreach (var dim in PreviewParts.Values) {
                dim.Rect.X = (int)(dim.Area.X * Width);
                dim.Rect.Y = (int)(dim.Area.Y * Height);
                dim.Rect.Width = (int)(dim.Area.Width * Width);
                dim.Rect.Height = (int)(dim.Area.Height * Height);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            RecomputePreviewParts();
            this.Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (!_Inited)
                return;
            e.Graphics.Clear(Color.MidnightBlue);
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

            DisplayPreviewAreas(e);
            foreach (var part in PreviewParts.Values) {
                if (part.Type == PreviewType.Ring) {
                    DisplayRing(e, part);
                } else {
                    DisplayMatrix(e, part);
                }
            }

            DisplayAreaMissmatch(e);
        }

        private void DrawLedStrip(PaintEventArgs e, Point origin, Point increment, byte[] values)
        {
            _LedRectangle.X = origin.X;
            _LedRectangle.Y = origin.Y;
            _LedRectangle.Width = _LedRectangle.Height = Math.Abs(increment.X) > 0 ? Math.Abs(increment.X )- 1 : Math.Abs(increment.Y) -1;
            for (int i = 0; i < values.Length; i += 3) {
                previewPanelBrush.Color = Color.FromArgb(values[i], values[i+1], values[i+2]);
                if (!previewPanelBrush.Color.IsEmpty) {
                    e.Graphics.FillRectangle(previewPanelBrush, _LedRectangle);
                }
                _LedRectangle.X += increment.X;
                _LedRectangle.Y += increment.Y;
            }
        }

        private Color GetColorValue(PreviewPartDescriptor pDesc, int x, int y)
        {
            int valueIdx = pDesc.LedStrip.OutputMappingTable[x, y];

            switch (pDesc.LedStrip.ColorOrder) {
                case RGBOrderEnum.BGR:
                    return Color.FromArgb(pDesc.Values[valueIdx + 2], pDesc.Values[valueIdx + 1], pDesc.Values[valueIdx]);
                case RGBOrderEnum.BRG:
                    return Color.FromArgb(pDesc.Values[valueIdx + 2], pDesc.Values[valueIdx + 1], pDesc.Values[valueIdx]);
                case RGBOrderEnum.GBR:
                    return Color.FromArgb(pDesc.Values[valueIdx + 2], pDesc.Values[valueIdx], pDesc.Values[valueIdx + 1]);
                case RGBOrderEnum.GRB:
                    return Color.FromArgb(pDesc.Values[valueIdx + 1], pDesc.Values[valueIdx], pDesc.Values[valueIdx + 2]);
                case RGBOrderEnum.RBG:
                    return Color.FromArgb(pDesc.Values[valueIdx], pDesc.Values[valueIdx + 2], pDesc.Values[valueIdx + 1]);
                case RGBOrderEnum.RGB:
                    return Color.FromArgb(pDesc.Values[valueIdx], pDesc.Values[valueIdx + 1], pDesc.Values[valueIdx + 2]);
            }

            return Color.Black;
        }

        private void DisplayMatrix(PaintEventArgs e, PreviewPartDescriptor pDesc)
        {
            if (pDesc.Type != PreviewType.Matrix) return;

            var center = new Point(pDesc.Rect.X + pDesc.Rect.Width / 2, pDesc.Rect.Y + pDesc.Rect.Height / 2);

            //Round width & height to ledstrip matrix dimensions
            var width = (pDesc.Rect.Width / pDesc.LedStrip.Width) * pDesc.LedStrip.Width;
            var height = (pDesc.Rect.Height / pDesc.LedStrip.Height) * pDesc.LedStrip.Height;

            //Keep leds as square
            var ledWidth = width / pDesc.LedStrip.Width;
            var ledHeight = height / pDesc.LedStrip.Height;
            var ledSize = 0;
            if (ledWidth <= ledHeight) {
                ledSize = ledWidth;
                height = ledSize * pDesc.LedStrip.Height;
            } else {
                ledSize = ledHeight;
                width = ledSize * pDesc.LedStrip.Width;
            }

            //Matrix Background
            var origin = new Point(center.X - (int)(width / 2), center.Y - (int)(height / 2));
            _LedRectangle.X = origin.X;
            _LedRectangle.Y = origin.Y;
            _LedRectangle.Width = width;
            _LedRectangle.Height = height;
            previewPanelBrush.Color = Color.Black;
            e.Graphics.FillRectangle(previewPanelBrush, _LedRectangle);

            //Matrix Colors
            _LedRectangle.Width = _LedRectangle.Height = ShowMatrixGrid ? ledSize - 1 : ledSize;
            for (int col = 0; col < pDesc.LedStrip.Width; ++col) {
                for(int line = 0; line < pDesc.LedStrip.Height; ++line) {
                    var color = GetColorValue(pDesc, col, line);
                    if (color.GetBrightness() != 0.0f) {
                        _LedRectangle.X = origin.X + (col * ledSize);
                        _LedRectangle.Y = origin.Y + (line * ledSize);
                        previewPanelBrush.Color = color;
                        e.Graphics.FillRectangle(previewPanelBrush, _LedRectangle);
                    }
                }
            }
        }

        private void DisplayRing(PaintEventArgs e, PreviewPartDescriptor pDesc)
        {
            if (pDesc.Type != PreviewType.Ring || pDesc.LedStrip.Height != 1) return;
            
            var center = new Point(pDesc.Rect.X + pDesc.Rect.Width / 2, pDesc.Rect.Y + pDesc.Rect.Height / 2);
            var radius = (int)(Math.Min(pDesc.Rect.Width, pDesc.Rect.Height) / 2 * 0.9f);
            _LedRectangle.Width = _LedRectangle.Height = radius / (pDesc.LedStrip.Width/ 4);
            previewPanelBrush.Color = Color.Black;
            e.Graphics.FillRectangle(previewPanelBrush, pDesc.Rect);
            for (int i = 0; i < pDesc.LedStrip.Width; ++i) {
                var angle = (float)i / Math.PI * 2;
                _LedRectangle.X = center.X + (int)((Math.Cos(angle) * radius) + 0.5f);
                _LedRectangle.Y = center.Y + (int)((Math.Sin(angle) * radius) + 0.5f);
                previewPanelBrush.Color = Color.FromArgb(pDesc.Values[i*3], pDesc.Values[(i*3) + 1], pDesc.Values[(i*3) + 2]);
                if (!previewPanelBrush.Color.IsEmpty) {
                    e.Graphics.FillRectangle(previewPanelBrush, _LedRectangle);
                }
            }
        }

        internal void OnClose()
        {
            previewPanelFont.Dispose();
            previewPanelBrush.Dispose();
        }
    }
}
