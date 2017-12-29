using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Regulation.Core.Domain;
using Lykke.Service.Regulation.Core.Exceptions;
using Lykke.Service.Regulation.Core.Repositories;
using Lykke.Service.Regulation.Core.Services;

namespace Lykke.Service.Regulation.Services
{
    public class MarginRegulationRuleService : IMarginRegulationRuleService
    {
        private readonly IRegulationRepository _regulationRepository;
        private readonly IMarginRegulationRuleRepository _marginRegulationRuleRepository;

        public MarginRegulationRuleService(
            IRegulationRepository regulationRepository,
            IMarginRegulationRuleRepository marginRegulationRuleRepository)
        {
            _regulationRepository = regulationRepository;
            _marginRegulationRuleRepository = marginRegulationRuleRepository;
        }

        public async Task<IMarginRegulationRule> GetAsync(string ruleId)
        {
            IMarginRegulationRule regulationRule = await _marginRegulationRuleRepository.GetAsync(ruleId);

            if (regulationRule == null)
            {
                throw new ServiceException("Margin regulation rule not found.");
            }

            return regulationRule;
        }

        public async Task<IEnumerable<IMarginRegulationRule>> GetAllAsync()
        {
            return await _marginRegulationRuleRepository.GetAllAsync();
        }

        public async Task<IEnumerable<IMarginRegulationRule>> GetByCountryAsync(string country)
        {
            return await _marginRegulationRuleRepository.GetByCountryAsync(country);
        }

        public async Task<IEnumerable<IMarginRegulationRule>> GetByRegulationIdAsync(string regulationId)
        {
            return await _marginRegulationRuleRepository.GetByRegulationIdAsync(regulationId);
        }

        public async Task AddAsync(IMarginRegulationRule rule)
        {
            IRegulation result = await _regulationRepository.GetAsync(rule.RegulationId);

            if (result == null)
            {
                throw new ServiceException("Regulation not found.");
            }

            await _marginRegulationRuleRepository.InsertAsync(rule);
        }

        public async Task UpdateAsync(IMarginRegulationRule rule)
        {
            IRegulation result = await _regulationRepository.GetAsync(rule.RegulationId);

            if (result == null)
            {
                throw new ServiceException("Regulation not found.");
            }

            IMarginRegulationRule regulationRule = await _marginRegulationRuleRepository.GetAsync(rule.Id);

            if (regulationRule == null)
            {
                throw new ServiceException("Margin regulation rule not found.");
            }

            await _marginRegulationRuleRepository.UpdateAsync(rule);
        }

        public async Task DeleteAsync(string ruleId)
        {
            await _marginRegulationRuleRepository.DeleteAsync(ruleId);
        }
    }
}
