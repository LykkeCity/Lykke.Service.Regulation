using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Regulation.Core.Domain;

namespace Lykke.Service.Regulation.Core.Repositories
{
    public interface IClientRegulationRepository
    {
        Task<IClientRegulation> GetAsync(string clientId, string regulationId);

        Task<IEnumerable<IClientRegulation>> GetByClientIdAsync(string clientId);

        Task<IEnumerable<IClientRegulation>> GetActiveByClientIdAsync(string clientId);

        Task<IEnumerable<IClientRegulation>> GetByRegulationIdAsync(string regulationId);

        Task<IEnumerable<IClientRegulation>> GetAvailableByClientIdAsync(string clientId);

        Task AddAsync(IClientRegulation clientRegulation);

        Task UpdateAsync(IClientRegulation clientRegulation);

        Task DeleteAsync(string clientId, string regulationId);
    }
}
