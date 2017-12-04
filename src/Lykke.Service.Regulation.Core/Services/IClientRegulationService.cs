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

        Task SetDefaultAsync(string clientId, string country);

        Task<string> GetCountryCodeByPhoneAsync(string phoneNumber);

        Task UpdateKycAsync(string clientId, string regulationId, bool active);

        Task UpdateActiveAsync(string clientId, string regulationId, bool state);

        Task DeleteAsync(string clientId, string regulationId);
    }
}
