using Charger.Interfaces;
using System.IO;
using System.Xml.Serialization;

namespace Charger.Common
{
    public class FileHelper : IFileHelper
    {
        public T ReadXmlFile<T>(string filepath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            FileStream stream = new FileStream(filepath, FileMode.Open);

            return (T)serializer.Deserialize(stream);
        }

        public void WriteXmlFile<T>(string filepath, object obj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            TextWriter stream = new StreamWriter(filepath);

            serializer.Serialize(stream, obj);

            stream.Close();
        }

    }
}
