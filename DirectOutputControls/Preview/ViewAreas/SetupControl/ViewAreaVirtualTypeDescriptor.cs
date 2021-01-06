using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectOutputControls
{
    class ViewAreaVirtualTypeDescriptor : BaseTypeDescriptor
    {
        public DirectOutputViewAreaVirtual WrappedArea { get; private set; }

        public ViewAreaVirtualTypeDescriptor(DirectOutputViewAreaVirtual area, bool editable)
            : base(area, editable)
        {
            WrappedArea = area;

            PropertyDescriptors["Squarred"] = new PropertyDescriptorHandler() { Browsable = false };

            Refresh();
        }

        public override void Refresh()
        {
        }
    }
}
