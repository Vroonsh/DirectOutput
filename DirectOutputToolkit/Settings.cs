using System.Collections.Generic;
using System.IO;
using System.Reflection;
using DofConfigToolWrapper;

namespace DirectOutputToolkit
{
    public class Settings : XmlSerializable<Settings>
    {
        private string _LastDofConfigSetup = "";

        public string LastDofConfigSetup
        {
            get { return _LastDofConfigSetup; }
            set { _LastDofConfigSetup = value; }
        }

        private string _LastDirectOutputViewSetup = "";

        public string LastDofViewSetup
        {
            get { return _LastDirectOutputViewSetup; }
            set { _LastDirectOutputViewSetup = value; }
        }

        private List<string> _DofConfigSetups = new List<string>();

        public List<string> DofConfigSetups
        {
            get { return _DofConfigSetups; }
            set { _DofConfigSetups = value; }
        }


        private List<string> _DirectOutputViewSetups = new List<string>();

        public List<string> DofViewSetups
        {
            get { return _DirectOutputViewSetups; }
            set { _DirectOutputViewSetups = value; }
        }

        private string _LastRomName = "";

        public string LastRomName
        {
            get { return _LastRomName; }
            set { _LastRomName = value; }
        }

        private int _PulseDurationMs = 100;

        public int PulseDurationMs
        {
            get { return _PulseDurationMs; }
            set { _PulseDurationMs = value.Limit(10, 5000); }
        }

        public int EffectMinDurationMs { get; set; } = 60;
        public int EffectRGBMinDurationMs { get; set; } = 120;

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
