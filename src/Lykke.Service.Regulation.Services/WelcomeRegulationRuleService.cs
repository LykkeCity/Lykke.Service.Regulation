using System.Collections.Generic;
using System.Linq;
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

            IEnumerable<IWelcomeRegulationRule> regulationRules =
                await _welcomeRegulationRuleRepository.GetByCountryAsync(welcomeRegulationRule.Country);

            if (regulationRules.Any(o => o.RegulationId == welcomeRegulationRule.RegulationId))
            {
                throw new ServiceException("Rule already exist.");
            }
            
            await _welcomeRegulationRuleRepository.AddAsync(welcomeRegulationRule);
        }

        public async Task UpdateActiveAsync(string regulationRuleId, bool active)
        {
            IWelcomeRegulationRule regulationRule = await _welcomeRegulationRuleRepository.GetAsync(regulationRuleId);

            if (regulationRule == null)
            {
                throw new ServiceException("Regulation rule not found.");
            }

            await _welcomeRegulationRuleRepository.UpdateAsync(new WelcomeRegulationRule
            {
                Id = regulationRule.Id,
                RegulationId = regulationRule.RegulationId,
                Country = regulationRule.Country,
                Active = active
            });
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
