using System;
using System.ComponentModel;
using System.Globalization;

namespace DirectOutput.General.Color
{
    class ColorTypeConverter : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
        {
            if ((destType == typeof(string)) && (value is IARGBConverter)) {
                IARGBConverter color = (IARGBConverter)value;
                return color.ToARGB().ToString("X08");
            }
            return base.ConvertTo(context, culture, value, destType);
        }

    }
}
