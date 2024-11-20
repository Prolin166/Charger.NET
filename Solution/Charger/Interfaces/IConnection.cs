using System.Threading.Tasks;

namespace Charger.Interfaces
{
    public interface IConnection
    {
        void Init();
        Task PublishConfigurationMessage();
    }
}