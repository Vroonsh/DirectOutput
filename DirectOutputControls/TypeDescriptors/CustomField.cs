using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectOutputControls
{
    public class CustomField<TField>
    {
        public CustomField(String name, TField defaultValue)
        {
            Name = name;
            DataType = typeof(TField);
            DefaultValue = defaultValue;
        }

        public String Name { get; private set; }

        public Type DataType { get; private set; }

        public TField DefaultValue { get; private set; }
    }

    public class CustomFieldPropertyDescriptor<TComponent, TField> : PropertyDescriptor
    {
        public CustomField<TField> CustomField { get; private set; }

        private BaseTypeDescriptor Container;

        public CustomFieldPropertyDescriptor(BaseTypeDescriptor container, CustomField<TField> customField, Attribute[] attrs)
            : base(customField.Name, attrs)
        {
            Container = container;
            CustomField = customField;
            SetValue(null, CustomField.DefaultValue);
        }

        public override bool CanResetValue(object component)
        {
            return true;
        }

        public override Type ComponentType
        {
            get {
                return typeof(TComponent);
            }
        }

        public override object GetValue(object component)
        {
            return Container[CustomField.Name] ?? (CustomField.DataType.IsValueType ?
                (Object)Activator.CreateInstance(CustomField.DataType) : null);
        }

        public override bool IsReadOnly
        {
            get {
                var readOnlyAttribute = Attributes.Cast<Attribute>().FirstOrDefault(A=>A is ReadOnlyAttribute) as ReadOnlyAttribute;
                return (readOnlyAttribute != null && readOnlyAttribute.IsReadOnly);
            }
        }

        public override Type PropertyType
        {
            get {
                return CustomField.DataType;
            }
        }

        public override void ResetValue(object component)
        {
            SetValue(component, CustomField.DefaultValue);
        }

        public override void SetValue(object component, object value)
        {
            Container[CustomField.Name] = value;
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }
}
