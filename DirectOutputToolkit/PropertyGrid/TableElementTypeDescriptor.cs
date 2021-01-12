using DirectOutput.Table;
using DirectOutputControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectOutputToolkit
{
    public class TableElementTypeDescriptor : BaseTypeDescriptor
    {
        public TableElement WrappedTE { get; private set; }

        public TableElementTypeDescriptor(TableElement TE, bool editable)
            : base(TE, editable)
        {
            WrappedTE = TE;

            PropertyDescriptors["Name"] = new PropertyDescriptorHandler();
            PropertyDescriptors["Number"] = new PropertyDescriptorHandler();
            PropertyDescriptors["TableElementType"] = new PropertyDescriptorHandler();

            PropertyDescriptors["AssignedEffects"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["Value"] = new PropertyDescriptorHandler() { Browsable = false };

            Refresh();
        }

        public override void Refresh()
        {
            if (WrappedTE.TableElementType == DirectOutput.TableElementTypeEnum.NamedElement) {
                PropertyDescriptors["Name"].Browsable = true;
                PropertyDescriptors["Number"].Browsable = false;
            } else if (WrappedTE.TableElementType != DirectOutput.TableElementTypeEnum.Unknown) {
                PropertyDescriptors["Name"].Browsable = false;
                PropertyDescriptors["Number"].Browsable = true;
            } else {
                PropertyDescriptors["Name"].Browsable = false;
                PropertyDescriptors["Number"].Browsable = false;
            }
        }
    }
}
