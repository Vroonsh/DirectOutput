using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DofConfigToolWrapper
{
    public class XmlSerializable<T> where T : new()
    {
        public static T ReadFromXml(string FilePath)
        {
            string Xml = DirectOutput.General.FileReader.ReadFileToString(FilePath);
            try {
                using (MemoryStream ms = new MemoryStream(Encoding.Default.GetBytes(Xml))) {
                    T newObj = (T)new XmlSerializer(typeof(T)).Deserialize(ms);
                    return newObj;
                }
            } catch (Exception e) {
                throw new Exception($"Cannot read {typeof(T).ToString()} from {FilePath}", e);
            }
        }

        public void WriteToXml(string FilePath)
        {
            try {
                string Xml = "";
                using (MemoryStream ms = new MemoryStream()) {
                    new XmlSerializer(typeof(T)).Serialize(ms, this);
                    ms.Position = 0;
                    using (StreamReader sr = new StreamReader(ms, Encoding.Default)) {
                        Xml = sr.ReadToEnd();
                    }

                }
                Xml.WriteToFile(FilePath);
            } catch (Exception e) {
                throw new Exception($"Cannot write {this.GetType().ToString()} to {FilePath}", e);
            }
        }
    }
}
