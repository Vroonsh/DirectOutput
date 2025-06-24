﻿using DirectOutput.General.Color;

namespace DirectOutput.FX.MatrixFX
{
    /// <summary>
    /// Does create random flickering with a defineable density, durations and color within the spefied area of a ledstrip.
    /// </summary>
    public class RGBAMatrixFlickerEffect : MatrixFlickerEffectBase<RGBAColor>, IMatrixRGBAColor
    {
        private const int RefreshIntervalMs = 30;

        private RGBAColor _ActiveColor = new RGBAColor(0xff, 0xff, 0xff, 0xff);

        /// <summary>
        /// Gets or sets the active color.
        /// The FadeMode property defines how this value is used.
        /// </summary>
        /// <value>
        /// The active color.
        /// </value>
        public RGBAColor ActiveColor
        {
            get { return _ActiveColor; }
            set { _ActiveColor = value; }
        }

        private RGBAColor _InactiveColor = new RGBAColor(0, 0, 0, 0);

        /// <summary>
        /// Gets or sets the inactive color.
        /// The FadeMode property defines how this value is used.
        /// </summary>
        /// <value>
        /// The inactive color.
        /// </value>
        public RGBAColor InactiveColor
        {
            get { return _InactiveColor; }
            set { _InactiveColor = value; }
        }


        /// <summary>
        /// Gets the effect color by mixinging Active and InactiveColor based on the TriggerValue.
        /// </summary>
        /// <param name="TriggerValue">The trigger value.</param>
        /// <returns>RGBAColor representing a mix of InactiveColor and ActiveColor.</returns>
        protected override RGBAColor GetEffectValue(int TriggerValue)
        {
            RGBAColor D = new RGBAColor();

            int V = TriggerValue.Limit(0, 255);
            D.Red = InactiveColor.Red + (int)((float)(ActiveColor.Red - InactiveColor.Red) * V / 255).Limit(0, 255);
            D.Green = InactiveColor.Green + (int)((float)(ActiveColor.Green - InactiveColor.Green) * V / 255).Limit(0, 255);
            D.Blue = InactiveColor.Blue + (int)((float)(ActiveColor.Blue - InactiveColor.Blue) * V / 255).Limit(0, 255);
            D.Alpha = InactiveColor.Alpha + (int)((float)(ActiveColor.Alpha - InactiveColor.Alpha) * V / 255).Limit(0, 255);
            return D;
        }

    }
}
