using DirectOutput.General.Color;
using DirectOutput.LedControl.Loader;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DirectOutputControls
{
    public class ColorConfigTypeConverter : TypeConverter
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
            Width = 200;
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
            if (context.Instance is IEditableInstance editable) {
                if (!editable.IsEditable()) {
                    return value;
                }
            }

            if (context.Instance is IColorListProvider colorListProvider) {
                IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                var colorConfig = value as ColorConfig;
                if (colorConfig == null) {
                    colorConfig = colorListProvider.GetColorConfig(null);
                }
                if (edSvc != null) {
                    var colorList = colorListProvider.GetColorList();
                    colorList.Insert(0, new RGBAColorNamed("Custom Color", new RGBColor(255, 255, 255)));
                    ColorConfigListBoxEditor dropdown = new ColorConfigListBoxEditor(colorConfig.Name, colorList, edSvc);
                    dropdown.Items.AddRange(colorList.Select(CN => CN.Name).ToArray());
                    edSvc.DropDownControl(dropdown);
                    if (dropdown.SelectedIndex == 0) {
                        ColorDialog dlg = new ColorDialog();
                        dlg.ShowDialog();
                        RGBAColor customColor = new RGBAColor(dlg.Color.R, dlg.Color.G, dlg.Color.B) ;
                        return new ColorConfig() { Name = $"{customColor.ToString()}", Red = customColor.Red, Green = customColor.Green, Blue = customColor.Blue, Alpha = 255 };
                    } else {
                        return colorListProvider.GetColorConfig(dropdown.Selection as string);
                    }
                }
            }

            return value;
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null) {
                if (context.Instance is IEditableInstance editable) {
                    return editable.IsEditable() ? UITypeEditorEditStyle.DropDown : UITypeEditorEditStyle.None;
                }
                return UITypeEditorEditStyle.DropDown;
            }
            return UITypeEditorEditStyle.None;
        }

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return (context.Instance is IColorListProvider);
        }

        public override void PaintValue(PaintValueEventArgs e)
        {
            if (e.Context.Instance is IColorListProvider) {
                var colorConfig = e.Value as ColorConfig;
                if (colorConfig != null) {
                    Rectangle rect = e.Bounds;
                    rect.Inflate(-2, -2);
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(colorConfig.Red, colorConfig.Green, colorConfig.Blue)), rect);
                }
            }
        }
    }
}
