using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DirectOutput.FX;
using DirectOutput.FX.AnalogToyFX;
using DirectOutput.FX.ConditionFX;
using DirectOutput.FX.MatrixFX;
using DirectOutput.FX.RGBAFX;
using DirectOutput.FX.TimmedFX;
using DirectOutput.FX.ValueFX;
using DirectOutput.General.Analog;
using DirectOutput.General.Color;
using DirectOutput.Table;

namespace DirectOutput.LedControl.Loader
{
    /// <summary>
    /// A single setting from a LedControl.ini file.
    /// </summary>
    public class TableConfigSetting
    {


        /// <summary>
        /// Defines the control mode for a output. It can be constantly on, off or it can be controlled by a element of a pinball table.
        /// </summary>
        /// <value>
        /// The output control enum value.
        /// </value>
        [Category("Trigger")]
        [DisplayName("Output Control")]
        [Description("Defines the control mode for a output. It can be constantly on, off or it can be controlled by a element of a pinball table.")]
        [DefaultValue(OutputControlEnum.FixedOn)]
        public OutputControlEnum OutputControl { get; set; }

        /// <summary>
        /// Gets or sets the name of the color of the setting.<br/>
        /// This should only be set for RGB outputs.
        /// </summary>
        /// <value>
        /// The name of the color as specified in the color section of the Ledcontrol.ini file.
        /// </value>
        [Category("Colors")]
        [DisplayName("Color Name")]
        [Description("The name of the color of the setting.\nThis should only be set for RGB outputs.")]
        [DefaultValue("")]
        public string ColorName { get; set; }



        /// <summary>
        /// Gets or sets the color config.
        /// </summary>
        /// <value>
        /// The color config.
        /// </value>
        [Category("Colors")]
        [DisplayName("Color Config")]
        [Description("The color config attached to the Color Name.")]
        public ColorConfig ColorConfig { get; set; }


        /// <summary>
        /// The table element triggering the effect (if available)
        /// </summary>
        [Category("Trigger")]
        [DisplayName("Table Element")]
        [Description("The table element triggering the effect (if available)")]
        public string TableElement { get; set; } = null;


        /// <summary>
        /// The condition if available.
        /// </summary>
        [Category("Trigger")]
        [DisplayName("Condition")]
        [Description("The condition if available.")]
        public string Condition { get; set; } = null;


        /// <summary>
        /// Gets the type of the output.<br/>
        /// The value of this property depends on the value of the ColorName property.
        /// </summary>
        /// <value>
        /// The type of the output.
        /// </value>
        [DisplayName("Output Type")]
        [Description("The type of the output.\nThe value of this property depends on the value of the ColorName property.")]
        [ReadOnly(true)]
        public OutputTypeEnum OutputType
        {
            get
            {
                return (!ColorName.IsNullOrWhiteSpace() ? OutputTypeEnum.RGBOutput : OutputTypeEnum.AnalogOutput);
            }
        }

        /// <summary>
        /// Gets or sets the duration in milliseconds.
        /// </summary>
        /// <value>
        /// The duration in milliseconds.
        /// </value>
        [Category("Timers")]
        [DisplayName("Duration (ms)")]
        [Description("The duration in milliseconds.")]
        [DefaultValue(0)]
        public int DurationMs { get; set; }

        private int _MinDurationMs = 0;

        /// <summary>
        /// Gets or sets the minimum duration in milliseconds.
        /// </summary>
        /// <value>
        /// The minimum duration in milliseconds.
        /// </value>
        [Category("Timers")]
        [DisplayName("Min Duration (ms)")]
        [Description("The minimum duration in milliseconds.")]
        [DefaultValue(0)]
        public int MinDurationMs
        {
            get { return _MinDurationMs; }
            set { _MinDurationMs = value; }
        }


        /// <summary>
        /// Gets or sets the max duration for the effect in milliseconds.
        /// </summary>
        /// <value>
        /// The max duration of the effect in milliseconds.
        /// </value>
        [Category("Timers")]
        [DisplayName("Max Duration (ms)")]
        [Description("The max duration for the effect in milliseconds.")]
        [DefaultValue(0)]
        public int MaxDurationMs { get; set; }

        /// <summary>
        /// Gets or sets the extended duration for the effect in milliseconds.
        /// </summary>
        /// <value>
        /// The extended duration of the effect in milliseconds.
        /// </value>
        [Category("Timers")]
        [DisplayName("Extended Duration (ms)")]
        [Description("The extended duration for the effect in milliseconds.")]
        [DefaultValue(0)]
        public int ExtDurationMs { get; set; }

        private int _Intensity;
        /// <summary>
        /// Gets or sets the intensity.<br/>
        /// If the property <see cref="ColorName"/> is set, this property will always return -1.
        /// </summary>
        /// <value>
        /// The intensity.
        /// </value>
        [Category("Colors")]
        [DisplayName("Intensity")]
        [Description("The intensity.\nIf the property \"Color Name\" is set, this property will always be -1.")]
        [DefaultValue(-1)]
        public int Intensity
        {

            get { return (ColorName.IsNullOrWhiteSpace() ? _Intensity : -1); }
            set { _Intensity = value; }
        }

        private int _FadingDurationUpMs = 0;

        /// <summary>
        /// Gets or sets the duration for fading up in milliseconds.
        /// </summary>
        /// <value>
        /// The duration of the fading in milliseconds.
        /// </value>
        [Category("Fade")]
        [DisplayName("Fading Up Duration (ms)")]
        [Description("The duration for fading up in milliseconds.")]
        [DefaultValue(0)]
        public int FadingUpDurationMs
        {
            get { return _FadingDurationUpMs; }
            set { _FadingDurationUpMs = value; }
        }

        private int _FadingDownDurationMs = 0;

        /// <summary>
        /// Gets or sets the duration for fading down in milliseconds.
        /// </summary>
        /// <value>
        /// The duration of the fading in milliseconds.
        /// </value>
        [Category("Fade")]
        [DisplayName("Fading Down Duration (ms)")]
        [Description("The duration for fading down in milliseconds.")]
        [DefaultValue(0)]
        public int FadingDownDurationMs
        {
            get { return _FadingDownDurationMs; }
            set { _FadingDownDurationMs = value; }
        }

        /// <summary>
        /// Gets or sets the number blinks.
        /// </summary>
        /// <value>
        /// The number of blinks. -1 means infinite number of blinks.
        /// </value>
        [Category("Blink")]
        [DisplayName("Blink Numbers")]
        [Description("The number blinks.\n-1 means infinite number of blinks.")]
        [DefaultValue(0)]
        public int Blink { get; set; }

