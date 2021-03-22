using DirectOutput.General.Color;
using DirectOutput.LedControl.Loader;

namespace DirectOutputControls
{
    public interface IColorListProvider
    {
        ColorConfig GetColorConfig(string colorName);
        ColorConfig ResolveColorConfig(RGBAColor color);
        ColorList GetColorList();
    }
}
