using DirectOutput.Cab.Toys;
using DirectOutput.General.Analog;
using DirectOutput.General.Color;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace LedControlToolkit
{
    public class ToyNameEditor : UITypeEditor
    {
        public ToyNameEditor()
        {
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context.Instance is TableConfigSettingTypeDescriptor TCSDesc) {
                if (TCSDesc.Editable) {
                    IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                    string shapeName = value as string;
                    if (edSvc != null) {
                        ListBoxEditor dropdown = new ListBoxEditor(value, edSvc);
                        dropdown.Items.AddRange(TCSDesc.Handler.Pinball.Cabinet.Toys.Where(T => T is IMatrixToy<RGBAColor> || T is IMatrixToy<AnalogAlpha>).Select(T => T.Name).ToArray());
                        edSvc.DropDownControl(dropdown);
                        return dropdown.Selection;
                    }
                }
            }

            return value;
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance is TableConfigSettingTypeDescriptor TCSDesc) {
                return TCSDesc.Editable ? UITypeEditorEditStyle.DropDown : UITypeEditorEditStyle.None;
            }
            return UITypeEditorEditStyle.None;
        }
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return false;
        }
    }
}
