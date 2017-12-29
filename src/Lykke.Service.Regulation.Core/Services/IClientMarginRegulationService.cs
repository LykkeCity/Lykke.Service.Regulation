using System.Threading.Tasks;
using Lykke.Service.Regulation.Core.Domain;

namespace Lykke.Service.Regulation.Core.Services
{
    public interface IClientMarginRegulationService
    {
        Task<IClientMarginRegulation> GetAsync(string clientId);

        Task AddAsync(string clientId, string regulationId);

        Task SetAsync(string clientId, string regulationId);

        Task DeleteAsync(string clientId);
    }
}
