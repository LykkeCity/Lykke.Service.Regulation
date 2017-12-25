using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Regulation.Core.Domain;

namespace Lykke.Service.Regulation.Core.Services
{
    public interface IRegulationService
    {
        Task<IRegulation> GetAsync(string regulationId);

        Task<IEnumerable<IRegulation>> GetAllAsync();

        Task<IRegulation> GetByCountryAsync(string country);

        Task AddAsync(IRegulation regulation);

        Task UpdateAsync(IRegulation regulation);

        Task DeleteAsync(string regulationId);
    }
}
