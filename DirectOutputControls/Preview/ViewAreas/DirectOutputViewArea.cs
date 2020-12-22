using DofConfigToolWrapper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace DirectOutputControls
{
    public class DirectOutputViewArea
    {
        public string Name { get; set; } = string.Empty;

        public bool Enabled { get; set; } = true;

        public bool Visible { get; set; } = true;

        public List<DofConfigToolOutputEnum> DofOutputs { get; private set; } = new List<DofConfigToolOutputEnum>();

        public RectangleF Dimensions { get; set; } = RectangleF.FromLTRB(0.0f, 0.0f, 1.0f, 1.0f);

        public List<DirectOutputViewArea> Children { get; private set; } = new List<DirectOutputViewArea>();

        protected byte[] Values;
        protected RectangleF GlobalDimensions;
        protected Rectangle DisplayRect = Rectangle.Empty;

        public void ComputeGlobalDimensions(RectangleF ParentDimensions)
        {
            GlobalDimensions.X = ParentDimensions.X + (Dimensions.X * ParentDimensions.Width);
            GlobalDimensions.Y = ParentDimensions.Y + (Dimensions.Y * ParentDimensions.Height);
            GlobalDimensions.Width = ParentDimensions.Width * Dimensions.Width;
            GlobalDimensions.Height = ParentDimensions.Height * Dimensions.Height;
            foreach(var area in Children) {
                area.ComputeGlobalDimensions(GlobalDimensions);
            }
        }

        public void AssignToDictionary(Dictionary<DofConfigToolOutputEnum, List<DirectOutputViewArea>> Dict)
        {
            foreach (var output in DofOutputs) {
                if (!Dict.Keys.Contains(output)) {
                    Dict[output] = new List<DirectOutputViewArea>();
                }
                Dict[output].Add(this);
            }

            foreach (var area in Children) {
                area.AssignToDictionary(Dict);
            }
        }

        public void Resize(RectangleF RefDimensions)
        {
            DisplayRect.X = (int)(GlobalDimensions.X * RefDimensions.Width);
            DisplayRect.Y = (int)(GlobalDimensions.Y * RefDimensions.Height);
            DisplayRect.Width = (int)(GlobalDimensions.Width * RefDimensions.Width);
            DisplayRect.Height = (int)(GlobalDimensions.Height * RefDimensions.Height);

            foreach(var area in Children) {
                area.Resize(RefDimensions);
            }
        }

        public void DisplayArea(Graphics gr, Font f, SolidBrush br, Pen p)
        {
            if (!Enabled) return;
            var dispName = $"{Name} [{string.Join(",", DofOutputs)}]";
            var sizeName = gr.MeasureString(dispName, f);
            gr.DrawString(dispName, f, br, new Point(Math.Min(DisplayRect.X, (int)(gr.ClipBounds.Width - sizeName.Width)), Math.Max(0, DisplayRect.Y - (int)(f.Height * 1.05f))));
            gr.DrawRectangle(p, DisplayRect);

            foreach (var area in Children) {
                area.DisplayArea(gr, f, br, p);
            }
        }

        public virtual bool SetValues(byte[] values) => false;
        public virtual void Display(Graphics gr, Font f, SolidBrush br, Pen p)
        {
            foreach(var area in Children) {
                area.Display(gr, f, br, p);
            }
        }
    }
}
