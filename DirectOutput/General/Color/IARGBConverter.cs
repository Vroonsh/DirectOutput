using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectOutput.General.Color
{
    interface IARGBConverter
    {
        int ToARGB();
        void FromARGB(int argb);
    }
}
