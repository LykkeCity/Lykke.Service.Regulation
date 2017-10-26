using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Regulation.Core.Domain;

namespace Lykke.Service.Regulation.Core.Repositories
{
    public interface IClientAvailableRegulationRepository
    {
        Task<IEnumerable<IClientAvailableRegulation>> GetAllAsync();

        Task<IEnumerable<IClientAvailableRegulation>> GetByClientIdAsync(string clientId);

        Task<IEnumerable<IClientAvailableRegulation>> GetByRegulationIdAsync(string regulationId);

        Task AddAsync(IClientAvailableRegulation clientRegulation);

        Task RemoveAsync(string clientId, string regulationId);
    }
}
