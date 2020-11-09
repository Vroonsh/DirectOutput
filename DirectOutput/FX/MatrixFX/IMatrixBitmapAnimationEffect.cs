using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectOutput.FX.MatrixFX
{
    public interface IMatrixBitmapAnimationEffect : IMatrixBitmapEffect
    {
        MatrixAnimationStepDirectionEnum AnimationStepDirection { get; set; }
        int AnimationStepSize { get; set; }
        int AnimationFrameCount { get; set; }
        AnimationBehaviourEnum AnimationBehaviour { get; set; }
        int AnimationFrameDurationMs { get; set; }
    }
}
