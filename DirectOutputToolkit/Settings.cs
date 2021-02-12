using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using DofConfigToolWrapper;

namespace DirectOutputToolkit
{
    public class Settings : XmlSerializable<Settings>
    {
        public string LastDofConfigSetup { get; set; } = "";
        public string LastDirectOutputViewSetup { get; set; } = "";
        public List<string> DofConfigSetups { get; set; } = new List<string>();
        public List<string> DirectOutputViewSetups { get; set; } = new List<string>();
        public string LastRomName { get; set; } = "";

        public int EffectMinDurationMs { get; set; } = 60;
        public int EffectRGBMinDurationMs { get; set; } = 120;

        public bool DofFilesAutoUpdate { get; set; } = true;
        public bool AutoSaveOnQuit { get; set; } = true;

        public Rectangle LastMainWindowRect = new Rectangle();
        public Rectangle LastPreviewWindowRect = new Rectangle();

        public void SaveSettings()
        {
            new DirectoryInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "config")).CreateDirectoryPath();
            WriteToXml(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "config", "DirectOutputToolkitSettings.xml"));
        }

        public static Settings LoadSettings()
        {
            return ReadFromXml(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "config", "DirectOutputToolkitSettings.xml"));
        }

    }
}
