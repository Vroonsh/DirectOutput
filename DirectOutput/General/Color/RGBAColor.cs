﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using DirectOutput.General.Generic;

namespace DirectOutput.General.Color
{

    /// <summary>
    /// This class stores information on colors used for toys and effects (e.g. RGBLed).
    /// </summary>
    [EditorAttribute(typeof(ColorEditor), typeof(UITypeEditor))]
    [TypeConverter(typeof(ColorTypeConverter))]
    public class RGBAColor : IARGBConverter
    {



        private int _Red;

        /// <summary>
        /// Brightness for Red. 
        /// </summary>
        /// <value>Brightness between 0 and 255.</value>
        [XmlIgnoreAttribute]
        public int Red
        {
            get { return _Red; }
            set { _Red = value.Limit(0, 255); }
        }

        private int _Green;

        /// <summary>
        /// Brightness for Green.
        /// </summary>
        /// <value>Brightness between 0 and 255.</value>
        [XmlIgnoreAttribute]
        public int Green
        {
            get { return _Green; }
            set { _Green = value.Limit(0, 255); }
        }

        private int _Blue;
        /// <summary>
        /// Brightness for Blue.
        /// </summary>
        /// <value>Brightness between 0 and 255.</value>
        [XmlIgnoreAttribute]
        public int Blue
        {
            get { return _Blue; }
            set { _Blue = value.Limit(0, 255); }
        }

        private int _Alpha;
        /// <summary>
        /// Alpha value for the color.
        /// </summary>
        /// <value>Alpha value between 0 and 255.</value>
        [XmlIgnoreAttribute]
        public int Alpha
        {
            get { return _Alpha; }
            set { _Alpha = value.Limit(0, 255); }
        }


        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>A clone of this RGBAColor instance.</returns>
        public RGBAColor Clone()
        {
            return new RGBAColor(HexColor);
        }

        /// <summary>
        /// Returns the hexadecimal code for the color.
        /// </summary>
        /// <value>6 digit hexadecimal color code with leading  &#35;(e.g. &#35;ff0000 for red).</value>
        [Browsable(false)]
        public string HexColor
        {
            get
            {
                return "#{0:X2}{1:X2}{2:X2}{3:X2}".Build(new object[] { Red, Green, Blue, Alpha });
            }
            set
            {
                SetColor(value);
            }
        }

        /// <summary>
        /// Gets a new RGBAColor instance with the same color values.
        /// </summary>
        /// <returns></returns>
        public RGBAColor GetRGBAColor()
        {
            return new RGBAColor(Red, Green, Blue, Alpha);
        }

        /// <summary>
        /// IARGBConverter implementation of ToARGB
        /// </summary>
        /// <returns>argb color as int</returns>
        public int ToARGB() => (Alpha << 24) | (Red << 16) | (Green << 8) | Blue;
        /// <summary>
        /// IARGBConverter implementation of FromARGB
        /// </summary>
        /// <param name="argb">the argb color input</param>
        public void FromARGB(int argb)
        {
            Alpha = (int)((argb & 0xFF000000) >> 24);
            Red = (argb & 0xFF0000) >> 16;
            Green = (argb & 0xFF00) >> 8;
            Blue = (argb & 0xFF);
        }

        /// <summary>
        /// Sets the RGBA components of the Color.
        /// </summary>
        /// <param name="Red">Red brightness</param>
        /// <param name="Green">Green brightness</param>
        /// <param name="Blue">Blue brightness</param>
        /// <param name="Alpha">Alpha value for the color</param>
        /// <returns>true</returns>
        public bool SetColor(int Red, int Green, int Blue, int Alpha)
        {
            this.Red = Red;
            this.Blue = Blue;
            this.Green = Green;
            this.Alpha = Alpha;
            return true;
        }

        /// <summary>
        /// Sets the RGB components of the Color.<br/>
        /// The Alpha value is set to 0 if all color components are set to 0, otherwise the Alpha value will be set to 255.
        /// </summary>
        /// <param name="Red">Red brightness</param>
        /// <param name="Green">Green brightness</param>
        /// <param name="Blue">Blue brightness</param>
        /// <returns>true</returns>
        public bool SetColor(int Red, int Green, int Blue)
        {
            this.Red = Red;
            this.Blue = Blue;
            this.Green = Green;
            this.Alpha = (Red + Green + Blue > 0 ? 255 : 0);
            return true;
        }

        /// <summary>
        /// Sets the RGB components of the RGBAColor.<br />
        /// The Alpha value is set to 0 if all color components are set to 0, otherwise the Alpha value will be set to 255.
        /// </summary>
        /// <param name="Color">The RGB color to be set.</param>
        /// <returns></returns>
        public bool SetColor(RGBColor Color)
        {
            SetColor(Color.Red, Color.Green, Color.Blue);
            return true;
        }


