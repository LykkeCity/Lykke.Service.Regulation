using System.Threading.Tasks;
using Lykke.Service.Regulation.Core.Contracts;

namespace Lykke.Service.Regulation.Core.Services
{
    public interface IClientRegulationPublisher
    {
        Task PublishAsync(ClientRegulationsMessage message);
    }
}
