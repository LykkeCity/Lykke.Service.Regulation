using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Regulation.Core.Domain;

namespace Lykke.Service.Regulation.Core.Services
{
    public interface IClientRegulationService
    {
        Task<IClientRegulation> GetAsync(string clientId, string regulationId);

        Task<IEnumerable<IClientRegulation>> GetByClientIdAsync(string clientId);

        Task<IEnumerable<IClientRegulation>> GetByRegulationIdAsync(string regulationId);

        Task<IEnumerable<IClientRegulation>> GetActiveByClientIdAsync(string clientId);

        Task<IEnumerable<IClientRegulation>> GetAvailableByClientIdAsync(string clientId);

        Task AddAsync(IClientRegulation clientRegulation);

        Task SetKycAsync(string clientId, string regulationId);

        Task SetActiveAsync(string clientId, string regulationId, bool state);

        Task DeleteAsync(string clientId, string regulationId);
    }
}
