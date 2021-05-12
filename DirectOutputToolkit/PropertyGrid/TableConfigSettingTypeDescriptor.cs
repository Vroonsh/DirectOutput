using DirectOutput.Cab.Toys;
using DirectOutput.FX.MatrixFX;
using DirectOutput.General.Analog;
using DirectOutput.General.Color;
using DirectOutput.LedControl.Loader;
using DirectOutputControls;
using DofConfigToolWrapper;
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
    class TableConfigSettingTypeDescriptor : BaseTypeDescriptor, IColorListProvider, IShapeListProvider, IToyListProvider, IBitmapListProvider
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

        public ColorConfig ResolveColorConfig(RGBAColor color)
        {
            var colorConf = Handler?.ColorConfigurations.FirstOrDefault(CC => CC.Red == color.Red && CC.Green == color.Green && CC.Blue == color.Blue && CC.Alpha == color.Alpha);
            if (colorConf == null) {
                return new ColorConfig() { Name = $"{color.ToString()}", Red = color.Red, Green = color.Green, Blue = color.Blue, Alpha = 255 };
            }
            return colorConf;
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

        #region IBitmapListProvider
        public IEnumerable<Image> GetBitmapList()
        {
            return Handler.GetBitmapList(WrappedTCS);
        }
        #endregion

        public TableConfigSettingTypeDescriptor(EffectTreeNode node, bool editable, DirectOutputToolkitHandler handler)
            : base(node.TCS, editable)
        {
            EffectNode = node;
            WrappedTCS = EffectNode.TCS;
            Handler = handler;

            PropertyDescriptors["AreaEffectType"] = new PropertyDescriptorHandler() { };
            PropertyDescriptors["AreaLeft"] = new PropertyDescriptorHandler() { };
            PropertyDescriptors["AreaTop"] = new PropertyDescriptorHandler() { };
            PropertyDescriptors["AreaWidth"] = new PropertyDescriptorHandler() { };
            PropertyDescriptors["AreaHeight"] = new PropertyDescriptorHandler() { };

            PropertyDescriptors["OutputControl"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["TableElement"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["Condition"] = new PropertyDescriptorHandler() { Browsable = WrappedTCS.OutputControl == OutputControlEnum.Condition };
            PropertyDescriptors["OutputType"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["ColorName"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["ColorName2"] = new PropertyDescriptorHandler() { Browsable = false };

            PropertyDescriptors["ColorConfig"] = new PropertyDescriptorHandler() { DisplayName = "Color", TypeConverter = typeof(ColorConfigTypeConverter), TypeEditor = typeof(ColorConfigEditor) };
            PropertyDescriptors["Intensity"] = new PropertyDescriptorHandler() { DisplayName = "Intensity (0->255)" };

            PropertyDescriptors["BlinkLow"] = new PropertyDescriptorHandler() { DisplayName = "BlinkLow (0->255)" };

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

            PropertyDescriptors["AreaBitmapTop"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["AreaBitmapLeft"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["AreaBitmapWidth"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["AreaBitmapHeight"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["AreaBitmapFrame"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["AreaBitmapAnimationStepSize"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["AreaBitmapAnimationStepCount"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["AreaBitmapAnimationFrameDuration"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["AreaBitmapAnimationDirection"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["AreaBitmapAnimationBehaviour"] = new PropertyDescriptorHandler() { Browsable = false };

            GenerateCustomFields();
            Refresh();
        }

        protected override void GenerateCustomFields()
        {
            CustomFields.Add(new CustomFieldPropertyDescriptor<TableConfigSetting, string>(this,
                new CustomField<string>("ToyName", EffectNode.Effect?.GetAssignedToy()?.Name),
                new Attribute[]
                {
                new DisplayNameAttribute("Toy Name"),
                new ReadOnlyAttribute(!Editable),
                new EditorAttribute(typeof(ToyNameEditor), typeof(UITypeEditor))
                }));

            var bitMapSetting = new AreaBitmapSetting();
            bitMapSetting.FromTableConfigSetting(WrappedTCS);

            CustomFields.Add(new CustomFieldPropertyDescriptor<TableConfigSetting, AreaBitmapSetting>(this,
                new CustomField<AreaBitmapSetting>("BitmapSettings", bitMapSetting),
                new Attribute[]
                {
                new CategoryAttribute("Bitmap"),
                new DisplayNameAttribute("Bitmap settings"),
                new ReadOnlyAttribute(!Editable),
                new EditorAttribute(typeof(AreaBitmapSettingEditor), typeof(UITypeEditor))
                }));
        }

        public override void Refresh()
        {
            var toyName = CustomFieldValues["ToyName"] as string;
            if (!toyName.Equals(EffectNode.Effect?.GetAssignedToy()?.Name, StringComparison.InvariantCultureIgnoreCase)) {
                var toy = Handler.Toys.FirstOrDefault(T => T.Name.Equals(toyName, StringComparison.InvariantCultureIgnoreCase));
                EffectNode.Effect.SetAssignedToy(toy);

                if (toy is IMatrixToy<RGBAColor> || toy is IMatrixToy<AnalogAlpha>) {
                    if (WrappedTCS.AreaEffectType == TableConfigSetting.EffectTypeMX.Invalid) {
                        WrappedTCS.AreaEffectType = TableConfigSetting.EffectTypeMX.None;
                    }
                } else {
                    WrappedTCS.AreaEffectType = TableConfigSetting.EffectTypeMX.Invalid;
                }
            }

            var effectType = WrappedTCS.AreaEffectType;
            var assignedToy = EffectNode.Effect.GetAssignedToy();

            PropertyDescriptors["ColorConfig"].Browsable = (assignedToy is IRGBAToy) || (assignedToy is IMatrixToy<RGBAColor>);
            PropertyDescriptors["Intensity"].Browsable = (assignedToy is IAnalogAlphaToy) || (assignedToy is IMatrixToy<AnalogAlpha>);

            PropertyDescriptors["AreaEffectType"].Browsable = (effectType != TableConfigSetting.EffectTypeMX.Invalid);
            PropertyDescriptors["AreaLeft"].Browsable = (effectType != TableConfigSetting.EffectTypeMX.Invalid);
            PropertyDescriptors["AreaTop"].Browsable = (effectType != TableConfigSetting.EffectTypeMX.Invalid);
            PropertyDescriptors["AreaWidth"].Browsable = (effectType != TableConfigSetting.EffectTypeMX.Invalid);
            PropertyDescriptors["AreaHeight"].Browsable = (effectType != TableConfigSetting.EffectTypeMX.Invalid);

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

            GetCustomField<TableConfigSetting, AreaBitmapSetting>("BitmapSettings").Enabled = (effectType == TableConfigSetting.EffectTypeMX.Bitmap);
            if (effectType == TableConfigSetting.EffectTypeMX.Bitmap) {
                var bitmapSetting = CustomFieldValues["BitmapSettings"] as AreaBitmapSetting;
                bitmapSetting.ToTableConfigSetting(WrappedTCS);
            }
        }

    }
}
