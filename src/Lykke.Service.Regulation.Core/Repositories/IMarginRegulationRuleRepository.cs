using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Regulation.Core.Domain;

namespace Lykke.Service.Regulation.Core.Repositories
{
    public interface IMarginRegulationRuleRepository
    {
        Task<IMarginRegulationRule> GetAsync(string ruleId);

        Task<IEnumerable<IMarginRegulationRule>> GetAllAsync();

        Task<IEnumerable<IMarginRegulationRule>> GetByCountryAsync(string country);

        Task<IEnumerable<IMarginRegulationRule>> GetByRegulationIdAsync(string regulationId);

        Task InsertAsync(IMarginRegulationRule rule);

        Task UpdateAsync(IMarginRegulationRule rule);

        Task DeleteAsync(string ruleId);
    }
}