        /// <summary>
        /// Gets or sets the blink interval in milliseconds.
        /// </summary>
        /// <value>
        /// The blink interval in  milliseconds.
        /// </value>
        [Category("Blink")]
        [DisplayName("Blink Interval (ms)")]
        [Description("The blink interval in milliseconds.")]
        [DefaultValue(0)]
        public int BlinkIntervalMs { get; set; }


        /// <summary>
        /// Gets or sets the blink interval in milliseconds for nested blinking.
        /// </summary>
        /// <value>
        /// The blink interval in milliseconds for nested blinking.
        /// </value>
        [Category("Blink")]
        [DisplayName("Blink Interval Nested (ms)")]
        [Description("The blink interval in milliseconds for nested blinking.")]
        [DefaultValue(0)]
        public int BlinkIntervalMsNested { get; set; }

        private int _BlinkPulseWidthNested = 50;

        /// <summary>
        /// Gets or sets the width of the blink pulse for nested blinking.
        /// Value must be between 1 and 99 (defaults to 50).
        /// </summary>
        /// <value>
        /// The width of the blink pulse for nested blinking.
        /// </value>
        [Category("Blink")]
        [DisplayName("Blink Pulse Width Nested (%)")]
        [Description("The width of the blink pulse for nested blinking.\nValue must be between 1 and 99 (defaults to 50).")]
        [DefaultValue(50)]
        public int BlinkPulseWidthNested
        {
            get { return _BlinkPulseWidthNested; }
            set { _BlinkPulseWidthNested = value.Limit(1, 99); }
        }

        private int _BlinkPulseWidth = 50;

        /// <summary>
        /// Gets or sets the width of the blink pulse.
        /// Value must be between 1 and 99 (defaults to 50).
        /// </summary>
        /// <value>
        /// The width of the blink pulse.
        /// </value>
        [Category("Blink")]
        [DisplayName("Blink Pulse Width (%)")]
        [Description("The width of the blink pulse.\nValue must be between 1 and 99 (defaults to 50).")]
        [DefaultValue(50)]
        public int BlinkPulseWidth
        {
            get { return _BlinkPulseWidth; }
            set { _BlinkPulseWidth = value.Limit(1, 99); }
        }


        private int _BlinkLow = 0;

