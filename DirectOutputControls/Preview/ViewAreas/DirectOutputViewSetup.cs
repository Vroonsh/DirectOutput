using DirectOutput.Cab;
using DofConfigToolWrapper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DirectOutputControls
{
    public class DirectOutputViewSetup
    {
        public List<DirectOutputViewArea> ViewAreas { get; private set; } = new List<DirectOutputViewArea>();

        [XmlIgnore]
        public Dictionary<DofConfigToolOutputEnum, List<DirectOutputViewArea>> ViewAreasDictionary { get; private set; } = new Dictionary<DofConfigToolOutputEnum, List<DirectOutputViewArea>>();

        public void Init()
        {
            ViewAreasDictionary.Clear();
            RectangleF baseRect = RectangleF.FromLTRB(0.0f, 0.0f, 1.0f, 1.0f);
            foreach (var area in ViewAreas) {
                area.ComputeGlobalDimensions(baseRect);
                area.AssignToDictionary(ViewAreasDictionary);
            }
        }

        public void Resize(RectangleF bounds)
        {
            foreach(var area in ViewAreas) {
                area.Resize(bounds);
            }
        }

        public void DisplayAreas(Graphics gr, Font f, SolidBrush br, Pen p)
        {
            foreach (var area in ViewAreas) {
                area.DisplayArea(gr, f, br, p);
            }
        }

        internal DirectOutputViewArea[] GetViewAreas(DofConfigToolOutputEnum output, Type viewType, bool enabledOnly = true)
        {
            if (ViewAreasDictionary.Keys.Contains(output)) {
                return ViewAreasDictionary[output].Where(V => V.GetType() == viewType && (!enabledOnly || V.Enabled)).ToArray();
            }
            return null;
        }
    }
}
