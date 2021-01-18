using DirectOutput;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DirectOutputControls
{
    public static class DirectOutputViewSetupSerializer
    {
        public static DirectOutputViewSetup ReadFromXml(string FileName)
        {
            if (FileName.IsNullOrEmpty()) {
                return new DirectOutputViewSetup();
            }

            string Xml;
            try {
                Xml = DirectOutput.General.FileReader.ReadFileToString(FileName);
            } catch (Exception E) {
                Log.Exception("Could not load DirectOutput View Setup from {0}.".Build(FileName), E);
                throw new Exception("Could not read DirectOutput View Setup file {0}.".Build(FileName), E);
            }

            byte[] xmlBytes = Encoding.Default.GetBytes(Xml);
            using (MemoryStream ms = new MemoryStream(xmlBytes)) {
                try {
                    return (DirectOutputViewSetup)new XmlSerializer(typeof(DirectOutputViewSetup)).Deserialize(ms);
                } catch (Exception E) {
                    Exception Ex = new Exception("Could not deserialize DirectOutput View Setup from XML data.", E);
                    Ex.Data.Add("XML Data", Xml);
                    Log.Exception("Could not load DirectOutput View Setup from XML data.", Ex);
                    throw Ex;
                }
            }
        }

        public static bool WriteToXml(DirectOutputViewSetup Setup, string FileName)
        {
            if (!FileName.IsNullOrEmpty()) {
                using (MemoryStream ms = new MemoryStream()) {
                    var serializer = new XmlSerializer(typeof(DirectOutputViewSetup));
                    Setup.Cleanup();
                    serializer.Serialize(ms, Setup);
                    ms.Position = 0;
                    string Xml = string.Empty;
                    using (StreamReader sr = new StreamReader(ms, Encoding.Default)) {
                        Xml = sr.ReadToEnd();
                        sr.Dispose();
                    }
                    File.WriteAllText(FileName, Xml);
                    return true;
                }
            }
            return false;
        }

    }
}
