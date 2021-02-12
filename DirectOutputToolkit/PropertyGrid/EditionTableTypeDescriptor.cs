using DirectOutput.Table;
using DirectOutputControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectOutputToolkit
{
    class EditionTableTypeDescriptor : BaseTypeDescriptor
    {
        public EditionTableTreeNode TableNode { get; private set; }

        public EditionTableTypeDescriptor(EditionTableTreeNode tableNode, bool editable = true)
            : base(tableNode.EditionTable, editable)
        {
            TableNode = tableNode;

            PropertyDescriptors["TableElements"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["Pinball"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["Bitmaps"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["ShapeDefinitions"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["TableFilename"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["TableConfigurationFilename"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["AddLedControlConfig"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["ConfigurationSource"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["Effects"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["AssignedStaticEffects"] = new PropertyDescriptorHandler() { Browsable = false };

            GenerateCustomFields();
            Refresh();
        }

        protected override void GenerateCustomFields()
        {
            CustomFields.Add(new CustomFieldPropertyDescriptor<EditionTableTypeDescriptor, Image>(this,
                new CustomField<Image>("Image", TableNode.Image),
                new Attribute[]
                {
                new BrowsableAttribute(true),
                new DisplayNameAttribute("Image"),
                new ReadOnlyAttribute(!Editable),
                new EditorAttribute(typeof(ImageSelectorEditor), typeof(UITypeEditor))
                }));
        }

        public override void Refresh()
        {
            TableNode.OnImageChanged(CustomFieldValues["Image"] as Image);
        }
    }
}
