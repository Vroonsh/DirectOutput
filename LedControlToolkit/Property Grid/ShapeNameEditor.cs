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
    //public class ColorConfigListBoxEditor : ListBoxEditor
    //{
    //    private string[] ShapeNames;

    //    public ColorConfigListBoxEditor(object selection, string[] shapeNames, IWindowsFormsEditorService edsvc)
    //        : base(selection, edsvc)
    //    {
    //        ShapeNames = shapeNames;
    //        DrawMode = DrawMode.OwnerDrawFixed;
    //    }

    //    protected override void OnDrawItem(DrawItemEventArgs e)
    //    {
    //        base.OnDrawItem(e);
    //        if (e.Index >= 0) {
    //            var shapeName = ShapeNames[e.Index];
    //            if (shapeName != null) {
    //                Rectangle rect = e.Bounds;
    //                rect.Inflate(-2, -2);
    //                Font f = new Font("Arial", 9);
    //                e.Graphics.DrawString(shapeName, f, SystemBrushes.WindowText, rect.Left + rect.Height + 4, rect.Top + ((rect.Height - e.Graphics.MeasureString(shapeName, f).Height) / 2));
    //                f.Dispose();
    //            }
    //        }
    //    }
    //}

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
                    dropdown.Items.AddRange(TCSDesc.Handler.PinballTable.ShapeDefinitions.Shapes.Select(S=>S.Name).ToArray());
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
