using DofConfigToolWrapper;
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
    [XmlInclude(typeof(DirectOutputViewAreaAnalog))]
    [XmlInclude(typeof(DirectOutputViewAreaRGB))]
    [Serializable]
    public class DirectOutputViewAreaUpdatable : DirectOutputViewArea
    {
        public List<DofConfigToolOutputEnum> DofOutputs { get; set; } = new List<DofConfigToolOutputEnum>();

        public bool Squarred { get; set; } = true;

        [Browsable(false)]
        public override string DisplayName => $"{Name} [{string.Join(",", DofOutputs)}]";

        internal bool HasOutput(DofConfigToolOutputEnum output) => (DofOutputs.Any(O => O == output));

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

        public virtual bool SetValues(byte[] values) => false;

        internal DirectOutputViewAreaUpdatable(DirectOutputViewAreaUpdatable src) : base(src)
        {
            DofOutputs.AddRange(src.DofOutputs);
            Squarred = src.Squarred;
        }

        public DirectOutputViewAreaUpdatable() : base() { }

        internal override DirectOutputViewArea Clone()
        {
            return new DirectOutputViewAreaUpdatable(this);
        }

    }
}
