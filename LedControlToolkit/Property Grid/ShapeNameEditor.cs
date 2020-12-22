using DirectOutputControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace LedControlToolkit
{
    public class ShapeNameEditor : UITypeEditor
    {
        public ShapeNameEditor()
        {
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context.Instance is TableConfigSettingTypeDescriptor TCSDesc) {
                IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                string shapeName = value as string;
                if (edSvc != null) {
                    ListBoxEditor dropdown = new ListBoxEditor(value, edSvc);
                    dropdown.Items.AddRange(TCSDesc.Handler.GetShapeNames());
                    edSvc.DropDownControl(dropdown);
                    return dropdown.Selection;
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
            return false;
        }
    }
}
