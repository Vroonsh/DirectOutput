using DirectOutput.FX.MatrixFX;
using DirectOutput.LedControl.Loader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedControlToolkit
{
    class TableConfigSettingTypeDescriptor : BaseTypeDescriptor
    {
        public TableConfigSetting WrappedTCS { get; private set; }

        public enum EffectType
        {
            None,
            Flicker,
            Shift,
            Plasma,
            Bitmap,
            Shape
        }

        public EffectType EffType { get; private set; } = EffectType.None;

        public TableConfigSettingTypeDescriptor(TableConfigSetting TCS, bool editable)
            : base(TCS, editable)
        {
            WrappedTCS = TCS;

            PropertyDescriptors["OutputControl"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["TableElement"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["Condition"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["OutputType"] = new PropertyDescriptorHandler() { Browsable = false };
            PropertyDescriptors["ColorConfig"] = new PropertyDescriptorHandler() { Browsable = false };

            PropertyDescriptors["PlasmaSpeed"] = new PropertyDescriptorHandler();
            PropertyDescriptors["PlasmaDensity"] = new PropertyDescriptorHandler();
            PropertyDescriptors["ColorName2"] = new PropertyDescriptorHandler();
            PropertyDescriptors["ColorConfig2"] = new PropertyDescriptorHandler() { Browsable = false };

            PropertyDescriptors["AreaDirection"] = new PropertyDescriptorHandler();
            PropertyDescriptors["AreaAcceleration"] = new PropertyDescriptorHandler();
            PropertyDescriptors["AreaSpeed"] = new PropertyDescriptorHandler();

            PropertyDescriptors["ShapeName"] = new PropertyDescriptorHandler();

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

            Refresh();
        }

        protected override void GenerateCustomFields()
        {
//            CustomFields.Add(new CustomFieldPropertyDescriptor<TableConfigSetting, EffectType>(this, new CustomField<EffectType>("Effect Type", EffectType.None) { ReadOnly = !Editable }));
        }

        //private void InitEffectType()
        //{
        //    EffectType effectType;
        //    if (!WrappedTCS.ShapeName.IsNullOrEmpty()) {
        //        effectType = EffectType.Shape;
        //    } else if (WrappedTCS.IsBitmap) {
        //        effectType = EffectType.Bitmap;
        //    } else if (WrappedTCS.IsPlasma) {
        //        effectType = EffectType.Plasma;
        //    } else if (WrappedTCS.AreaDirection != MatrixShiftDirectionEnum.Invalid) {
        //        effectType = EffectType.Shift;
        //    } else if (WrappedTCS.AreaFlickerDensity > 0) {
        //        effectType = EffectType.Flicker;
        //    } else {
        //        effectType = EffectType.None;
        //    }
        //    CustomFieldValues["Effect Type"] = effectType;
        //}

        public override void Refresh()
        {
            //if (!Editable) {
            //    InitEffectType();
            //}
            //EffectType effectType = (EffectType)CustomFieldValues["Effect Type"];
            //if (Editable) {
            //    WrappedTCS.IsPlasma = (effectType == EffectType.Plasma);
            //    WrappedTCS.IsBitmap = (effectType == EffectType.Bitmap);
            //    if (effectType != EffectType.Shift) {
            //        WrappedTCS.AreaDirection = MatrixShiftDirectionEnum.Invalid;
            //    }
            //    if (effectType != EffectType.Shape) {
            //        WrappedTCS.ShapeName = string.Empty;
            //    }
            //    if (effectType != EffectType.Flicker) {
            //        WrappedTCS.AreaFlickerDensity = 0;
            //    }
            //} 
            var effectType = WrappedTCS.EffectType;

            PropertyDescriptors["ColorName2"].Browsable = (effectType == TableConfigSetting.EffectTypeMX.Plasma);
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