        /// <summary>
        /// Sets the RGBA components of the Color.<br/>
        /// The parameter string <paramref name="Color"/> ist first parsed for hexadecimal color codes and afterwards checked for comma separated color values.<br/>
        /// The following values are accepted:<br/>
        /// 
        /// * Hexadecimal color code including alpha channel (e.g. &#35;ff0000FF for fully opaque red).<br/>
        /// * Hexadecimal color code without alpha channel (e.g. &#35;ff0000 for red). If all color components are set to 0 (that equals black) the alpha value is set to 0 (fully transparent), otherwise to 255 (fully opaque).
        /// * Comma separated color components including alpha channel (e.g. 255,0,0,128 for half transparent red).
        /// * Comma separated color components without alpha channel (e.g. 255,0,0 for for fully opque red). If all color components are set to 0 (that equals black) the alpha value is set to 0 (fully transparent), otherwise to 255 (fully opaque).
        /// 
        /// </summary>
        /// <param name="Color">String describing the color.</param>
        /// <returns>true if the parameter string contained a valid color code, otherwise false.</returns>
        public bool SetColor(string Color)
        {
            if ((Color.Length == 8 && Color.IsHexString()) || (Color.Length == 9 && Color.StartsWith("#") && Color.IsHexString(1)))
            {
                int Offset;
                if (Color.StartsWith("#"))
                {
                    Offset = 1;
                }
                else
                {
                    Offset = 0;
                }
                SetColor(Color.Substring(0 + Offset, 2).HexToInt(), Color.Substring(2 + Offset, 2).HexToInt(), Color.Substring(4 + Offset, 2).HexToInt(), Color.Substring(6 + Offset, 2).HexToInt());
                return true;
            };

            if ((Color.Length == 6 && Color.IsHexString()) || (Color.Length == 7 && Color.StartsWith("#") && Color.IsHexString(1)))
            {
                int Offset;
                if (Color.StartsWith("#"))
                {
                    Offset = 1;
                }
                else
                {
                    Offset = 0;
                }
                SetColor(Color.Substring(0 + Offset, 2).HexToInt(), Color.Substring(2 + Offset, 2).HexToInt(), Color.Substring(4 + Offset, 2).HexToInt());
                return true;
            };

            string[] SplitColors = Color.Split(',');
            if (SplitColors.Length == 3)
            {
                bool ColorsOK = true;
                foreach (string C in SplitColors)
                {
                    if (C.IsInteger())
                    {
                        ColorsOK = false;
                    }
                }
                if (ColorsOK)
                {
                    SetColor(int.Parse(SplitColors[0]), int.Parse(SplitColors[1]), int.Parse(SplitColors[2]));
                    return true;
                }
            }
            else if (SplitColors.Length == 4)
            {
                bool ColorsOK = true;
                foreach (string C in SplitColors)
                {
                    if (C.IsInteger())
                    {
                        ColorsOK = false;
                    }
                }
                if (ColorsOK)
                {
                    SetColor(int.Parse(SplitColors[0]), int.Parse(SplitColors[1]), int.Parse(SplitColors[2]), int.Parse(SplitColors[3]));
                    return true;
                }

            }
            return false;
        }

        #region Contructor
        /// <summary>
        /// Initializes a new instance of the <see cref="RGBAColor"/> class.
        /// </summary>
        public RGBAColor() { }


        /// <summary>
        /// Initializes a new instance of the <see cref="RGBAColor"/> class.<br/>
        /// If all color components are set to 0, the alpha value will be set to 0, otherwise the alpha value will be set to 255.
        /// </summary>
        /// <param name="BrightnessRed">The brightness for red.</param>
        /// <param name="BrightnessGreen">The brightness for green.</param>
        /// <param name="BrightnessBlue">The brightness for blue.</param>
        public RGBAColor(int BrightnessRed, int BrightnessGreen, int BrightnessBlue)
        {
            SetColor(BrightnessRed, BrightnessGreen, BrightnessBlue);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RGBAColor"/> class.
        /// </summary>
        /// <param name="BrightnessRed">The brightness for red.</param>
        /// <param name="BrightnessGreen">The brightness for green.</param>
        /// <param name="BrightnessBlue">The brightness for blue.</param>
        /// <param name="Alpha">The alpha value for the color.</param>
        public RGBAColor(int BrightnessRed, int BrightnessGreen, int BrightnessBlue, int Alpha)
        {
            SetColor(BrightnessRed, BrightnessGreen, BrightnessBlue, Alpha);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="RGBAColor"/> class.<br/>
        /// The parameter string <paramref name="Color"/> is first parsed for hexadecimal color codes and afterwards checked for comma separated color values.<br/>
        /// The following values are accepted:<br/>
        /// 
        /// * Hexadecimal color code including alpha channel (e.g. &#35;ff0000FF for fully opaque red).<br/>
        /// * Hexadecimal color code without alpha channel (e.g. &#35;ff0000 for red). If all color components are set to 0 (that equals black) the alpha value is set to 0 (fully transparent), otherwise to 255 (fully opaque).
        /// * Comma separated color components including alpha channel (e.g. 255,0,0,128 for half transparent red).
        /// * Comma separated color components without alpha channel (e.g. 255,0,0 for for fully opque red). If all color components are set to 0 (that equals black) the alpha value is set to 0 (fully transparent), otherwise to 255 (fully opaque).
        /// </summary>
        /// <param name="Color">The color string.</param>
        public RGBAColor(string Color)
        {
            SetColor(Color);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="RGBAColor"/> class.
        /// The Alpha value is set to 0 if all color components are set to 0, otherwise the Alpha value will be set to 255.
        /// </summary>
        /// <param name="RGBColor">RGBColor object.</param>
        public RGBAColor(RGBColor RGBColor)
        {
            SetColor(RGBColor);
        }

        #endregion



       new public string ToString()
        {
            return HexColor;
        }
    }
}
