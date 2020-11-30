using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedControlToolkit
{
    public abstract class BaseTypeDescriptor : CustomTypeDescriptor
    {
        public enum EditionMode
        {
            Disabled,
            Table,
            Full
        }

        protected EditionMode EditMode { get; private set; } = EditionMode.Disabled;

        protected object WrappedObject;

        protected Dictionary<string, PropertyDescriptorHandler> PropertyDescriptors = new Dictionary<string, PropertyDescriptorHandler>();

        public BaseTypeDescriptor(object wrappedObj, EditionMode editMode)
            : base(TypeDescriptor.GetProvider(wrappedObj).GetTypeDescriptor(wrappedObj))
        {
            WrappedObject = wrappedObj;
            EditMode = editMode;
        }

        public override PropertyDescriptorCollection GetProperties()
        {
            return this.GetProperties(new Attribute[] { });
        }

        private PropertyDescriptor ToCustomProperty(PropertyDescriptor p)
        {
            if (!PropertyDescriptors.Keys.Contains(p.Name)) {
                return p;
            }

            var customDesc = PropertyDescriptors[p.Name];

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
            var customProp = TypeDescriptor.CreateProperty(WrappedObject.GetType(), p, attributes.Cast<Attribute>().ToArray());
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
