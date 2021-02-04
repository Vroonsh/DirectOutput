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
            get { return _DirectOutputViewSetup;  }
            private set {
                _DirectOutputViewSetup = value;
                _DirectOutputViewSetup?.Init();
                if (SetupSet != null) {
                    SetupSet.Invoke(_DirectOutputViewSetup);
                }
            }
        }

        public DofConfigToolResources DofResources { get; private set; } = new DofConfigToolResources();

        public Color BackgroundColor { get; set; } = Color.MidnightBlue;
        public Color AreaDisplayColor { get; set; } = Color.Green;

        private SolidBrush Brush = new SolidBrush(Color.Red);

        public Action<DirectOutputViewSetup> SetupSet = null;

        public bool DrawViewAreasInfos { get; set; } = false;

        private ToolTip AreasTooltip = new ToolTip();

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
                DirectOutputViewSetup.Display(e.Graphics, Font, Brush);
                if (DrawViewAreasInfos) {
                    Brush.Color = AreaDisplayColor;
                    DirectOutputViewSetup.DisplayAreas(e.Graphics, Font, Brush, new Pen(Brush));
                }
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
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DirectOutputPreviewControl_MouseMove);
            this.ResumeLayout(false);

        }

        public void OnControllerRefresh()
        {
            try {
                if (Parent != null && Parent.Created && !(Parent.Disposing || Parent.IsDisposed)) {
                    Parent?.Invoke((Action)(() =>
                    {
                        try {
                            if (this.Created && !this.IsDisposed && !this.Disposing) {
                                this.Refresh();
                            }
                        } catch { }
                    }));
                }
            } catch { }
        }

        private void DirectOutputPreviewControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (_DirectOutputViewSetup == null) return;
            var areas = _DirectOutputViewSetup.HitTest(e.Location);
            AreasTooltip.UseFading = true;
            if (areas.Length > 0) {
                AreasTooltip.Show(string.Join("\n", areas.Where(A => !(A is DirectOutputViewAreaVirtual) && A.Enabled && A.Visible).Select(A=>$"{A.DisplayName}").ToArray()), this, e.Location.X + 10, e.Location.Y);
            } else {
                AreasTooltip.Hide(this);
            }
        }
    }
}
