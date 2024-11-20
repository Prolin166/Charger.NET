namespace Charger.Interfaces
{
    public interface IFileHelper
    {
        T ReadXmlFile<T>(string filepath);
        void WriteXmlFile<T>(string filepath, object obj);
    }
}
