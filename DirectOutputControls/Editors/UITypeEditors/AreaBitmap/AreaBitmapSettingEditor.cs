using DirectOutputControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace DirectOutputControls
{
    public class AreaBitmapSettingEditor : UITypeEditor
    {
        public AreaBitmapSettingEditor()
        {
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context.Instance is IEditableInstance editable) {
                if (!editable.IsEditable()) {
                    return value;
                }
            }

            if (context.Instance is IBitmapListProvider bitmapProvider) {
                IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                AreaBitmapSetting bitmapSetting = value as AreaBitmapSetting;
                if (edSvc != null) {
                    AreaBitmapSettingForm frm = new AreaBitmapSettingForm() { Setting = bitmapSetting, BitmapList = bitmapProvider.GetBitmapList().ToArray() };
                    frm.ShowDialog();
                    return frm.Setting;
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
