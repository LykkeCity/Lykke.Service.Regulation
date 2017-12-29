using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Regulation.Core.Domain;

namespace Lykke.Service.Regulation.Core.Services
{
    public interface IMarginRegulationRuleService
    {
        Task<IMarginRegulationRule> GetAsync(string ruleId);

        Task<IEnumerable<IMarginRegulationRule>> GetAllAsync();

        Task<IEnumerable<IMarginRegulationRule>> GetByCountryAsync(string country);

        Task<IEnumerable<IMarginRegulationRule>> GetByRegulationIdAsync(string regulationId);

        Task AddAsync(IMarginRegulationRule rule);

        Task UpdateAsync(IMarginRegulationRule rule);

        Task DeleteAsync(string ruleId);
    }
}
