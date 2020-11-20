using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;
using System.Drawing;
using System.ComponentModel;

namespace LedControlToolkit
{
    public class Settings
    {
        public class LedPreviewArea
        {
            [Browsable(false)]
            public int Id => Name.GetHashCode();

            public string Name { get; set; }
            private float _X = 0.0f;
            private float _Y = 0.0f;
            private float _Width = 0.5f;
            private float _Height = 0.5f;

            public LedPreviewArea() { }

            public LedPreviewArea(LedPreviewArea ledPreviewArea)
            {
                this.Name = ledPreviewArea.Name;
                this.X = ledPreviewArea.X;
                this.Y = ledPreviewArea.Y;
                this.Width = ledPreviewArea.Width;
                this.Height = ledPreviewArea.Height;
                this.PreviewType = ledPreviewArea.PreviewType;
            }

            public float X { get { return _X; } set { _X = value.Limit(0.0f, 1.0f); } }
            public float Y { get { return _Y; } set { _Y = value.Limit(0.0f, 1.0f); } }
            public float Width { get { return _Width; } set { _Width = value.Limit(0.0f, 1.0f); } }
            public float Height { get { return _Height; } set { _Height = value.Limit(0.0f, 1.0f); } }
            public LedMatrixPreviewControl.PreviewType PreviewType { get; set; } = LedMatrixPreviewControl.PreviewType.Matrix;
        }

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

        private bool _ShowPreviewAreas = true;

        public bool ShowPreviewAreas
        {
            get { return _ShowPreviewAreas; }
            set { _ShowPreviewAreas = value; }
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
                Xml.WriteToFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),"config","LedControlToolkitSettings.xml"));
            }
            catch
            {

            }
        }


        public static Settings LoadSettings()
        {
            try
            {
                string SettingsXML = DirectOutput.General.FileReader.ReadFileToString(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "config", "LedControlToolkitSettings.xml"));
                
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
