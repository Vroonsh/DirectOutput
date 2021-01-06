using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace DirectOutputControls
{
    public interface IShapeListProvider
    {
        string[] GetShapeNames();
    }

    public class ShapeNameEditor : UITypeEditor
    {
        public ShapeNameEditor()
        {
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context.Instance is IShapeListProvider shapeListProvider) {
                IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                string shapeName = value as string;
                if (edSvc != null) {
                    ListBoxEditor dropdown = new ListBoxEditor(value, edSvc);
                    dropdown.Items.AddRange(shapeListProvider.GetShapeNames());
                    edSvc.DropDownControl(dropdown);
                    return dropdown.Selection;
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
            return false;
        }
    }
}
