using DirectOutput.General.Color;
using DirectOutput.LedControl.Loader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace LedControlToolkit
{
    class ColorConfigTypeConverter : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
        {
            if ((destType == typeof(string)) && (value is ColorConfig)) {
                return (value as ColorConfig).Name;
            }
            return base.ConvertTo(context, culture, value, destType);
        }

    }

    public class ColorConfigListBoxEditor : ListBoxEditor
    {
        private ColorList CabinetColorList;

        public ColorConfigListBoxEditor(object selection, ColorList cabColorList, IWindowsFormsEditorService edsvc)
            : base(selection, edsvc)
        {
            CabinetColorList = cabColorList;
            DrawMode = DrawMode.OwnerDrawFixed;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);
            if (e.Index >= 0) {
                var colorNamed = CabinetColorList[e.Index];
                if (colorNamed != null) {
                    Rectangle rect = e.Bounds;
                    rect.Inflate(-2, -2);
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(colorNamed.ToARGB())), rect.Left, rect.Top, rect.Height, rect.Height);
                    Font f = new Font("Arial", 9);
                    e.Graphics.DrawString(colorNamed.Name, f, SystemBrushes.WindowText, rect.Left + rect.Height + 4, rect.Top + ((rect.Height - e.Graphics.MeasureString(colorNamed.Name, f).Height) / 2));
                    f.Dispose();
                }
            }
        }
    }

    public class ColorConfigEditor : UITypeEditor
    {
        public ColorConfigEditor()
        {
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context.Instance is TableConfigSettingTypeDescriptor TCSDesc) {
                IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                var colorConfig = value as ColorConfig;
                if (colorConfig == null) {
                    colorConfig = TCSDesc.Handler.LedControlConfigData.ColorConfigurations[0];
                }
                if (edSvc != null) {
                    var colorList = TCSDesc.Handler.LedControlConfigData.ColorConfigurations.GetCabinetColorList();
                    ColorConfigListBoxEditor dropdown = new ColorConfigListBoxEditor(colorConfig.Name, colorList, edSvc);
                    dropdown.Items.AddRange(colorList.Select(CN => CN.Name).ToArray());
                    edSvc.DropDownControl(dropdown);
                    return TCSDesc.Handler.LedControlConfigData.ColorConfigurations.FirstOrDefault(CC=>CC.Name.Equals(dropdown.Selection as string, StringComparison.InvariantCultureIgnoreCase));
                }
            }

            return value;
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return (context.Instance is TableConfigSettingTypeDescriptor);
        }
        public override void PaintValue(PaintValueEventArgs e)
        {
            if (e.Context.Instance is TableConfigSettingTypeDescriptor TCSDesc) {
                var colorConfig = e.Value as ColorConfig;
                if (colorConfig != null) {
                    Rectangle rect = e.Bounds;
                    rect.Inflate(-2, -2);
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(colorConfig.GetCabinetColor().ToARGB())), rect);
                }
            }
        }
    }
}
