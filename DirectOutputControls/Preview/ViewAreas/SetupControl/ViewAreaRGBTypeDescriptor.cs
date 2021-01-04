using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectOutputControls
{
    public class ViewAreaRGBTypeDescriptor : BaseTypeDescriptor
    {
        public DirectOutputViewAreaRGB WrappedArea { get; private set; }

        public ViewAreaRGBTypeDescriptor(DirectOutputViewAreaRGB area, bool editable)
            : base(area, editable)
        {
            WrappedArea = area;

            PropertyDescriptors["Width"] = new PropertyDescriptorHandler();
            PropertyDescriptors["Height"] = new PropertyDescriptorHandler();
            PropertyDescriptors["StartAngle"] = new PropertyDescriptorHandler();

            Refresh();
        }

        public override void Refresh()
        {
            switch (WrappedArea.RenderType) {
                case DirectOutputViewAreaRGB.RenderTypeEnum.Frame:
                case DirectOutputViewAreaRGB.RenderTypeEnum.Matrix: {
                    PropertyDescriptors["Width"].Browsable = true;
                    PropertyDescriptors["Height"].Browsable = true;
                    PropertyDescriptors["StartAngle"].Browsable = false;
                    break;
                }

                case DirectOutputViewAreaRGB.RenderTypeEnum.Ring: {
                    PropertyDescriptors["Width"].Browsable = true;
                    PropertyDescriptors["Height"].Browsable = false;
                    PropertyDescriptors["StartAngle"].Browsable = true;
                    break;
                }

                default: {
                    PropertyDescriptors["Width"].Browsable = false;
                    PropertyDescriptors["Height"].Browsable = false;
                    PropertyDescriptors["StartAngle"].Browsable = false;
                    break;
                }
            }
        }
    }
}
