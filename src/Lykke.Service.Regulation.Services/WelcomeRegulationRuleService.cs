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

        public Task<IEnumerable<IWelcomeRegulationRule>> GetAllAsync()
        {
            return _welcomeRegulationRuleRepository.GetAllAsync();
        }

        public Task<IEnumerable<IWelcomeRegulationRule>> GetByCountryAsync(string country)
        {
            return _welcomeRegulationRuleRepository.GetByCountryAsync(country);
        }

        public Task<IEnumerable<IWelcomeRegulationRule>> GetByRegulationIdAsync(string regulationId)
        {
            return _welcomeRegulationRuleRepository.GetByRegulationIdAsync(regulationId);
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

        public Task DeleteAsync(string id)
        {
            return _welcomeRegulationRuleRepository.DeleteAsync(id);
        }
    }
}
