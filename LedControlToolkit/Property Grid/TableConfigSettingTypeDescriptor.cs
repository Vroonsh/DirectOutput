using DirectOutput.FX.MatrixFX;
using DirectOutput.LedControl.Loader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedControlToolkit
{
    class TableConfigSettingTypeDescriptor : BaseTypeDescriptor
    {
        public TableConfigSetting WrappedTCS { get; private set; }

        public TableConfigSettingTypeDescriptor(TableConfigSetting TCS, EditionMode editMode = EditionMode.Disabled)
            : base(TCS, editMode)
        {
            WrappedTCS = TCS;

            PropertyDescriptors["OutputControl"] = new PropertyDescriptorHandler() { ReadOnly = (EditMode != EditionMode.Full) };
            PropertyDescriptors["TableElement"] = new PropertyDescriptorHandler() { ReadOnly = (EditMode != EditionMode.Full) };
            PropertyDescriptors["Condition"] = new PropertyDescriptorHandler() { ReadOnly = (EditMode != EditionMode.Full) };

            PropertyDescriptors["OutputType"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["ColorConfig"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["ColorConfig2"] = new PropertyDescriptorHandler() { Browsable = false };
        }
    }
}
