using DirectOutput.Table;
using DirectOutputControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedControlToolkit
{
    class EditionTableTypeDescriptor : BaseTypeDescriptor
    {
        public Table EditionTable {get; private set;}

        public EditionTableTypeDescriptor(Table Table, bool editable = true)
            : base(Table, editable)
        {
            EditionTable = Table;

            PropertyDescriptors["TableElements"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["Pinball"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["Bitmaps"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["ShapeDefinitions"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["TableFilename"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["TableConfigurationFilename"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["AddLedControlConfig"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["ConfigurationSource"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["Effects"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["AssignedStaticEffects"] = new PropertyDescriptorHandler() { Browsable = false };

            Refresh();
        }

        public override void Refresh() {}
    }
}
