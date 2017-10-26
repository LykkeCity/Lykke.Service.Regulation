using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Regulation.Core.Domain;

namespace Lykke.Service.Regulation.Core.Repositories
{
    public interface IClientRegulationRepository
    {
        Task<IClientRegulation> GetAsync(string clientId);

        Task AddAsync(IClientRegulation clientAvailableRegulation);

        Task RemoveAsync(string clientId);
    }
}
