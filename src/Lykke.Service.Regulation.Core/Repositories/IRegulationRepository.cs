using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Regulation.Core.Domain;

namespace Lykke.Service.Regulation.Core.Repositories
{
    public interface IRegulationRepository
    {
        Task AddRegulation(IRegulation regulation);
        Task<IEnumerable<IRegulation>> GetAllAsync();
        Task<IRegulation> GetAsync(string id);
        Task<IEnumerable<IRegulation>> GetAsync(string[] ids);
        Task RemoveAsync(string id);
        Task UpdateAsync(IRegulation asset);
    }
}
