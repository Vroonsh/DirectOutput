using DofConfigToolWrapper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirectOutputControls
{
    public class DirectOutputPreviewControl : UserControl
    {
        private DirectOutputViewSetup _DirectOutputViewSetup;
        public DirectOutputViewSetup DirectOutputViewSetup{
            get { return _DirectOutputViewSetup;  } private set { _DirectOutputViewSetup = value; _DirectOutputViewSetup?.Init(); }
        }

        public Color BackgroundColor { get; set; } = Color.MidnightBlue;
        public Color AreaDisplayColor { get; set; } = Color.Green;

        private SolidBrush Brush = new SolidBrush(Color.Red);

        public DirectOutputPreviewControl()
        {
            InitializeComponent();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Invalidate();
        }

        public void OnSetupChanged(DirectOutputViewSetup setup)
        {
            if (DirectOutputViewSetup == null) {
                DirectOutputViewSetup = setup;
            }
            if (DirectOutputViewSetup == setup) {
                Refresh();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.Clear(BackgroundColor);
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

            if (DirectOutputViewSetup != null) {
                DirectOutputViewSetup.Resize(e.ClipRectangle);
                Brush.Color = AreaDisplayColor;
                DirectOutputViewSetup.DisplayAreas(e.Graphics, Font, Brush, new Pen(Brush));
                DirectOutputViewSetup.Display(e.Graphics, Font, Brush);
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DirectOutputPreviewControl
            // 
            this.DoubleBuffered = true;
            this.Name = "DirectOutputPreviewControl";
            this.Size = new System.Drawing.Size(454, 715);
            this.ResumeLayout(false);

        }

    }
}
