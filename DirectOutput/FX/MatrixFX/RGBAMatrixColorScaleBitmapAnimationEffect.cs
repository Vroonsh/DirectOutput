﻿using DirectOutput.General.BitmapHandling;
using DirectOutput.General.Color;

namespace DirectOutput.FX.MatrixFX
{

    /// <summary>
    /// This displays a defined part of a bitmap as a animation in the given colors on a area of a RGBAtoy Matrix. The effect take the overall brightness of the pixels of the bitmap to control the brightness of the specified colors for each pixel.
    /// 
    /// The properties of the effect allow you to select the parts of the bitmap to display for the animation as well as the area of the matrix on which the bitmap is displayed. Depending on the size of your bitmap you might choose different modes for the image extraction.
    /// 
    /// The effect supports numerous image formats, inluding png, gif (also animated) and jpg.
    /// 
    /// The image extraction takes place upon initalization of the framework. While the framework is active, it only outputs the previously extracted and scaled data to allow for better performance. 
    /// </summary>
    public class RGBAMatrixColorScaleBitmapAnimationEffect : MatrixBitmapAnimationEffectBase<RGBAColor>, IMatrixRGBAColor
    {
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
        /// Gets the value for a single element in the matrix.
        /// </summary>
        /// <param name="TriggerValue">The trigger value.</param>
        /// <param name="Pixel">A pixel representing a element in the matrix.</param>
        /// <returns>The RGBAData for a element in the matrix</returns>
        protected override RGBAColor GetEffectValue(int TriggerValue, PixelData Pixel)
        {
            RGBAColor D = Pixel.GetRGBAColor();

            D.Alpha = (int)((float)Pixel.Alpha * TriggerValue / 255);

            return D;

        }



        /// <summary>
        /// Initializes the effect.
        /// Resolves object references, extracts source image data.
        /// </summary>
        /// <param name="Table">Table object containing the effect.</param>
        public override void Init(Table.Table Table)
        {
            base.Init(Table);

            if (Pixels != null)
            {
                foreach (PixelData[,] Frame in Pixels)
                {


                    for (int Y = 0; Y <= Frame.GetUpperBound(1); Y++)
                    {

                        for (int X = 0; X <= Frame.GetUpperBound(0); X++)
                        {
                            PixelData P = Frame[X, Y];

                            double Brightness = ((double)(P.Red + P.Green + P.Blue) / 3).Limit(0, 255);

                            P.Red = (byte)(InactiveColor.Red + (int)((float)(ActiveColor.Red - InactiveColor.Red) * Brightness / 255)).Limit(0, 255);
                            P.Green = (byte)(InactiveColor.Green + (int)((float)(ActiveColor.Green - InactiveColor.Green) * Brightness / 255)).Limit(0, 255);
                            P.Blue = (byte)(InactiveColor.Blue + (int)((float)(ActiveColor.Blue - InactiveColor.Blue) * Brightness / 255)).Limit(0, 255);
                            P.Alpha = (byte)(InactiveColor.Alpha + (int)((float)(ActiveColor.Alpha - InactiveColor.Alpha) * Brightness / 255)).Limit(0, 255);
                            Frame[X, Y] = P;

                        }

                    }
                }

            }

        }

    }
}
