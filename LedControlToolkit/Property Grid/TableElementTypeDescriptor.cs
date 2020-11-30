using DirectOutput.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedControlToolkit
{
    public class TableElementTypeDescriptor : BaseTypeDescriptor
    {
        public TableElement WrappedTE { get; private set; }

        public TableElementTypeDescriptor(TableElement TE, EditionMode editMode = EditionMode.Disabled)
            : base(TE, editMode)
        {
            WrappedTE = TE;

            PropertyDescriptors["Name"] = new PropertyDescriptorHandler() { ReadOnly = (EditMode != EditionMode.Full) };
            PropertyDescriptors["Number"] = new PropertyDescriptorHandler() { ReadOnly = (EditMode != EditionMode.Full) };
            PropertyDescriptors["TableElementType"] = new PropertyDescriptorHandler() { ReadOnly = (EditMode != EditionMode.Full) };

            PropertyDescriptors["AssignedEffects"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["Value"] = new PropertyDescriptorHandler() { Browsable = false };
        }
    }
}
