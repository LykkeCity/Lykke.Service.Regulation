using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Regulation.Core.Domain;
using Lykke.Service.Regulation.Core.Repositories;
using Lykke.Service.Regulation.Core.Services;
using Lykke.Service.Regulation.Services.Exceptions;

namespace Lykke.Service.Regulation.Services
{
    public class WelcomeRegulationRuleService : IWelcomeRegulationRuleService
    {
        private readonly IRegulationRepository _regulationRepository;
        private readonly IWelcomeRegulationRuleRepository _welcomeRegulationRuleRepository;

        public WelcomeRegulationRuleService(
            IRegulationRepository regulationRepository, 
            IWelcomeRegulationRuleRepository welcomeRegulationRuleRepository)
        {
            _regulationRepository = regulationRepository;
            _welcomeRegulationRuleRepository = welcomeRegulationRuleRepository;
        }

        public async Task<IWelcomeRegulationRule> GetAsync(string regulationRuleId)
        {
            IWelcomeRegulationRule regulationRule = await _welcomeRegulationRuleRepository.GetAsync(regulationRuleId);

            if (regulationRule == null)
            {
                throw new ServiceException("Regulation rule not found.");
            }

            return regulationRule;
        }

        public Task<IEnumerable<IWelcomeRegulationRule>> GetAllAsync()
        {
            return _welcomeRegulationRuleRepository.GetAllAsync();
        }

        public Task<IEnumerable<IWelcomeRegulationRule>> GetByCountryAsync(string country)
        {
            return _welcomeRegulationRuleRepository.GetByCountryAsync(country);
        }

        public async Task<IEnumerable<IWelcomeRegulationRule>> GetByRegulationIdAsync(string regulationId)
        {
            IRegulation result = await _regulationRepository.GetAsync(regulationId);

            if (result == null)
            {
                throw new ServiceException("Regulation not found.");
            }

            return await _welcomeRegulationRuleRepository.GetByRegulationIdAsync(regulationId);
        }

        public async Task AddAsync(IWelcomeRegulationRule welcomeRegulationRule)
        {
            IRegulation result = await _regulationRepository.GetAsync(welcomeRegulationRule.RegulationId);

            if (result == null)
            {
                throw new ServiceException("Regulation not found.");
            }

            await _welcomeRegulationRuleRepository.AddAsync(welcomeRegulationRule);
        }

        public async Task UpdateAsync(IWelcomeRegulationRule welcomeRegulationRule)
        {
            IRegulation result = await _regulationRepository.GetAsync(welcomeRegulationRule.RegulationId);

            if (result == null)
            {
                throw new ServiceException("Regulation not found.");
            }

            IWelcomeRegulationRule regulationRule = await _welcomeRegulationRuleRepository.GetAsync(welcomeRegulationRule.Id);

            if (regulationRule == null)
            {
                throw new ServiceException("Regulation rule not found.");
            }

            await _welcomeRegulationRuleRepository.UpdateAsync(welcomeRegulationRule);
        }

        public async Task DeleteAsync(string regulationRuleId)
        {
            IWelcomeRegulationRule regulationRule = await _welcomeRegulationRuleRepository.GetAsync(regulationRuleId);

            if (regulationRule == null)
            {
                throw new ServiceException("Regulation rule not found.");
            }

            await _welcomeRegulationRuleRepository.DeleteAsync(regulationRuleId);
        }
    }
}
