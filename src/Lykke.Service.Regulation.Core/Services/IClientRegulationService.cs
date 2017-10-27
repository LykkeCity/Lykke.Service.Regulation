using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Regulation.Core.Domain;

namespace Lykke.Service.Regulation.Core.Services
{
    public interface IClientRegulationService
    {
        Task<string> GetAsync(string clientId);
        Task<IEnumerable<string>> GetAvailableAsync(string clientId);
        Task AddAsync(IClientAvailableRegulation regulation);
        Task SetAsync(IClientRegulation regulation);
        Task RemoveAsync(string clientId);
        Task RemoveAvailableAsync(string clientId, string regulationId);
    }
}
