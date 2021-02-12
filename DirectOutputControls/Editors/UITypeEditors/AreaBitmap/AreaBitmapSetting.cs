using DirectOutput.FX.MatrixFX;
using DirectOutput.LedControl.Loader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectOutputControls
{
    public class AreaBitmapSetting : ICloneable
    {
        public Rectangle Rect { get; set; } = new Rectangle();
        [Browsable(false)]
        public int Frame { get; set; } = 0;
        [DisplayName("Animation Step Size")]
        public int AreaBitmapAnimationStepSize { get; set; } = 1;
        [DisplayName("Animation Step Count")]
        public int AreaBitmapAnimationStepCount { get; set; } = 0;
        [DisplayName("Animation Frame Rate")]
        public int AreaBitmapAnimationFrameRate { get; set; } = 30;
        [DisplayName("Animation Direction")]
        public MatrixAnimationStepDirectionEnum AreaBitmapAnimationDirection { get; set; } = MatrixAnimationStepDirectionEnum.Frame;
        [DisplayName("Animation Behaviour")]
        public AnimationBehaviourEnum AreaBitmapAnimationBehaviour { get; set; } = AnimationBehaviourEnum.Loop;

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override string ToString()
        {
            return $"Click to edit";
        }

        public void FromTableConfigSetting(TableConfigSetting TCS)
        {
            Rect = new Rectangle(TCS.AreaBitmapLeft, TCS.AreaBitmapTop, TCS.AreaBitmapWidth, TCS.AreaBitmapHeight);
            Frame = TCS.AreaBitmapFrame;
            AreaBitmapAnimationDirection = TCS.AreaBitmapAnimationDirection;
            AreaBitmapAnimationBehaviour = TCS.AreaBitmapAnimationBehaviour;
            AreaBitmapAnimationFrameRate = (int)((1000.0f / TCS.AreaBitmapAnimationFrameDuration)+0.5f);
            AreaBitmapAnimationStepCount = TCS.AreaBitmapAnimationStepCount;
            AreaBitmapAnimationStepSize = TCS.AreaBitmapAnimationStepSize;
        }

        public void ToTableConfigSetting(TableConfigSetting TCS)
        {
            TCS.AreaBitmapLeft = Rect.X;
            TCS.AreaBitmapTop = Rect.Y;
            TCS.AreaBitmapWidth = Rect.Width;
            TCS.AreaBitmapHeight = Rect.Height;
            TCS.AreaBitmapFrame = Frame;
            TCS.AreaBitmapAnimationDirection = AreaBitmapAnimationDirection;
            TCS.AreaBitmapAnimationBehaviour = AreaBitmapAnimationBehaviour;
            TCS.AreaBitmapAnimationFrameDuration = (int)((1000.0f / AreaBitmapAnimationFrameRate) + 0.5f);
            TCS.AreaBitmapAnimationStepCount = AreaBitmapAnimationStepCount;
            TCS.AreaBitmapAnimationStepSize = AreaBitmapAnimationStepSize;
        }
    }
}
