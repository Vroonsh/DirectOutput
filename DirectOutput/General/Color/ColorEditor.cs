using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DirectOutput.General.Color
{
    class ColorEditor : UITypeEditor
    {
        private IWindowsFormsEditorService service;

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            // This tells it to show the [...] button which is clickable firing off EditValue below.
            return UITypeEditorEditStyle.Modal;
        }
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider != null)
                service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (service != null) {
                // Create a new instance of the ColorDialog.
                ColorDialog selectionControl = new ColorDialog();


                // Set the selected color in the dialog.
                int argb = 0;
                int alpha = 0;
                if (value is IARGBConverter argbConverter) {
                    argb = argbConverter.ToARGB();
                    alpha = (int)((argb & 0xFF000000) >> 24);
                }

                selectionControl.Color = System.Drawing.Color.FromArgb(argb);

                // Show the dialog.
                selectionControl.ShowDialog();

                // Return the newly selected color.
                if (value is RGBAColor) {
                    value = new RGBAColor();
                    (value as IARGBConverter).FromARGB(selectionControl.Color.ToArgb());
                    (value as RGBAColor).Alpha = alpha;
                } else if(value is RGBColor) {
                    value = new RGBColor();
                    (value as IARGBConverter).FromARGB(selectionControl.Color.ToArgb());
                }
            }

            return value;
        }
    }
}
