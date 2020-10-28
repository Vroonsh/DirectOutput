using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LedMatrixToolkit
{
    internal class LedMatrixPreviewControl : Control
    {
        public int BackboardNbLines = 8;
        public int BackboardNbLedsPerLine = 72;

        public enum PreviewPart
        {
            BackBoard,
            LeftRing,
            RightRing,
            LeftSide,
            RightSide,
            Undercab,
            Count
        }

        //Preview Panel Layout in ratio
        private Dictionary<PreviewPart, RectangleF> PreviewPartsRectangles = new Dictionary<PreviewPart, RectangleF>();

        private SolidBrush previewPanelBrush = new SolidBrush(Color.Red);
        private Rectangle previewRectangle = new Rectangle();

        private Random r = new Random();

        public LedMatrixPreviewControl() : base()
        {
            PreviewPartsRectangles[PreviewPart.BackBoard] = new RectangleF(0.05f, 0.15f, 0.9f, 0.15f);
            PreviewPartsRectangles[PreviewPart.LeftRing] = new RectangleF(0.0f, 0.0f, 0.15f, 0.15f);
            PreviewPartsRectangles[PreviewPart.RightRing] = new RectangleF(0.85f, 0.0f, 0.15f, 0.15f);
            PreviewPartsRectangles[PreviewPart.LeftSide] = new RectangleF(0.0f, 0.35f, 0.05f, 0.75f);
            PreviewPartsRectangles[PreviewPart.RightSide] = new RectangleF(0.95f, 0.35f, 0.05f, 0.75f);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int smallDim = Math.Min(Width, Height);
            int offsetX = (Width - smallDim) / 2;
            int offsetY = (Height - smallDim) / 2;

            foreach (var rect in PreviewPartsRectangles.Values) {
                previewRectangle.X = offsetX + (int)(rect.X * smallDim);
                previewRectangle.Y = offsetY + (int)(rect.Y * smallDim);
                previewRectangle.Width = (int)(rect.Width * smallDim);
                previewRectangle.Height = (int)(rect.Height * smallDim);
                e.Graphics.DrawRectangle(new Pen(previewPanelBrush), previewRectangle);
            }

            /*

            int stepX = (int)(Width / BackboardNbLedsPerLine);
            int size = (int)((Width - (BackboardNbLedsPerLine * 2)) / BackboardNbLedsPerLine);

            previewRectangle.Width = previewRectangle.Height = size;

            for (int line = 0; line < BackboardNbLines; ++line) {
                previewRectangle.Y = line * stepX;
                for (int col = 0; col < BackboardNbLedsPerLine; col++) {
                    previewPanelBrush.Color = Color.FromArgb(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255));
                    previewRectangle.X = col * stepX;
                    e.Graphics.FillRectangle(previewPanelBrush, previewRectangle);
                }
            }
            */
        }
    }
}
