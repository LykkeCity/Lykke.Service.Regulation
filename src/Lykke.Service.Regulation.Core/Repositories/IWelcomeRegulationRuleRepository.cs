using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Regulation.Core.Domain;

namespace Lykke.Service.Regulation.Core.Repositories
{
    public interface IWelcomeRegulationRuleRepository
    {
        Task<IEnumerable<IWelcomeRegulationRule>> GetAllAsync();

        Task<IEnumerable<IWelcomeRegulationRule>> GetByCountryAsync(string country);

        Task<IEnumerable<IWelcomeRegulationRule>> GetByRegulationIdAsync(string regulationId);

        Task AddAsync(IWelcomeRegulationRule welcomeRegulationRule);

        Task DeleteAsync(string id);
    }
}
