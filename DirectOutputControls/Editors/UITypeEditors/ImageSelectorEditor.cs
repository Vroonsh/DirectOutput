using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DirectOutputControls
{
    public class ImageSelectorEditor : UITypeEditor
    {
        public ImageSelectorEditor()
        {
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context.Instance is IEditableInstance editable) {
                if (!editable.IsEditable()) {
                    return value;
                }
            }

            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            Image image = value as Image;
            if (edSvc != null) {
                OpenFileDialog dlg = new OpenFileDialog() {
                    Filter = "Image Files|*.PNG;*.BMP;*.JPG;*.GIF|All Files|*.*",
                    DefaultExt = "PNG",
                    Title = "Select image file"
                };
                dlg.ShowDialog();

                if (dlg.FileName != "") {
                    var newImage = Image.FromFile(dlg.FileName);
                    newImage.Tag = dlg.FileName;
                    return newImage;
                }
            }

            return value;
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null) {
                if (context.Instance is IEditableInstance editable) {
                    return editable.IsEditable() ? UITypeEditorEditStyle.Modal : UITypeEditorEditStyle.None;
                }
                return UITypeEditorEditStyle.Modal;
            }
            return UITypeEditorEditStyle.None;
        }

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return false;
        }
    }
}
