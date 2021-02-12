using DirectOutput.General.BitmapHandling;
using DirectOutput.Cab.Toys.Layer;
using DirectOutput.General.Color;

namespace DirectOutput.FX.MatrixFX
{
    /// <summary>
    /// The RGBAMatrixBitmapEffect displays a defined part of a bitmap on a area of a RGBAtoy Matrix.
    /// 
    /// The properties of the effect allow you to select the part of the bitmap to display as well as the area of the matrix on which the bitmap is displayed. Dempending on the size of your bitmap you might choose different modes for the image extraction.
    /// 
    /// The effect supports numerous imahe formats, inluding png, gif (also animated) and jpg.
    /// 
    /// The image extraction takes place upon initalization of the framework. While the framework is active, it only outputs the previously extracted and scaled data to allow for better performance. 
    /// </summary>
    public class RGBAMatrixBitmapEffect : MatrixBitmapEffectBase<RGBAColor>, IMatrixRGBAColor
    {
        /// <summary>
        /// Gets or sets the first active color.
        /// </summary>
        /// <value>
        /// The active color.
        /// </value>
        public RGBAColor ActiveColor { get; set; } = new RGBAColor(0x00, 0x00, 0xff, 0xff);

        /// <summary>
        /// Gets or sets the first active color.
        /// </summary>
        /// <value>
        /// The active color.
        /// </value>
        public RGBAColor InactiveColor { get; set; } = new RGBAColor(0x00, 0x00, 0xff, 0x00);

        /// <summary>
        /// Gets the value for a single element in the matrix.
        /// </summary>
        /// <param name="TriggerValue">The trigger value.</param>
        /// <param name="Pixel">A pixel representing a element in the matrix.</param>
        /// <returns>The RGBAData for a element in the matrix</returns>
        public override RGBAColor GetEffectValue(int TriggerValue, PixelData Pixel)
        {
     

            RGBAColor D = Pixel.GetRGBAColor();

            D.Red = (int)((float)D.Red * ActiveColor.Red / 255);
            D.Green = (int)((float)D.Green * ActiveColor.Green / 255);
            D.Blue = (int)((float)D.Blue * ActiveColor.Blue / 255);
            D.Alpha = (int)((float)Pixel.Alpha * TriggerValue / 255);

            return D;
            
        }
    }
}
