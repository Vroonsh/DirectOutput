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
        public bool Editable { get; private set; } = true;

        protected object WrappedObject;

        protected Dictionary<string, PropertyDescriptorHandler> PropertyDescriptors = new Dictionary<string, PropertyDescriptorHandler>();

        protected List<PropertyDescriptor> CustomFields = new List<PropertyDescriptor>();
        protected Dictionary<string, object> CustomFieldValues = new Dictionary<string, object>();

        public object this[string fieldName]
        {
            get {
                object value = null;
                CustomFieldValues.TryGetValue(fieldName, out value);
                return value;
            }

            set {
                CustomFieldValues[fieldName] = value;
            }
        }

        public BaseTypeDescriptor(object wrappedObj, bool editable, bool generateCustomFields = false)
            : base(TypeDescriptor.GetProvider(wrappedObj).GetTypeDescriptor(wrappedObj))
        {
            WrappedObject = wrappedObj;
            Editable = editable;
            if (generateCustomFields) {
                GenerateCustomFields();
            }
        }

        public override PropertyDescriptorCollection GetProperties()
        {
            return this.GetProperties(new Attribute[] { });
        }

        private PropertyDescriptor ToCustomProperty(PropertyDescriptor p)
        {
            List<Attribute> customAttributes = new List<Attribute>();

            if (PropertyDescriptors.Keys.Contains(p.Name)) {
                var customDesc = PropertyDescriptors[p.Name];
                customAttributes.Add(new BrowsableAttribute(customDesc.Browsable));
                customAttributes.Add(new ReadOnlyAttribute(customDesc.ReadOnly || !Editable));
                if (!customDesc.DisplayName.IsNullOrEmpty()) {
                    customAttributes.Add(new DisplayNameAttribute(customDesc.DisplayName));
                }
                if (!customDesc.Description.IsNullOrEmpty()) {
                    customAttributes.Add(new DescriptionAttribute(customDesc.Description));
                }
                if (!customDesc.Category.IsNullOrEmpty()) {
                    customAttributes.Add(new CategoryAttribute(customDesc.Category));
                }
                if (customDesc.TypeConverter != null) {
                    customAttributes.Add(new TypeConverterAttribute(customDesc.TypeConverter));
                }
                if (customDesc.TypeEditor != null) {
                    customAttributes.Add(new EditorAttribute(customDesc.TypeEditor, typeof(UITypeEditor)));
                }
            } else {
                customAttributes.Add(new ReadOnlyAttribute(!Editable));
            }

            var attributes = AttributeCollection.FromExisting(p.Attributes, customAttributes.ToArray());
            var customProp = TypeDescriptor.CreateProperty(WrappedObject.GetType(), p, attributes.Cast<Attribute>().ToArray());
            return customProp;
        }

        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            var properties = base.GetProperties(attributes).Cast<PropertyDescriptor>()
                                 .Select(p => ToCustomProperty(p)).Union(CustomFields);

            return new PropertyDescriptorCollection(properties.ToArray());
        }

        protected virtual void GenerateCustomFields() { }

        public abstract void Refresh();
    }
}
