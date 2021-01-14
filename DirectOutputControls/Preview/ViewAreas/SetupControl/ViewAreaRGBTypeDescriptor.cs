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

            PropertyDescriptors["Squarred"] = new PropertyDescriptorHandler();
            PropertyDescriptors["MxWidth"] = new PropertyDescriptorHandler();
            PropertyDescriptors["MxHeight"] = new PropertyDescriptorHandler();
            PropertyDescriptors["StartAngle"] = new PropertyDescriptorHandler();
            PropertyDescriptors["ValueType"] = new PropertyDescriptorHandler();
            PropertyDescriptors["ShowMatrixGrid"] = new PropertyDescriptorHandler();

            Refresh();
        }

        public override void Refresh()
        {
            PropertyDescriptors["ShowMatrixGrid"].Browsable = (WrappedArea.RenderType == DirectOutputViewAreaRGB.RenderTypeEnum.Matrix);

            switch (WrappedArea.RenderType) {
                case DirectOutputViewAreaRGB.RenderTypeEnum.Simple: {
                    WrappedArea.MxWidth = WrappedArea.MxHeight = 0;
                    PropertyDescriptors["Squarred"].Browsable = true;
                    PropertyDescriptors["MxWidth"].Browsable = false;
                    PropertyDescriptors["MxHeight"].Browsable = false;
                    PropertyDescriptors["StartAngle"].Browsable = false;
                    PropertyDescriptors["ValueType"].Browsable = false;
                    break;
                }

                case DirectOutputViewAreaRGB.RenderTypeEnum.Frame:
                case DirectOutputViewAreaRGB.RenderTypeEnum.Matrix: {
                    PropertyDescriptors["Squarred"].Browsable = false;
                    PropertyDescriptors["MxWidth"].Browsable = true;
                    PropertyDescriptors["MxHeight"].Browsable = true;
                    PropertyDescriptors["StartAngle"].Browsable = false;
                    PropertyDescriptors["ValueType"].Browsable = true;
                    break;
                }

                case DirectOutputViewAreaRGB.RenderTypeEnum.Ring: {
                    WrappedArea.MxWidth = 1;
                    PropertyDescriptors["Squarred"].Browsable = false;
                    PropertyDescriptors["MxWidth"].Browsable = false;
                    PropertyDescriptors["MxHeight"].Browsable = true;
                    PropertyDescriptors["MxHeight"].DisplayName = "Length";
                    PropertyDescriptors["StartAngle"].Browsable = true;
                    PropertyDescriptors["ValueType"].Browsable = true;
                    break;
                }

                default: {
                    WrappedArea.MxWidth = WrappedArea.MxHeight = 0;
                    PropertyDescriptors["MxWidth"].Browsable = false;
                    PropertyDescriptors["MxHeight"].Browsable = false;
                    PropertyDescriptors["StartAngle"].Browsable = false;
                    PropertyDescriptors["ValueType"].Browsable = false;
                    break;
                }
            }
        }
    }
}
