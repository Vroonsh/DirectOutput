using DirectOutput.Cab.Toys;
using DirectOutput.FX.MatrixFX;
using DirectOutput.General.Color;
using DirectOutput.LedControl.Loader;
using DirectOutputControls;
using DofConfigToolWrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectOutputToolkit
{
    class TableConfigSettingTypeDescriptor : BaseTypeDescriptor, IColorListProvider, IShapeListProvider, IToyListProvider
    {
        public EffectTreeNode EffectNode { get; private set; }
        public TableConfigSetting WrappedTCS { get; private set; }
        public DirectOutputToolkitHandler Handler;

        #region IColorListProvider

        public ColorConfig GetColorConfig(string colorName)
        {
            if (colorName.IsNullOrEmpty()) {
                return Handler?.ColorConfigurations[0];
            }
            return Handler?.ColorConfigurations.FirstOrDefault(CC => CC.Name.Equals(colorName, StringComparison.InvariantCultureIgnoreCase));
        }

        public ColorList GetColorList() => Handler?.ColorConfigurations.GetCabinetColorList();

        #endregion

        #region IShapeListProvider
        public string[] GetShapeNames() => Handler.ShapeNames;
        #endregion

        #region IToyListProvider
        public IEnumerable<IToy> GetToyList()
        {
            return Handler.GetCompatibleToys(WrappedTCS);
        }
        #endregion

        public TableConfigSettingTypeDescriptor(EffectTreeNode node, bool editable, DirectOutputToolkitHandler handler)
            : base(node.TCS, editable)
        {
            EffectNode = node;
            WrappedTCS = EffectNode.TCS;
            Handler = handler;

            PropertyDescriptors["OutputControl"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["TableElement"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["Condition"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["OutputType"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["ColorName"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["ColorName2"] = new PropertyDescriptorHandler() { Browsable = false };

            PropertyDescriptors["ColorConfig"] = new PropertyDescriptorHandler() { DisplayName = "Color", TypeConverter = typeof(ColorConfigTypeConverter), TypeEditor = typeof(ColorConfigEditor) };

            PropertyDescriptors["PlasmaSpeed"] = new PropertyDescriptorHandler();
            PropertyDescriptors["PlasmaDensity"] = new PropertyDescriptorHandler();
            PropertyDescriptors["ColorConfig2"] = new PropertyDescriptorHandler() { DisplayName = "Color 2", TypeConverter = typeof(ColorConfigTypeConverter), TypeEditor = typeof(ColorConfigEditor) };

            PropertyDescriptors["AreaDirection"] = new PropertyDescriptorHandler();
            PropertyDescriptors["AreaAcceleration"] = new PropertyDescriptorHandler();
            PropertyDescriptors["AreaSpeed"] = new PropertyDescriptorHandler();

            PropertyDescriptors["ShapeName"] = new PropertyDescriptorHandler() { TypeEditor = typeof(ShapeNameEditor) };

            PropertyDescriptors["AreaFlickerDensity"] = new PropertyDescriptorHandler();
            PropertyDescriptors["AreaFlickerMinDurationMs"] = new PropertyDescriptorHandler();
            PropertyDescriptors["AreaFlickerMaxDurationMs"] = new PropertyDescriptorHandler();
            PropertyDescriptors["AreaFlickerFadeDurationMs"] = new PropertyDescriptorHandler();

            PropertyDescriptors["AreaBitmapTop"] = new PropertyDescriptorHandler();
            PropertyDescriptors["AreaBitmapLeft"] = new PropertyDescriptorHandler();
            PropertyDescriptors["AreaBitmapWidth"] = new PropertyDescriptorHandler();
            PropertyDescriptors["AreaBitmapHeight"] = new PropertyDescriptorHandler();
            PropertyDescriptors["AreaBitmapFrame"] = new PropertyDescriptorHandler();
            PropertyDescriptors["AreaBitmapAnimationStepSize"] = new PropertyDescriptorHandler();
            PropertyDescriptors["AreaBitmapAnimationStepCount"] = new PropertyDescriptorHandler();
            PropertyDescriptors["AreaBitmapAnimationFrameDuration"] = new PropertyDescriptorHandler();
            PropertyDescriptors["AreaBitmapAnimationDirection"] = new PropertyDescriptorHandler();
            PropertyDescriptors["AreaBitmapAnimationBehaviour"] = new PropertyDescriptorHandler();

            GenerateCustomFields();
            Refresh();
        }

        protected override void GenerateCustomFields()
        {
            CustomFields.Add(new CustomFieldPropertyDescriptor<TableConfigSetting, string>(this,
                new CustomField<string>("ToyName", EffectNode.Effect.GetAssignedToy()?.Name), 
                new Attribute[]
                {
                    new DisplayNameAttribute("Toy Name"),
                    new ReadOnlyAttribute(!Editable),
                    new EditorAttribute(typeof(ToyNameEditor), typeof(UITypeEditor))
                }));

            //CustomFields.Add(new CustomFieldPropertyDescriptor<TableConfigSetting, string>(this, 
            //    new CustomField<string>("DofOutput", Enum.GetName(typeof(DofConfigToolOutputEnum), EffectNode.Output)), 
            //    new Attribute[] 
            //    {
            //        new DisplayNameAttribute("Dof Output"),
            //        new ReadOnlyAttribute(!Editable),
            //        new EditorAttribute(typeof(DofOutputEditor), typeof(UITypeEditor))
            //    }));
        }

        public override void Refresh()
        {
            var toyName = (CustomFieldValues["ToyName"] as string);
            if (!toyName.Equals(EffectNode.Effect.GetAssignedToy()?.Name, StringComparison.InvariantCultureIgnoreCase)) {
                EffectNode.Effect.SetAssignedToy(Handler.Toys.FirstOrDefault(T => T.Name.Equals(toyName, StringComparison.InvariantCultureIgnoreCase)));
            }

            var effectType = WrappedTCS.EffectType;

            PropertyDescriptors["ColorConfig2"].Browsable = (effectType == TableConfigSetting.EffectTypeMX.Plasma);
            PropertyDescriptors["PlasmaSpeed"].Browsable = (effectType == TableConfigSetting.EffectTypeMX.Plasma);
            PropertyDescriptors["PlasmaDensity"].Browsable = (effectType == TableConfigSetting.EffectTypeMX.Plasma);

            PropertyDescriptors["AreaDirection"].Browsable = (effectType == TableConfigSetting.EffectTypeMX.Shift);
            PropertyDescriptors["AreaAcceleration"].Browsable = (effectType == TableConfigSetting.EffectTypeMX.Shift);
            PropertyDescriptors["AreaSpeed"].Browsable = (effectType == TableConfigSetting.EffectTypeMX.Shift);

            PropertyDescriptors["ShapeName"].Browsable = (effectType == TableConfigSetting.EffectTypeMX.Shape);

            PropertyDescriptors["AreaFlickerDensity"].Browsable = (effectType == TableConfigSetting.EffectTypeMX.Flicker);
            PropertyDescriptors["AreaFlickerMinDurationMs"].Browsable = (effectType == TableConfigSetting.EffectTypeMX.Flicker);
            PropertyDescriptors["AreaFlickerMaxDurationMs"].Browsable = (effectType == TableConfigSetting.EffectTypeMX.Flicker);
            PropertyDescriptors["AreaFlickerFadeDurationMs"].Browsable = (effectType == TableConfigSetting.EffectTypeMX.Flicker);

            PropertyDescriptors["AreaBitmapTop"].Browsable = (effectType == TableConfigSetting.EffectTypeMX.Bitmap);
            PropertyDescriptors["AreaBitmapLeft"].Browsable = (effectType == TableConfigSetting.EffectTypeMX.Bitmap);
            PropertyDescriptors["AreaBitmapWidth"].Browsable = (effectType == TableConfigSetting.EffectTypeMX.Bitmap);
            PropertyDescriptors["AreaBitmapHeight"].Browsable = (effectType == TableConfigSetting.EffectTypeMX.Bitmap);
            PropertyDescriptors["AreaBitmapFrame"].Browsable = (effectType == TableConfigSetting.EffectTypeMX.Bitmap);
            PropertyDescriptors["AreaBitmapAnimationStepSize"].Browsable = (effectType == TableConfigSetting.EffectTypeMX.Bitmap);
            PropertyDescriptors["AreaBitmapAnimationStepCount"].Browsable = (effectType == TableConfigSetting.EffectTypeMX.Bitmap);
            PropertyDescriptors["AreaBitmapAnimationFrameDuration"].Browsable = (effectType == TableConfigSetting.EffectTypeMX.Bitmap);
            PropertyDescriptors["AreaBitmapAnimationDirection"].Browsable = (effectType == TableConfigSetting.EffectTypeMX.Bitmap);
            PropertyDescriptors["AreaBitmapAnimationBehaviour"].Browsable = (effectType == TableConfigSetting.EffectTypeMX.Bitmap);
        }
    }
}
