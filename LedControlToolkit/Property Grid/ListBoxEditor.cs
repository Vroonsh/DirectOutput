using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace LedControlToolkit
{
    public class ListBoxEditor : ListBox
    {
        protected object m_oSelection = null;
        protected IWindowsFormsEditorService m_iwsService = null;

        public ListBoxEditor(object selection, IWindowsFormsEditorService edsvc)
        {
            m_iwsService = edsvc;
            SelectionMode = SelectionMode.One;
            BorderStyle = BorderStyle.None;
            m_oSelection = selection;
        }

        public object Selection
        {
            get {
                return m_oSelection;
            }
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            if (SelectedItem != null) {
                m_oSelection = SelectedItem;
            }
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (m_iwsService != null) {
                m_iwsService.CloseDropDown();
            }
        }
    }
}
