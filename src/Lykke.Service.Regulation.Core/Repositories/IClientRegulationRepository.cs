using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Regulation.Core.Domain;

namespace Lykke.Service.Regulation.Core.Repositories
{
    public interface IClientRegulationRepository
    {
        Task<IEnumerable<IRegulation>> GetAvailableRegulation(string clientId);
        Task AddAvailableRegulation(string clientId, string regulationId);
        Task<IRegulation> SetRegulation(string clientId, string regulationId);
        Task<IRegulation> GetRegulation(string clientId);
    }
}