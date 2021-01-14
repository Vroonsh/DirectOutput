using DirectOutput.General.Generic;
using DofConfigToolWrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DirectOutputControls
{
    [XmlInclude(typeof(DirectOutputViewAreaVirtual))]
    [XmlInclude(typeof(DirectOutputViewAreaAnalog))]
    [XmlInclude(typeof(DirectOutputViewAreaRGB))]
    [Serializable]
    public abstract class DirectOutputViewArea
    {
        public abstract bool IsVirtual();

        public string Name { get; set; } = string.Empty;

        public bool Enabled { get; set; } = true;

        [Browsable(false)]
        public bool Visible { get; set; } = true;

        public bool Squarred { get; set; } = true;

        public virtual DofConfigToolOutputEnum DofOutput { get; set; } = DofConfigToolOutputEnum.Invalid;

        //        public virtual List<DofConfigToolOutputEnum> ComboDofOutputs { get; private set; } = new List<DofConfigToolOutputEnum>();

        protected RectangleF _Dimensions = RectangleF.FromLTRB(0.0f, 0.0f, 1.0f, 1.0f);

        [Category("Dimensions")]
        public float Left { get { return _Dimensions.X; } set { _Dimensions.X = value.Limit(0.0f, 1.0f); _Dimensions.Width = Math.Min(1.0f - _Dimensions.X, _Dimensions.Width); } }
        [Category("Dimensions")]
        public float Top { get { return _Dimensions.Y; } set { _Dimensions.Y = value.Limit(0.0f, 1.0f); _Dimensions.Height = Math.Min(1.0f - _Dimensions.Y, _Dimensions.Height); } }
        [Category("Dimensions")]
        public float Width { get { return _Dimensions.Width; } set { _Dimensions.Width = value.Limit(0.0f, 1.0f - _Dimensions.X); } }
        [Category("Dimensions")]
        public float Height { get { return _Dimensions.Height; } set { _Dimensions.Height = value.Limit(0.0f, 1.0f - _Dimensions.Y); } }

        [Browsable(false)]
        public ExtList<DirectOutputViewArea> Children = new ExtList<DirectOutputViewArea>();

        protected RectangleF GlobalDimensions;
        protected Rectangle DisplayRect = Rectangle.Empty;

        public void ComputeGlobalDimensions(RectangleF ParentDimensions)
        {
            GlobalDimensions.X = ParentDimensions.X + (_Dimensions.X * ParentDimensions.Width);
            GlobalDimensions.Y = ParentDimensions.Y + (_Dimensions.Y * ParentDimensions.Height);
            GlobalDimensions.Width = ParentDimensions.Width * _Dimensions.Width;
            GlobalDimensions.Height = ParentDimensions.Height * _Dimensions.Height;
            foreach (var area in Children) {
                area.ComputeGlobalDimensions(GlobalDimensions);
            }
        }

        public IEnumerable<DirectOutputViewArea> GetAllAreas()
        {
            yield return this;

            if (Children.Count == 0) {
                yield break;
            }

            foreach (var child in Children.SelectMany(C => C.GetAllAreas())) {
                yield return child;
            }
        }

        public void Resize(RectangleF RefDimensions)
        {
            DisplayRect.X = (int)(GlobalDimensions.X * RefDimensions.Width);
            DisplayRect.Y = (int)(GlobalDimensions.Y * RefDimensions.Height);
            DisplayRect.Width = (int)(GlobalDimensions.Width * RefDimensions.Width);
            DisplayRect.Height = (int)(GlobalDimensions.Height * RefDimensions.Height);

            foreach (var area in Children) {
                area.Resize(RefDimensions);
            }
        }

        protected Rectangle ComputeDisplayRect()
        {
            Rectangle rect = DisplayRect;
            if (Squarred) {
                var minDim = Math.Min(rect.Width, rect.Height);
                rect.X += (rect.Width - minDim) / 2;
                rect.Y += (rect.Height - minDim) / 2;
                rect.Width = rect.Height = minDim;
            }
            return rect;
        }

        public void DisplayArea(Graphics gr, Font f, SolidBrush br, Pen p)
        {
            if (!Enabled || !Visible) return;
            var outputs = DofOutput.ToString();
            //if (ComboDofOutputs.Count > 0) {
            //    outputs += "," + string.Join(",", ComboDofOutputs);
            //}
            var dispName = IsVirtual() ? "" : $"{Name} [{outputs}]";
            var sizeName = gr.MeasureString(dispName, f);
            gr.DrawString(dispName, f, br, new Point(Math.Min(DisplayRect.X, (int)(gr.ClipBounds.Width - sizeName.Width)), Math.Max(0, DisplayRect.Y - (int)(f.Height * 1.05f))));
            gr.DrawRectangle(p, DisplayRect);

            foreach (var area in Children) {
                area.DisplayArea(gr, f, br, p);
            }
        }

        public virtual bool SetValues(byte[] values) => false;

        public virtual void Display(Graphics gr, Font f, SolidBrush br)
        {
            if (!Enabled || !Visible) return;

            foreach (var area in Children) {
                area.Display(gr, f, br);
            }
        }

        internal void RemoveArea(DirectOutputViewArea area)
        {
            if (Children.Contains(area)) {
                Children.Remove(area);
            } else {
                foreach (var child in Children) {
                    child.RemoveArea(area);
                }
            }
        }

        internal bool HitTest(Point coords) => DisplayRect.Contains(coords);
    }

    [Serializable]
    public class DirectOutputViewAreaVirtual : DirectOutputViewArea
    {
        [Browsable(false)]
        public override DofConfigToolOutputEnum DofOutput => DofConfigToolOutputEnum.Invalid;

        public override bool IsVirtual() => true;

        public bool Draw { get; set; } = false;

        public override void Display(Graphics gr, Font f, SolidBrush br)
        {
            if (!Enabled || !Visible) return;

            if (Draw) {
                gr.DrawRectangle(new Pen(new SolidBrush(Color.Black)), DisplayRect);
            }
            base.Display(gr, f, br);
        }
    }

}
