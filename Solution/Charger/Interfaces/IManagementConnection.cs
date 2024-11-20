namespace Charger.Interfaces
{
    public interface IManagementConnection
    {
        void Init();
        void PublishConfigurationMessage();
    }
}