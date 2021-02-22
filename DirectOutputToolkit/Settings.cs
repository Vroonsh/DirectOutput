using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
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

        [XmlIgnore]
        public Color PreviewBackgroundColor { get; set; }  = Color.MidnightBlue;
        public int PreviewBackgroundColorARGB
        {   get {
                return PreviewBackgroundColor.ToArgb();
            }
            set {
                PreviewBackgroundColor = Color.FromArgb(value);
            }
        }

        public Rectangle LastMainWindowRect { get; set; } = new Rectangle();
        public Rectangle LastPreviewWindowRect { get; set; } = new Rectangle();

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
