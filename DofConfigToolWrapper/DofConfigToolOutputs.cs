using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DofConfigToolWrapper
{
    /// <summary>
    /// Enum matching DofConfigTool outputs from the online generator
    /// </summary>
    public enum DofConfigToolOutputEnum
    {
        Invalid = -1,
        [Browsable(false)]
        AnalogOutputs_Start,
        [Description("Start Button")]
        StartButton,
        [Description("Launch Button")]
        LaunchButton,
        [Description("Authentic Launch Button")]
        AuthenticLaunchBall,
        [Description("ZB Launch Ball")]
        ZBLaunchBall,
        [Description("Fire Button")]
        FireButton,
        [Description("Extraball")]
        ExtraBall,
        [Description("10 Bumper Back Left")]
        Bumper10BackLeft,
        [Description("10 Bumper Back Center")]
        Bumper10BackCenter,
        [Description("10 Bumper Back Right")]
        Bumper10BackRight,
        [Description("10 Bumper Middle Left")]
        Bumper10MiddleLeft,
        [Description("10 Bumper Middle Center")]
        Bumper10MiddleCenter,
        [Description("10 Bumper Middle Right")]
        Bumper10MiddleRight,
        [Description("Slingshot Left")]
        SlingshotLeft,
        [Description("Slingshot Right")]
        SlingshotRight,
        [Description("Flipper Left")]
        FlipperLeft,
        [Description("Flipper Right")]
        FlipperRight,
        [Description("8 Bumper Left")]
        Bumper8Left,
        [Description("8 Bumper Center")]
        Bumper8Center,
        [Description("8 Bumper Back")]
        Bumper8Back,
        [Description("8 Bumper Right")]
        Bumper8Right,
        [Description("Knocker")]
        Knocker,
        [Description("Shaker")]
        Shaker,
        [Description("Gear")]
        Gear,
        [Description("Beacon")]
        Beacon,
        [Description("Fan")]
        Fan,
        [Description("Strobe")]
        Strobe,
        [Description("Coin")]
        Coin,
        [Description("How to Play")]
        HowToPlay,
        [Description("Genre")]
        Genre,
        [Description("Exit")]
        Exit,
        [Description("Topper Bell")]
        TopperBell,
        [Description("Shell Bell Small")]
        ShellBellSmall,
        [Description("Shell Bell Large")]
        ShellBellLarge,
        [Description("Repeating Bell")]
        RepeatingBell,
        [Description("Chime Unit High Tone")]
        ChimeUnitHighTone,
        [Description("Chime Unit Mid Tone")]
        ChimeUnitMidTone,
        [Description("Chime Unit Low Tone")]
        ChimeUnitLowTone,
        [Description("Chime Unit Extra-Low Tone")]
        ChimeUnitExtraLowTone,
        [Description("Chime 5")]
        Chime5,
        [Description("Hellball Motor")]
        HellballMotor,
        [Browsable(false)]
        AnalogCustom_Start,
        [Description("Custom Output 1")]
        CustomOutput1,
        [Description("Custom Output 2")]
        CustomOutput2,
        [Description("Custom Output 3")]
        CustomOutput3,
        [Description("Custom Output 4")]
        CustomOutput4,
        [Description("Custom Output 5")]
        CustomOutput5,
        [Description("Custom Output 6")]
        CustomOutput6,
        [Description("Custom Output 7")]
        CustomOutput7,
        [Description("Custom Output 8")]
        CustomOutput8,
        [Browsable(false)]
        AnalogCustom_End,
        [Browsable(false)]
        AnalogOutputs_End,

        [Browsable(false)]
        RGBOutputs_Start,
        [Description("5 Flasher Outside Left")]
        Flasher5_OutsideLeft,
        [Description("5 Flasher Left")]
        Flasher5_Left,
        [Description("5 Flasher Center")]
        Flasher5_Center,
        [Description("5 Flasher Right")]
        Flasher5_Right,
        [Description("5 Flasher Outside Right")]
        Flasher5_OutsideRight,
        [Description("3 Flasher Left")]
        Flasher3_Left,
        [Description("3 Flasher Center")]
        Flasher3_Center,
        [Description("3 Flasher Right")]
        Flasher3_Right,
        [Description("RGB Flippers")]
        RGBFlippers,
        [Description("RGB Left Magnasave")]
        RGBLeftMagnasave,
        [Description("RGB Right Magnasave")]
        RGBRightMagnasave,
        [Description("RGB Undercab Smart")]
        RGBUndercabSmart,
        [Description("RGB Undercab Complex")]
        RGBUndercabComplex,
        [Description("Hellball Color")]
        HellballColor,
        [Browsable(false)]
        RGBCustom_Start,
        [Description("Custom RGB 1")]
        CustomRGB1,
        [Description("Custom RGB 2")]
        CustomRGB2,
        [Description("Custom RGB 3")]
        CustomRGB3,
        [Description("Custom RGB 4")]
        CustomRGB4,
        [Browsable(false)]
        RGBCustom_End,
        [Browsable(false)]
        RGBOutputs_End,

        [Browsable(false)]
        RGBMXOutputs_Start,
        [Description("RGB Undercab Complex MX")]
        RGBUndercabComplexMX,
        [Description("PF Left Flashers MX")]
        PFLeftFlashersMX,
        [Description("PF Left Effects MX")]
        PFLeftEffectsMX,
        [Description("PF Back Flashers MX")]
        PFBackFlashersMX,
        [Description("PF Back Effects MX")]
        PFBackEffectsMX,
        [Description("PF Back Strobe MX")]
        PFBackStrobeMX,
        [Description("PF Back Beacon MX")]
        PFBackBeaconMX,
        [Description("PF Back PBX MX")]
        PFBackPBXMX,
        [Description("PF Right Flashers MX")]
        PFRightFlashersMX,
        [Description("PF Right Effects MX")]
        PFRightEffectsMX,
        [Description("Flipper Button MX")]
        FlipperButtonMX,
        [Description("Flipper Button PBX MX")]
        FlipperButtonPBXMX,
        [Description("Magnasave Left MX")]
        MagnasaveLeftMX,
        [Description("Magnasave Right MX")]
        MagnasaveRightMX,
        [Description("Launch Ball MX")]
        LaunchBallMX,
        [Description("Fire MX")]
        FireMX,
        [Description("Extraball MX")]
        ExtraballMX,
        [Browsable(false)]
        RGBMXOutputs_End,
    }

    public class DofConfigToolOutputs
    {
        public static DofConfigToolOutputEnum GetOutput(string OutputName) => Enum.GetValues(typeof(DofConfigToolOutputEnum))
                                                                                                    .Cast<DofConfigToolOutputEnum>()
                                                                                                    .FirstOrDefault(x => x.ToString().Equals(OutputName, StringComparison.InvariantCultureIgnoreCase));

        public static DofConfigToolOutputEnum[] GetPublicDofOutput(bool includeInvalid) => Enum.GetValues(typeof(DofConfigToolOutputEnum))
                                                                                                    .Cast<DofConfigToolOutputEnum>()
                                                                                                    .Where(x => {
                                                                                                        var field = typeof(DofConfigToolOutputEnum).GetField(Enum.GetName(typeof(DofConfigToolOutputEnum), x));
                                                                                                        var browseattr = field.GetCustomAttributes(typeof(BrowsableAttribute), false)
                                                                                                                            .FirstOrDefault() as BrowsableAttribute;
                                                                                                        if (browseattr != null && !browseattr.Browsable)
                                                                                                            return false;
                                                                                                        return includeInvalid || x != DofConfigToolOutputEnum.Invalid;
                                                                                                    }
                                                                                                    ).ToArray();

        public static string[] GetPublicDofOutputNames(bool includeInvalid) => GetPublicDofOutput(includeInvalid).Select(O => O.ToString()).ToArray();
    }
}
