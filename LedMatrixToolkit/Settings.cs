using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;

namespace LedMatrixToolkit
{
    public enum BackboardDensity
    {
        LPM_30,
        LPM_60,
        LPM_144
    }

    public class Settings
    {
        private string _LastRomName = "";

        public string LastRomName
        {
            get { return _LastRomName; }
            set { _LastRomName = value; }
        }

        private List<string> _RomNames = new List<string>();

        public List<string> RomNames
        {
            get { return _RomNames; }
            set { _RomNames = value; }
        }

        private int _BackboardNbLines = 8;

        public int BackboardNbLines
        {
            get { return _BackboardNbLines; }
            set { _BackboardNbLines = value.Limit(1, 10); }
        }

        private BackboardDensity _BackboardDensity = BackboardDensity.LPM_144;

        public BackboardDensity BackboardDensity
        {
            get { return _BackboardDensity; }
            set { _BackboardDensity = value; }
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
