using DirectOutput.Cab.Toys;
using DirectOutput.General.Analog;
using DirectOutput.General.Color;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms.Design;

namespace DirectOutputControls
{
    public class ToyNameEditor : UITypeEditor
    {
        public ToyNameEditor()
        {
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context.Instance is IEditableInstance editable) {
                if (!editable.IsEditable()) {
                    return value;
                }
            }

            if (context.Instance is IToyListProvider toyListProvider) {
                IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                string shapeName = value as string;
                if (edSvc != null) {
                    ListBoxEditor dropdown = new ListBoxEditor(value, edSvc);
                    dropdown.Items.AddRange(toyListProvider.GetToyList(T => T is IMatrixToy<RGBAColor> || T is IMatrixToy<AnalogAlpha>).Select(T => T.Name).ToArray());
                    edSvc.DropDownControl(dropdown);
                    return dropdown.Selection;
                }
            }

            return value;
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null){
                if (context.Instance is IEditableInstance editable) {
                    return editable.IsEditable() ? UITypeEditorEditStyle.DropDown : UITypeEditorEditStyle.None;
                }
                return UITypeEditorEditStyle.DropDown;
            }
            return UITypeEditorEditStyle.None;
        }

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return false;
        }
    }
}