        [Category("Blink")]
        [DisplayName("Blink Low Intensity")]
        [Description("The width of the blink low intensity.\nValue must be between 0 and 255 (defaults to 0).")]
        [DefaultValue(0)]
        public int BlinkLow
        {
            get { return _BlinkLow; }
            set { _BlinkLow = value.Limit(0, 255); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the trigger value for the effect is inverted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if invert; otherwise, <c>false</c>.
        /// </value>
        [DisplayName("Invert")]
        [Description("A value indicating whether the trigger value for the effect is inverted.")]
        [DefaultValue(false)]
        public bool Invert { get; set; }

        /// <summary>
        /// Indicates the the trigger value of the effect is not to be treated as a boolean value resp. that the value should not be mapped to 0 or 255 (255 for all values which are not 0).
        /// </summary>
        /// <value>
        ///   <c>true</c> if [no bool]; otherwise, <c>false</c>.
        /// </value>
        [DisplayName("NoBool")]
        [Description("Indicates the trigger value of the effect is not to be treated as a boolean value resp. that the value should not be mapped to 0 or 255 (255 for all values which are not 0).")]
        [DefaultValue(false)]
        public bool NoBool { get; set; }


        /// <summary>
        /// Gets or sets the wait duration before the effect is triggered.
        /// </summary>
        /// <value>
        /// The wait duration in milliseconds
        /// </value>
        [Category("Timers")]
        [DisplayName("Wait Duration (ms)")]
        [Description("The wait duration before the effect is triggered.")]
        [DefaultValue(0)]
        public int WaitDurationMs { get; set; }


        /// <summary>
        /// Gets or sets the layer for the settings.
        /// </summary>
        /// <value>
        /// The layer for the settings.
        /// </value>
        [DisplayName("Layer")]
        [Description("The layer for the settings.")]
        public int? Layer { get; set; }


        [Category("Area")]
        [DisplayName("Left (%)")]
        [Description("The left position of the area effect in percentage.")]
        [DefaultValue(0)]
        public int AreaLeft { get; set; } = 0;
        [Category("Area")]
        [DisplayName("Top (%)")]
        [Description("The top position of the area effect in percentage.")]
        [DefaultValue(0)]
        public int AreaTop { get; set; } = 0;
        [Category("Area")]
        [DisplayName("Width (%)")]
        [Description("The width of the area effect in percentage.")]
        [DefaultValue(100)]
        public int AreaWidth { get; set; } = 100;
        [Category("Area")]
        [DisplayName("Height (%)")]
        [Description("The height of the area effect in percentage.")]
        [DefaultValue(100)]
        public int AreaHeight { get; set; } = 100;
        [Category("Shift")]
        [DisplayName("Speed")]
        [Description("The shift speed in percentage of the effect area per second.\n" +
                     "A speed setting of 100 will shift through the whole area of the effect in 1 second.\n" +
                     "200 will shift through the effect area in .5 seconds. 33 will shift through the effect area in approx. 3 seconds.\n" +
                     "Max. speed is 10000 (shift through the effect area in 1/100 seconds). Min. speed is 1 (shifts through the effect area in 100 seconds).")]
        [DefaultValue(100)]
        public int AreaSpeed { get; set; } = 100;
        [Category("Shift")]
        [DisplayName("Acceleration")]
        [Description("The acceleration for the shift speed in percent of the effect area per second.\n" +
                     "Acceleration can be zero, positive or negative. Positive values will increase the shift speed.\n" +
                     "Speed will be increased up to a max value of 10000. Negative values will decrease the shift speed.\n" +
                     "Speed will be decreased down to a minimum speed of 1.")]
        [DefaultValue(0)]
        public int AreaAcceleration { get; set; } = 0;
        [Category("Flicker")]
        [DisplayName("Density (%)")]
        [Description("The density of the flickering in percent.\n" +
                     "For 0 no elements of the defined area will will flicker, for 50 half of the elements will flicker, for 100 all elements will flicker.")]
        [DefaultValue(0)]
        public int AreaFlickerDensity { get; set; } = 0;
        [Category("Flicker")]
        [DisplayName("Min Duration (ms)")]
        [Description("The min duration in milliseconds for a single flicker/blink of a element.")]
        [DefaultValue(0)]
        public int AreaFlickerMinDurationMs { get; set; } = 0;
        [Category("Flicker")]
        [DisplayName("Max Duration (ms)")]
        [Description("The max duration in milliseconds for a single flicker/blink of a element.")]
        [DefaultValue(0)]
        public int AreaFlickerMaxDurationMs { get; set; } = 0;
        [Category("Flicker")]
        [DisplayName("Fade Duration (ms)")]
        [Description("The up & down fade durations in milliseconds for a single flicker/blink of a element.")]
        [DefaultValue(0)]
        public int AreaFlickerFadeDurationMs { get; set; } = 0;
        [Category("Shift")]
        [DisplayName("Direction")]
        [Description("The shift direction resp. the direction in which the color moves (Left, Right, Up, Down).")]
        [DefaultValue(MatrixShiftDirectionEnum.Invalid)]
        public MatrixShiftDirectionEnum AreaDirection { get; set; } = MatrixShiftDirectionEnum.Invalid;
        public bool IsArea = false;

        public bool IsBitmap = false;
        [Category("Bitmap")]
        [DisplayName("Top (pixels)")]
        [Description("The top position in pixels of the bitmap area in the reference image.")]
        [DefaultValue(0)]
        public int AreaBitmapTop { get; set; } = 0;
        [Category("Bitmap")]
        [DisplayName("Left (pixels)")]
        [Description("The left position in pixels of the bitmap area in the reference image.")]
        [DefaultValue(0)]
        public int AreaBitmapLeft { get; set; } = 0;
        [Category("Bitmap")]
        [DisplayName("Width (pixels)")]
        [Description("The width in pixels of the bitmap area in the reference image.")]
        [DefaultValue(-1)]
        public int AreaBitmapWidth { get; set; } = -1;
        [Category("Bitmap")]
        [DisplayName("Height (pixels)")]
        [Description("The height in pixels of the bitmap area in the reference image.")]
        [DefaultValue(-1)]
        public int AreaBitmapHeight { get; set; } = -1;
        [Category("Bitmap")]
        [DisplayName("Frame")]
        [Description("The frame used in the reference image, if it's a Gif.")]
        [DefaultValue(0)]
        public int AreaBitmapFrame { get; set; } = 0;
        [Category("Bitmap")]
        [DisplayName("Animation Step Size (pixels)")]
        [Description("The step in pixels between two steps of the bitmap animation.\nStep Size could be negative.")]
        [DefaultValue(1)]
        public int AreaBitmapAnimationStepSize { get; set; } = 1;
        [Category("Bitmap")]
        [DisplayName("Animation Step Count")]
        [Description("The number of steps of the bitmap animation.")]
        [DefaultValue(0)]
        public int AreaBitmapAnimationStepCount { get; set; } = 0;
        [Category("Bitmap")]
        [DisplayName("Animation Step Duration (ms)")]
        [Description("The delay in milliseconds between steps of the bitmap animation.")]
        [DefaultValue(30)]
        public int AreaBitmapAnimationFrameDuration { get; set; } = 30;
        [Category("Bitmap")]
        [DisplayName("Animation Direction")]
        [Description("The direction of the bitmap animation.\n" +
                     "Frame : animation goes through image frames (Gif only).\n" +
                     "Right/Down : animation shift through image pixels to the right/down using StepSize")]
        [DefaultValue(MatrixAnimationStepDirectionEnum.Frame)]
        public MatrixAnimationStepDirectionEnum AreaBitmapAnimationDirection { get; set; } = MatrixAnimationStepDirectionEnum.Frame;
        [Category("Bitmap")]
        [DisplayName("Animation Behaviour")]
        [Description("The behaviour of the bitmap animation.\n" +
                     "Once : restarts when triggered, play only once.\n" +
                     "Loop : restarts when triggered, play in loop" +
                     "Continue : continue existing animation when triggered then loops.")]
        [DefaultValue(AnimationBehaviourEnum.Loop)]
        public AnimationBehaviourEnum AreaBitmapAnimationBehaviour { get; set; } = AnimationBehaviourEnum.Loop;

        [Category("Shape")]
        [DisplayName("Shape Name")]
        [Description("The name of a predifined shape from DirectOutputShapes.xml")]
        [DefaultValue("")]
        public string ShapeName { get; set; } = string.Empty;


        public bool IsPlasma = false;
        [Category("Plasma")]
        [DisplayName("Speed")]
        [Description("The plasma speed")]
        [DefaultValue(100)]
        public int PlasmaSpeed { get; set; } = 100;
        [Category("Plasma")]
        [DisplayName("Density")]
        [Description("The plasma density")]
        [DefaultValue(100)]
        public int PlasmaDensity { get; set; } = 100;
        [Category("Plasma")]
        [DisplayName("Color Name 2")]
        [Description("The plasma secondary color name")]
        [DefaultValue("")]
        public string ColorName2 { get; set; } = string.Empty;
        [Category("Plasma")]
        [DisplayName("Color Config 2")]
        [Description("The plasma secondary color config")]
        [DefaultValue("")]
        public ColorConfig ColorConfig2 { get; set; } = null;


        //public int PlasmaScale = 100;

        /// <summary>
        /// Parses the setting data. <br />
        /// </summary>
        /// <param name="SettingData">The setting data.</param>
        /// <exception cref="System.Exception">
        /// No data to parse.
        /// or
        /// Cant parse the part {0} of the ledcontrol table config setting {1}..
        /// </exception>
        public void ParseSettingData(string SettingData)
        {
            string S = SettingData.Trim().ToUpper();

            if (S.StartsWith("("))
            {
                //It is a condition

                int BracketCnt = 1;
                int ClosingBracketPos = -1;
                for (int i = 1; i < S.Length; i++)
                {
                    if (S[i] == '(') { BracketCnt++; }
                    else if (S[i] == ')') { BracketCnt--; }

                    if (BracketCnt == 0)
                    {
                        ClosingBracketPos = i;
                        break;
                    }
                }


                if (ClosingBracketPos > 0)
                {
                    Condition = S.Substring(0, ClosingBracketPos + 1);
                    OutputControl = OutputControlEnum.Condition;
                    S = S.Substring(Condition.Length).Trim();
                    //TODO: Maybe add a check for the condition validity

                }
                else
                {
                    Log.Warning("No closing bracket found for condition in setting {0}.".Build(S));

                    throw new Exception("No closing bracket found for condition in setting {0}.".Build(S));
                }




            }
            else
            {
                //not a condition

                if (S.Length == 0)
                {
                    Log.Warning("No data to parse.");

                    throw new Exception("No data to parse.");
                }

                int TriggerEndPos = -1;
                char LastChar = (char)0;
                for (int i = 0; i < S.Length - 1; i++)
                {
                    if (S[i] == ' ' && LastChar != '|' && LastChar != (char)0)
                    {
                        TriggerEndPos = i;
                        break;
                    }
                    if (S[i] != ' ')
                    {
                        LastChar = S[i];
                    }
                }
                if (TriggerEndPos == -1) TriggerEndPos = S.Length;

                string Trigger = S.Substring(0, TriggerEndPos).Trim();



                //Get output state and table element (if applicable)
                bool ParseOK = true;
                switch (Trigger.ToUpper())
                {
                    case "ON":
                    case "1":
                        OutputControl = OutputControlEnum.FixedOn;
                        break;
                    case "OFF":
                    case "0":
                        OutputControl = OutputControlEnum.FixedOff;
                        break;
                    case "B":
                    case "BLINK":
                        OutputControl = OutputControlEnum.FixedOn;
                        Blink = -1;
                        BlinkIntervalMs = 1000;
                        break;
                    default:
                        string[] ATE = Trigger.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(Tr => Tr.Trim()).ToArray();
                        foreach (string E in ATE)
                        {
                            if (E.Length > 1)
                            {
                                if (E[0] == (char)TableElementTypeEnum.NamedElement && E.Substring(1).All(C => char.IsLetterOrDigit(C) || C == '_'))
                                {
                                    //Named element
                                }
                                else if (Enum.IsDefined(typeof(TableElementTypeEnum), (int)E[0]) && E.Substring(1).IsInteger())
                                {
                                    //Normal table element
                                }
                                else
                                {
                                    Log.Error("Failed: " + E);
                                    ParseOK = false;
                                    break;
                                }
                            }
                            else
                            {
                                ParseOK = false;
                                break;
                            }


                        }
                        if (ParseOK)
                        {
                            OutputControl = OutputControlEnum.Controlled;
                            TableElement = Trigger;
                        }



                        break;
                }
                if (!ParseOK)
                {
                    Log.Warning("Cant parse the trigger part {0} of the ledcontrol table config setting {1}.".Build(Trigger, SettingData));

                    throw new Exception("Cant parse the part {0} of the ledcontrol table config setting {1}.".Build(Trigger, SettingData));
                }

                //Remove first part
                S = S.Substring(Trigger.Length).Trim();

            }


            string[] Parts = S.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);


            int IntegerCnt = 0;
            int PartNr = 0;

            while (Parts.Length > PartNr)
            {

                if (Parts[PartNr].ToUpper() == "BLINK")
                {
                    Blink = -1;
                    BlinkIntervalMs = 1000;
                }
                else if (Parts[PartNr].ToUpper() == "INVERT")
                {
                    Invert = true;
                }
                else if (Parts[PartNr].ToUpper() == "NOBOOL")
                {
                    NoBool = true;
                }
                else if (Parts[PartNr].Length > 3 && Parts[PartNr].Substring(0, 3).ToUpper() == "APS" && Parts[PartNr].Substring(3).IsInteger())
                {
                    PlasmaSpeed = Parts[PartNr].Substring(3).ToInteger();
                    IsPlasma = true;
                    IsArea = true;
                }
                else if (Parts[PartNr].Length > 3 && Parts[PartNr].Substring(0, 3).ToUpper() == "APD" && Parts[PartNr].Substring(3).IsInteger())
                {
                    PlasmaDensity = Parts[PartNr].Substring(3).ToInteger();
                    IsPlasma = true;
                    IsArea = true;
                }
                else if (Parts[PartNr].Length > 3 && Parts[PartNr].Substring(0, 3).ToUpper() == "APC")
                {
                    ColorName2 = Parts[PartNr].Substring(3);
                    IsPlasma = true;
                    IsArea = true;
                }
                //else if (Parts[PartNr].Length > 3 && Parts[PartNr].Substring(0, 3).ToUpper() == "APC" && Parts[PartNr].Substring(3).IsInteger())
                //{
                //    PlasmaScale = Parts[PartNr].Substring(3).ToInteger();
                //    IsPlasma = true;
                //    IsArea = true;
                //}


                else if (Parts[PartNr].Length > 3 && Parts[PartNr].Substring(0, 3).ToUpper() == "SHP" )
                {
                    ShapeName = Parts[PartNr].Substring(3).Trim();
                    IsArea = true;
                }
                else if (Parts[PartNr].Length > 3 && Parts[PartNr].Substring(0, 3).ToUpper() == "ABT" && Parts[PartNr].Substring(3).IsInteger())
                {
                    AreaBitmapTop = Parts[PartNr].Substring(3).ToInteger();
                    IsArea = true;
                    IsBitmap = true;
                }
                else if (Parts[PartNr].Length > 3 && Parts[PartNr].Substring(0, 3).ToUpper() == "ABL" && Parts[PartNr].Substring(3).IsInteger())
                {
                    AreaBitmapLeft = Parts[PartNr].Substring(3).ToInteger();
                    IsArea = true;
                    IsBitmap = true;
                }
                else if (Parts[PartNr].Length > 3 && Parts[PartNr].Substring(0, 3).ToUpper() == "ABW" && Parts[PartNr].Substring(3).IsInteger())
                {
                    AreaBitmapWidth = Parts[PartNr].Substring(3).ToInteger();
                    IsArea = true;
                    IsBitmap = true;
                }
                else if (Parts[PartNr].Length > 3 && Parts[PartNr].Substring(0, 3).ToUpper() == "ABH" && Parts[PartNr].Substring(3).IsInteger())
                {
                    AreaBitmapHeight = Parts[PartNr].Substring(3).ToInteger();
                    IsArea = true;
                    IsBitmap = true;
                }
                else if (Parts[PartNr].Length > 3 && Parts[PartNr].Substring(0, 3).ToUpper() == "ABF" && Parts[PartNr].Substring(3).IsInteger())
                {
                    AreaBitmapFrame = Parts[PartNr].Substring(3).ToInteger();
                    IsArea = true;
                    IsBitmap = true;
                }
                else if (Parts[PartNr].Length > 3 && Parts[PartNr].Substring(0, 3).ToUpper() == "AAS" && Parts[PartNr].Substring(3).IsInteger())
                {
                    AreaBitmapAnimationStepSize = Parts[PartNr].Substring(3).ToInteger();
                    IsArea = true;
                    IsBitmap = true;
                }
                else if (Parts[PartNr].Length > 3 && Parts[PartNr].Substring(0, 3).ToUpper() == "AAC" && Parts[PartNr].Substring(3).IsInteger())
                {
                    AreaBitmapAnimationStepCount = Parts[PartNr].Substring(3).ToInteger();
                    IsArea = true;
                    IsBitmap = true;
                }
                else if (Parts[PartNr].Length > 3 && Parts[PartNr].Substring(0, 3).ToUpper() == "AAF" && Parts[PartNr].Substring(3).IsInteger())
                {
                    AreaBitmapAnimationFrameDuration = 1000 / Parts[PartNr].Substring(3).ToInteger().Limit(10, int.MaxValue);
                    IsArea = true;
                    IsBitmap = true;
                }
                else if (Parts[PartNr].Length == 4 && Parts[PartNr].Substring(0, 3).ToUpper() == "AAD" && Enum.IsDefined(typeof(MatrixAnimationStepDirectionEnum), (int)Parts[PartNr].Substring(3, 1).ToUpper()[0]))
                {

                    AreaBitmapAnimationDirection = (MatrixAnimationStepDirectionEnum)Parts[PartNr].Substring(3, 1).ToUpper()[0];
                    IsArea = true;
                    IsBitmap = true;
                }
                else if (Parts[PartNr].Length == 4 && Parts[PartNr].Substring(0, 3).ToUpper() == "AAB" && Enum.IsDefined(typeof(AnimationBehaviourEnum), (int)Parts[PartNr].Substring(3, 1).ToUpper()[0]))
                {

                    AreaBitmapAnimationBehaviour = (AnimationBehaviourEnum)Parts[PartNr].Substring(3, 1).ToUpper()[0];
                    IsArea = true;
                    IsBitmap = true;
                }

                else if (Parts[PartNr].Length > 5 && Parts[PartNr].Substring(0, 5).ToUpper() == "AFDEN" && Parts[PartNr].Substring(5).IsInteger())
                {
                    AreaFlickerDensity = Parts[PartNr].Substring(5).ToInteger();
                    IsArea = true;
                }
                else if (Parts[PartNr].Length > 5 && Parts[PartNr].Substring(0, 5).ToUpper() == "AFMIN" && Parts[PartNr].Substring(5).IsInteger())
                {
                    AreaFlickerMinDurationMs = Parts[PartNr].Substring(5).ToInteger();
                    IsArea = true;
                }
                else if (Parts[PartNr].Length > 5 && Parts[PartNr].Substring(0, 5).ToUpper() == "AFMAX" && Parts[PartNr].Substring(5).IsInteger())
                {
                    AreaFlickerMaxDurationMs = Parts[PartNr].Substring(5).ToInteger();
                    IsArea = true;
                }
                else if (Parts[PartNr].Length > 6 && Parts[PartNr].Substring(0, 6).ToUpper() == "AFFADE" && Parts[PartNr].Substring(6).IsInteger())
                {
                    AreaFlickerFadeDurationMs = Parts[PartNr].Substring(6).ToInteger();

                    IsArea = true;
                }
                else if (Parts[PartNr].Length > 2 && Parts[PartNr].Substring(0, 2).ToUpper() == "AT" && Parts[PartNr].Substring(2).IsInteger())
                {
                    AreaTop = Parts[PartNr].Substring(2).ToInteger().Limit(0, 100);
                    IsArea = true;
                }
                else if (Parts[PartNr].Length > 2 && Parts[PartNr].Substring(0, 2).ToUpper() == "AL" && Parts[PartNr].Substring(2).IsInteger())
                {
                    AreaLeft = Parts[PartNr].Substring(2).ToInteger().Limit(0, 100);
                    IsArea = true;
                }
                else if (Parts[PartNr].Length > 2 && Parts[PartNr].Substring(0, 2).ToUpper() == "AW" && Parts[PartNr].Substring(2).IsInteger())
                {
                    AreaWidth = Parts[PartNr].Substring(2).ToInteger().Limit(0, 100);
                    IsArea = true;
                }
                else if (Parts[PartNr].Length > 2 && Parts[PartNr].Substring(0, 2).ToUpper() == "AH" && Parts[PartNr].Substring(2).IsInteger())
                {
                    AreaHeight = Parts[PartNr].Substring(2).ToInteger().Limit(0, 100);
                    IsArea = true;
                }
                //TODO: Remove parameter AA
                else if (Parts[PartNr].Length > 2 && Parts[PartNr].Substring(0, 2).ToUpper() == "AA" && Parts[PartNr].Substring(2).IsInteger())
                {
                    AreaAcceleration = Parts[PartNr].Substring(2).ToInteger();
                    IsArea = true;
                }
                else if (Parts[PartNr].Length > 3 && Parts[PartNr].Substring(0, 3).ToUpper() == "ASA" && Parts[PartNr].Substring(3).IsInteger())
                {
                    AreaAcceleration = Parts[PartNr].Substring(3).ToInteger();
                    IsArea = true;
                }

                    //TODO:Remove AS para
                else if (Parts[PartNr].Length > 2 && Parts[PartNr].Substring(0, 2).ToUpper() == "AS" && Parts[PartNr].Substring(2).IsInteger())
                {
                    AreaSpeed = Parts[PartNr].Substring(2).ToInteger().Limit(1, 10000);
                    IsArea = true;
                }
                else if (Parts[PartNr].Length > 3 && Parts[PartNr].Substring(0, 3).ToUpper() == "ASS" && Parts[PartNr].Substring(3).IsInteger())
                {
                    AreaSpeed = Parts[PartNr].Substring(3).ToInteger().Limit(1, 10000);
                    IsArea = true;
                }
                else if (Parts[PartNr].Length > 5 && Parts[PartNr].Substring(0, 3).ToUpper() == "ASS" && Parts[PartNr].ToUpper().Right(2) == "MS" && Parts[PartNr].Substring(3, Parts[PartNr].Length - 5).IsInteger())
                {
                    AreaSpeed = (int)((double)100000 / Parts[PartNr].Substring(3, Parts[PartNr].Length - 5).ToInteger()).Limit(10, 100000);
                    IsArea = true;
                }

                    //TODO:Remove AD para
                else if (Parts[PartNr].Length == 3 && Parts[PartNr].Substring(0, 2).ToUpper() == "AD" && Enum.IsDefined(typeof(MatrixShiftDirectionEnum), (int)Parts[PartNr].Substring(2, 1).ToUpper()[0]))
                {

                    AreaDirection = (MatrixShiftDirectionEnum)Parts[PartNr].Substring(2, 1).ToUpper()[0];
                    IsArea = true;
                }
                else if (Parts[PartNr].Length == 4 && Parts[PartNr].Substring(0, 3).ToUpper() == "ASD" && Enum.IsDefined(typeof(MatrixShiftDirectionEnum), (int)Parts[PartNr].Substring(3, 1).ToUpper()[0]))
                {

                    AreaDirection = (MatrixShiftDirectionEnum)Parts[PartNr].Substring(3, 1).ToUpper()[0];
                    IsArea = true;
                }
                else if (Parts[PartNr].Length > 3 && Parts[PartNr].ToUpper().Substring(0, 3) == "MAX" && Parts[PartNr].Substring(3).IsInteger())
                {
                    MaxDurationMs = Parts[PartNr].Substring(3).ToInteger().Limit(0, int.MaxValue);
                }
                else if (Parts[PartNr].Length > 3 && Parts[PartNr].ToUpper().Substring(0, 3) == "BNP" && Parts[PartNr].Substring(3).IsInteger())
                {
                    BlinkIntervalMsNested = Parts[PartNr].Substring(3).ToInteger().Limit(0, int.MaxValue);
                }
                else if (Parts[PartNr].Length > 4 && Parts[PartNr].ToUpper().Substring(0, 4) == "BNPW" && Parts[PartNr].Substring(4).IsInteger())
                {
                    BlinkPulseWidthNested = Parts[PartNr].Substring(4).ToInteger().Limit(1, 99);
                }

                else if (Parts[PartNr].Length > 3 && Parts[PartNr].ToUpper().Substring(0, 3) == "BPW" && Parts[PartNr].Substring(3).IsInteger())
                {
                    BlinkPulseWidth = Parts[PartNr].Substring(3).ToInteger().Limit(1, 99);
                }
                else if (Parts[PartNr].Length > 3 && Parts[PartNr].ToUpper().Substring(0, 3) == "BL#" && Parts[PartNr].Substring(3).IsHexString())
                {

                    BlinkLow = Parts[PartNr].Substring(3).HexToInt().Limit(0, 255);
                }
                else if (Parts[PartNr].Length > 1 && Parts[PartNr].ToUpper().Substring(0, 1) == "BL" && Parts[PartNr].Substring(1).IsInteger())
                {

                    BlinkLow = (int)(((double)Parts[PartNr].Substring(2).ToInteger().Limit(0, 48)) * 5.3125);
                }
                else if (Parts[PartNr].Length > 1 && Parts[PartNr].ToUpper().Substring(0, 1) == "E" && Parts[PartNr].Substring(1).IsInteger())
                {

                    ExtDurationMs = Parts[PartNr].Substring(1).ToInteger().Limit(0, int.MaxValue);
                }
                else if (Parts[PartNr].Length > 2 && Parts[PartNr].ToUpper().Substring(0, 2) == "I#" && Parts[PartNr].Substring(2).IsHexString())
                {
                    //Intensity setting
                    Intensity = Parts[PartNr].Substring(2).HexToInt().Limit(0, 255);
                }
                else if (Parts[PartNr].Length > 1 && Parts[PartNr].ToUpper().Substring(0, 1) == "I" && Parts[PartNr].Substring(1).IsInteger())
                {
                    //Intensity setting
                    Intensity = (int)(((double)Parts[PartNr].Substring(1).ToInteger().Limit(0, 48)) * 5.3125);
                }
                else if (Parts[PartNr].Length > 1 && Parts[PartNr].ToUpper().Substring(0, 1) == "L" && Parts[PartNr].Substring(1).IsInteger())
                {
                    //Layer setting
                    Layer = Parts[PartNr].Substring(1).ToInteger();
                }
                else if (Parts[PartNr].Length > 1 && Parts[PartNr].ToUpper().Substring(0, 1) == "W" && Parts[PartNr].Substring(1).IsInteger())
                {
                    //WaitDuration setting
                    WaitDurationMs = Parts[PartNr].Substring(1).ToInteger().Limit(0, int.MaxValue);
                }
                else if (Parts[PartNr].Length > 1 && Parts[PartNr].ToUpper().Substring(0, 1) == "M" && Parts[PartNr].Substring(1).IsInteger())
                {
                    //MinimumDuration setting
                    MinDurationMs = Parts[PartNr].Substring(1).ToInteger().Limit(0, int.MaxValue);
                }
                else if (Parts[PartNr].Length > 1 && Parts[PartNr].ToUpper().Substring(0, 1) == "F" && Parts[PartNr].Substring(1).IsInteger())
                {

                    //Dimming duration for up and down
                    FadingUpDurationMs = Parts[PartNr].Substring(1).ToInteger().Limit(0, int.MaxValue);
                    FadingDownDurationMs = FadingUpDurationMs;
                }
                else if (Parts[PartNr].Length > 2 && Parts[PartNr].ToUpper().Substring(0, 2) == "FU" && Parts[PartNr].Substring(2).IsInteger())
                {

                    //Dimming up duration
                    FadingUpDurationMs = Parts[PartNr].Substring(2).ToInteger().Limit(0, int.MaxValue);

                }
                else if (Parts[PartNr].Length > 2 && Parts[PartNr].ToUpper().Substring(0, 2) == "FD" && Parts[PartNr].Substring(2).IsInteger())
                {

                    //Dimming down duration
                    FadingDownDurationMs = Parts[PartNr].Substring(2).ToInteger().Limit(0, int.MaxValue);
                }
                else if (Parts[PartNr].IsInteger())
                {
                    switch (IntegerCnt)
                    {
                        case 0:
                            if (Blink == -1)
                            {
                                //Its a blink interval
                                BlinkIntervalMs = (Parts[PartNr].ToInteger()).Limit(1, int.MaxValue);
                                DurationMs = 0;
                            }
                            else
                            {
                                //Its a duration

                                DurationMs = Parts[PartNr].ToInteger().Limit(1, int.MaxValue);
                            }
                            break;
                        case 1:
                            if (Blink != -1)
                            {
                                Blink = Parts[PartNr].ToInteger().Limit(1, int.MaxValue);
                                if (DurationMs > 0 & Blink >= 1)
                                {
                                    BlinkIntervalMs = (DurationMs / Blink).Limit(1, int.MaxValue);
                                    DurationMs = 0;

                                }
                            }
                            break;
                        default:
                            Log.Warning("The ledcontrol table config setting {0} contains more than 2 numeric values without a type definition.".Build(SettingData));
                            throw new Exception("The ledcontrol table config setting {0} contains more than 2 numeric values without a type definition.".Build(SettingData));

                    }
                    IntegerCnt++;
                }
                else if (PartNr == 0)
                {
                    //This should be a color
                    ColorName = Parts[PartNr];
                }
                else
                {
                    Log.Warning("Cant parse the part {0} of the ledcontrol table config setting {1}".Build(Parts[PartNr], SettingData));

                    throw new Exception("Cant parse the part {0} of the ledcontrol table config setting {1}".Build(Parts[PartNr], SettingData));
                }
                PartNr++;
            }




        }

        public void FromEffect(IEffect Effect)
        {
            IEffect currentFX = Effect;
            NoBool = true; //Until there is a ValueMapFullRangeEffect
            while (currentFX != null) {
                if (currentFX is TableElementConditionEffect conditionFX) {
                    OutputControl = OutputControlEnum.Condition;
                    Condition = conditionFX.Condition;
                } else if (currentFX is ValueMapFullRangeEffect) {
                    NoBool = false;
                } else if (currentFX is ValueInvertEffect) {
                    Invert = true;
                } else if (currentFX is DelayEffect delayFX) {
                    WaitDurationMs = delayFX.DelayMs;
                } else if (currentFX is ExtendDurationEffect extDurationFX) {
                    ExtDurationMs = extDurationFX.DurationMs;
                } else if (currentFX is MinDurationEffect minDurationFX) {
                    if (minDurationFX.Name.Split(' ').Last() != "DefaultMinDurationEffect") {
                        MinDurationMs = minDurationFX.MinDurationMs;
                    }
                } else if (currentFX is MaxDurationEffect maxDurationFX) {
                    MaxDurationMs = maxDurationFX.MaxDurationMs;
                } else if (currentFX is DurationEffect durationFX) {
                    //TargetEffect is a blink effect, Blink will be > 0 based on Duration & BlinkDuration
                    if (durationFX.TargetEffect is BlinkEffect blinkFX) {
                        var duration = durationFX.DurationMs;
                        BlinkIntervalMs = blinkFX.DurationActiveMs + blinkFX.DurationInactiveMs;
                        Blink = durationFX.DurationMs / BlinkIntervalMs;
                        DurationMs = 0;
                    } else {
                        DurationMs = durationFX.DurationMs;
                        Blink = 0;
                    }
                } else if (currentFX is BlinkEffect blinkFX) {
                    //Is there a nested Blink ?
                    if (blinkFX.TargetEffect is BlinkEffect nestedBlinkFX) {
                        BlinkLow = nestedBlinkFX.LowValue;
                        BlinkIntervalMsNested = nestedBlinkFX.DurationActiveMs + nestedBlinkFX.DurationInactiveMs;
                        BlinkPulseWidthNested = (int)(((double)nestedBlinkFX.DurationActiveMs * 100 / BlinkIntervalMsNested)+0.5f);
                        currentFX = blinkFX.TargetEffect;
                    } else if (Blink == 0) {
                        //Not already set by previous DurationEffect
                        Blink = -1;
                        DurationMs = 1000;
                        BlinkLow = blinkFX.LowValue;
                        BlinkIntervalMs = blinkFX.DurationActiveMs + blinkFX.DurationInactiveMs;
                        BlinkPulseWidth = (int)(((double)blinkFX.DurationActiveMs * 100 / BlinkIntervalMs)+0.5f);
                    }
                } else if (currentFX is FadeEffect fadeFX) {
                    FadingDownDurationMs = fadeFX.FadeDownDuration;
                    FadingUpDurationMs = fadeFX.FadeUpDuration;
                } else if (currentFX is AnalogToyValueEffect analogToyValueFX) {
                    Intensity = analogToyValueFX.ActiveValue.Alpha;
                } else if (currentFX is RGBAColorEffect rgbaColorFX) {
                    //TODO find the ColorConfig
                    ColorName = rgbaColorFX.ActiveColor.ToString();
                } else if (currentFX is IMatrixEffect matrixFX) {
                    IsArea = true;

                    if (matrixFX.FixedLayerNr) {
                        Layer = matrixFX.LayerNr;
                    }

                    AreaTop = (int)matrixFX.Top;
                    AreaLeft = (int)matrixFX.Left;
                    AreaWidth = (int)matrixFX.Width;
                    AreaHeight = (int)matrixFX.Height;

                    if (currentFX is IMatrixShiftEffect matrixShiftFX) {
                        AreaDirection = matrixShiftFX.ShiftDirection;
                        AreaAcceleration = (int)matrixShiftFX.ShiftAcceleration;
                        AreaSpeed = (int)matrixShiftFX.ShiftSpeed;
                    }

                    if (currentFX is IMatrixFlickerEffect matrixFlickerFX) {
                        AreaFlickerDensity = matrixFlickerFX.Density;
                        AreaFlickerMinDurationMs = matrixFlickerFX.MinFlickerDurationMs;
                        AreaFlickerMaxDurationMs = matrixFlickerFX.MaxFlickerDurationMs;
                        //Average the fades duration in case they were changed elsewhere
                        AreaFlickerFadeDurationMs = (matrixFlickerFX.FlickerFadeDownDurationMs + matrixFlickerFX.FlickerFadeUpDurationMs) / 2;
                    }

                    if (currentFX is IMatrixBitmapEffect matrixBitmapFX) {
                        IsBitmap = true;

                        AreaBitmapTop = matrixBitmapFX.BitmapTop;
                        AreaBitmapLeft = matrixBitmapFX.BitmapLeft;
                        AreaBitmapWidth = matrixBitmapFX.BitmapWidth;
                        AreaBitmapHeight = matrixBitmapFX.BitmapHeight;
                        AreaBitmapFrame = matrixBitmapFX.BitmapFrameNumber;
                    }

                    if (currentFX is IMatrixBitmapAnimationEffect matrixBitmapAnimationFX) {
                        AreaBitmapAnimationDirection = matrixBitmapAnimationFX.AnimationStepDirection;
                        AreaBitmapAnimationBehaviour = matrixBitmapAnimationFX.AnimationBehaviour;
                        AreaBitmapAnimationFrameDuration = matrixBitmapAnimationFX.AnimationFrameDurationMs;
                        AreaBitmapAnimationStepCount = matrixBitmapAnimationFX.AnimationFrameCount;
                        AreaBitmapAnimationStepSize = matrixBitmapAnimationFX.AnimationStepSize;
                    }

                    if (currentFX is IMatrixAnalogValue matrixAnalogFX) {
                        Intensity = matrixAnalogFX.ActiveValue.Alpha;
                    }

                    if (currentFX is IMatrixRGBAColor matrixRGBAFX) {
                        ColorName = matrixRGBAFX.ActiveColor.ToString();
                        if (currentFX is IMatrixRGBAColor2 matrixRGBA2FX) {
                            ColorName2 = matrixRGBA2FX.ActiveColor2.ToString();
                        }
                    }

                    if (currentFX is RGBAMatrixPlasmaEffect plasmaMatrixFX) {
                        IsPlasma = true;
                        PlasmaDensity = plasmaMatrixFX.PlasmaDensity;
                        PlasmaSpeed = plasmaMatrixFX.PlasmaSpeed;
                    } else if (currentFX is RGBAMatrixShapeEffect || currentFX is RGBAMatrixColorScaleShapeEffect) {
                        if (currentFX is RGBAMatrixColorScaleShapeEffect rgbaShapeFX) {
                            ShapeName = rgbaShapeFX.ShapeName;
                            ColorName = rgbaShapeFX.ActiveColor.ToString();
                        } else {
                            ShapeName = (currentFX as RGBAMatrixShapeEffect).ShapeName;
                        }
                    }
                } 

                if (currentFX is EffectEffectBase effectEffect) {
                    currentFX = effectEffect.TargetEffect;
                } else {
                    break;
                }
            }
        }

        private string GetConfigToolCommand<T>(T value, T defaultValue, string command)
        {
            if (value == null || (value?.Equals(defaultValue) ?? false)) {
                return string.Empty;
            }
            return $"{command}{value.ToString()} ";
        }

        private string GetConfigToolCommand(string value, string command)
        {
            if (value.IsNullOrEmpty()) {
                return string.Empty;
            }

            return $"{command}{value} ";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ToConfigToolCommand(ColorList colorList = null)
        {
            string configToolStr = string.Empty;

            switch (OutputControl) {
                case OutputControlEnum.FixedOn:
                    configToolStr += "ON ";
                    break;
                case OutputControlEnum.FixedOff:
                    configToolStr += "OFF ";
                    break;
                case OutputControlEnum.Controlled: {
                    configToolStr += $"{TableElement} ";
                    break;
                }
                case OutputControlEnum.Condition:
                    configToolStr += $"{Condition} ";
                    break;
            }

            //Main Color
            configToolStr += GetConfigToolCommand(ColorName, string.Empty);

            if (Invert) {
                configToolStr += "INVERT ";
            }

            if (NoBool) {
                configToolStr += "NOBOOL ";
            }

            //Blink and/or duration export
            if (Blink == -1) {
                if (BlinkIntervalMs != 1000) {
                    configToolStr += $"BLINK {BlinkIntervalMs} ";
                } else {
                    configToolStr += $"BLINK ";
                }
            } else if (Blink > 0) {
                configToolStr += $"{Blink * BlinkIntervalMs} {Blink} ";
            } else if (DurationMs >= 0) {
                configToolStr += $"{DurationMs} ";
            }

            //Blink & nested blink
            configToolStr += GetConfigToolCommand(BlinkLow, 0, "BL#");
            configToolStr += GetConfigToolCommand(BlinkPulseWidth, 50, "BPW");
            configToolStr += GetConfigToolCommand(BlinkIntervalMsNested, 0, "BNP");
            configToolStr += GetConfigToolCommand(BlinkPulseWidthNested, 50, "BNPW");

            //Durations
            configToolStr += GetConfigToolCommand(MinDurationMs, 0, "M");
            configToolStr += GetConfigToolCommand(MaxDurationMs, 0, "MAX");
            configToolStr += GetConfigToolCommand(WaitDurationMs, 0, "W");
            if (FadingUpDurationMs == FadingDownDurationMs) {
                configToolStr += GetConfigToolCommand(FadingDownDurationMs, 0, "F");
            } else {
                configToolStr += GetConfigToolCommand(FadingUpDurationMs, 0, "FU");
                configToolStr += GetConfigToolCommand(FadingDownDurationMs, 0, "FD");
            }

            //Intensity
            configToolStr += GetConfigToolCommand(Intensity, -1, "I#");

            //Layer
            configToolStr += GetConfigToolCommand(Layer, 0, "L");

            if (IsArea) {
                configToolStr += GetConfigToolCommand(AreaTop, 0, "AT");
                configToolStr += GetConfigToolCommand(AreaLeft, 0, "AL");
                configToolStr += GetConfigToolCommand(AreaWidth, 100, "AW");
                configToolStr += GetConfigToolCommand(AreaHeight, 100, "AH");

                configToolStr += GetConfigToolCommand(ShapeName, "SHP");

                if (IsPlasma) {
                    configToolStr += GetConfigToolCommand(PlasmaSpeed, 0,"APS");
                    configToolStr += GetConfigToolCommand(PlasmaDensity, 0, "APD");
                    configToolStr += GetConfigToolCommand(ColorName2, "APC");
                }

                if (IsBitmap) {
                    configToolStr += GetConfigToolCommand(AreaBitmapTop, 0, "ABT");
                    configToolStr += GetConfigToolCommand(AreaBitmapLeft, 0, "ABL");
                    configToolStr += GetConfigToolCommand(AreaBitmapWidth, -1, "ABW");
                    configToolStr += GetConfigToolCommand(AreaBitmapHeight, -1, "ABH");
                    configToolStr += GetConfigToolCommand(AreaBitmapFrame, 0, "ABF");

                    //BitmapAnimation
                    configToolStr += GetConfigToolCommand(AreaBitmapAnimationStepSize, 1, "AAS");
                    configToolStr += GetConfigToolCommand(AreaBitmapAnimationStepCount, 0, "AAC");
                    configToolStr += GetConfigToolCommand(AreaBitmapAnimationFrameDuration, 30, "AAF");
                    configToolStr += GetConfigToolCommand((char)AreaBitmapAnimationDirection, (char)MatrixAnimationStepDirectionEnum.Frame, "AAD");
                    configToolStr += GetConfigToolCommand((char)AreaBitmapAnimationBehaviour, (char)AnimationBehaviourEnum.Loop, "AAB");
                }

                //Flicker
                configToolStr += GetConfigToolCommand(AreaFlickerDensity, 0, "AFDEN");
                configToolStr += GetConfigToolCommand(AreaFlickerMinDurationMs, 0, "AFMIN");
                configToolStr += GetConfigToolCommand(AreaFlickerMaxDurationMs, 0, "AFMAX");
                configToolStr += GetConfigToolCommand(AreaFlickerFadeDurationMs, 0, "AFFADE");

                //Shift
                configToolStr += GetConfigToolCommand(AreaAcceleration, 0, "ASA");
                configToolStr += GetConfigToolCommand(AreaSpeed, 100, "ASS");
                configToolStr += GetConfigToolCommand((char)AreaDirection, (char)MatrixShiftDirectionEnum.Invalid, "ASD"); 
            }

            if (colorList != null) {
                foreach(var col in colorList) {
                    if (configToolStr.Contains(col.ToString())){
                        configToolStr = configToolStr.Replace(col.ToString(), col.Name);
                    }
                }
            }

            return configToolStr.TrimEnd(' ');
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableConfigSetting"/> class.
        /// Parses the setting data. <br/>
        /// </summary>
        /// <param name="SettingData">The setting data.</param>
        /// <exception cref="System.Exception">
        /// No data to parse.<br/>
        /// or <br/>
        /// Cant parse the part {0} of the ledcontrol table config setting {1}.
        /// </exception>
        public TableConfigSetting(string SettingData)
            : this()
        {
            ParseSettingData(SettingData);
        }



        /// <summary>
        /// Initializes a new instance of the <see cref="TableConfigSetting"/> class.
        /// </summary>
        public TableConfigSetting()
        {
            this.Intensity = 255;
            this.Blink = 0;
            this.DurationMs = -1;

        }







    }
}
