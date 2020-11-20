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
    class TCSPropertyDescriptor
    {
        public bool Browsable = true;
        public string Category = string.Empty;
        public bool ReadOnly = false;
        public Type TypeConverter = null;
        public UITypeEditor TypeEditor = null;
    }

    class TableConfigSettingTypeDescriptor : CustomTypeDescriptor
    {
        public bool EditableTrigger { get; set; } = false;

        public TableConfigSetting WrappedTCS { get; private set; }

        public OutputControlEnum OutputControl { get; set; }

        private Dictionary<string, TCSPropertyDescriptor> TCSPropertyDescriptors = new Dictionary<string, TCSPropertyDescriptor>();

        public TableConfigSettingTypeDescriptor(TableConfigSetting TCS)
            : base(TypeDescriptor.GetProvider(TCS).GetTypeDescriptor(TCS))
        {
            WrappedTCS = TCS;

            TCSPropertyDescriptors["OutputControl"] = new TCSPropertyDescriptor() { ReadOnly = !EditableTrigger };
            TCSPropertyDescriptors["TableElement"] = new TCSPropertyDescriptor() { ReadOnly = !EditableTrigger };
            TCSPropertyDescriptors["Condition"] = new TCSPropertyDescriptor() { ReadOnly = !EditableTrigger };

            TCSPropertyDescriptors["OutputType"] = new TCSPropertyDescriptor() { Browsable = false };
            TCSPropertyDescriptors["ColorConfig"] = new TCSPropertyDescriptor() { Browsable = false };
            TCSPropertyDescriptors["ColorConfig2"] = new TCSPropertyDescriptor() { Browsable = false };
        }

        public override PropertyDescriptorCollection GetProperties()
        {
            return this.GetProperties(new Attribute[] { });
        }

        private PropertyDescriptor ToCustomProperty(PropertyDescriptor p)
        {
            if (!TCSPropertyDescriptors.Keys.Contains(p.Name)) {
                return p;
            }

            var customDesc = TCSPropertyDescriptors[p.Name];

            List<Attribute> customAttributes = new List<Attribute>();
            customAttributes.Add(new BrowsableAttribute(customDesc.Browsable));
            customAttributes.Add(new ReadOnlyAttribute(customDesc.ReadOnly));
            if (customDesc.Category != string.Empty) {
                customAttributes.Add(new CategoryAttribute(customDesc.Category));
            }
            if (customDesc.TypeConverter != null) {
                customAttributes.Add(new TypeConverterAttribute(customDesc.TypeConverter));
            }
            if (customDesc.TypeEditor != null) {
                customAttributes.Add(new EditorAttribute(customDesc.TypeEditor.GetType(), typeof(UITypeEditor)));
            }
            var attributes = AttributeCollection.FromExisting(p.Attributes, customAttributes.ToArray());
            var customProp = TypeDescriptor.CreateProperty(WrappedTCS.GetType(), p, attributes.Cast<Attribute>().ToArray());
            return customProp;
        }

        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            var properties = base.GetProperties(attributes).Cast<PropertyDescriptor>()
                                 .Select(p => ToCustomProperty(p));

            return new PropertyDescriptorCollection(properties.ToArray());
        }
    }
}
