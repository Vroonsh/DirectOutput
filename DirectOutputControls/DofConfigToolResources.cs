using DofConfigToolWrapper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectOutputControls
{
    public class DofConfigToolResources
    {
        private static readonly Dictionary<DofConfigToolOutputEnum, Image> DofOutputIcons = new Dictionary<DofConfigToolOutputEnum, Image>(){
            { DofConfigToolOutputEnum.Invalid, null},
        };

        public static Image GetDofOutputIcon(DofConfigToolOutputEnum output)
        {
            Image icon = null;
            DofOutputIcons.TryGetValue(output, out icon);
            return icon;
        }
    }
}
