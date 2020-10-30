using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;
using System.Drawing;

namespace LedMatrixToolkit
{
    public class Settings
    {
        private string _LastGlobalConfigFilename = "";

        public string LastGlobalConfigFilename
        {
            get { return _LastGlobalConfigFilename; }
            set { _LastGlobalConfigFilename = value; }
        }

        private string _LastLedControlIniFilename = "";

        public string LastLedControlIniFilename
        {
            get { return _LastLedControlIniFilename; }
            set { _LastLedControlIniFilename = value; }
        }

        private string _LastRomName = "";

        public string LastRomName
        {
            get { return _LastRomName; }
            set { _LastRomName = value; }
        }

        private List<string> _LedControlIniFileNames = new List<string>();

        public List<string> LedControlIniFileNames
        {
            get { return _LedControlIniFileNames; }
            set { _LedControlIniFileNames = value; }
        }


        private List<string> _GlobalConfigFilenames = new List<string>();

        public List<string> GlobalConfigFilenames
        {
            get { return _GlobalConfigFilenames; }
            set { _GlobalConfigFilenames = value; }
        }

        private int _PulseDurationMs = 100;

        public int PulseDurationMs
        {
            get { return _PulseDurationMs; }
            set { _PulseDurationMs = value.Limit(10, 5000); }
        }

        private bool _ShowMatrixGrid = true;

        public bool ShowMatrixGrid
        {
            get { return _ShowMatrixGrid; }
            set { _ShowMatrixGrid = value; }
        }

        public class LedPreviewArea
        {
            public string Name = string.Empty;
            public float X, Y, W, H;
            public LedMatrixPreviewControl.PreviewType PreviewType = LedMatrixPreviewControl.PreviewType.Matrix;
        }

        private List<LedPreviewArea> _LedPreviewAreas = new List<LedPreviewArea>();

        public List<LedPreviewArea> LedPreviewAreas
        {
            get { return _LedPreviewAreas; }
            set { _LedPreviewAreas = value; }
        }

        public void SaveSettings()
        {
            try
            {
                string Xml = "";
                using (MemoryStream ms = new MemoryStream())
                {
                    new XmlSerializer(typeof(Settings)).Serialize(ms, this);
                    ms.Position = 0;

                    using (StreamReader sr = new StreamReader(ms, Encoding.Default))
                    {
                        Xml = sr.ReadToEnd();

                    }

                }
                new DirectoryInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),"config")).CreateDirectoryPath();
                Xml.WriteToFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),"config","LedMatrixToolkitSettings.xml"));
            }
            catch
            {

            }
        }


        public static Settings LoadSettings()
        {
            try
            {
                string SettingsXML = DirectOutput.General.FileReader.ReadFileToString(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "config", "LedMatrixToolkitSettings.xml"));
                
                using (MemoryStream ms = new MemoryStream(Encoding.Default.GetBytes(SettingsXML)))
                {
                    Settings S = (Settings)new XmlSerializer(typeof(Settings)).Deserialize(ms);
                    
                    return S;
                }
            }
            catch 
            {
                return new Settings();
            }
        }

    }
}
