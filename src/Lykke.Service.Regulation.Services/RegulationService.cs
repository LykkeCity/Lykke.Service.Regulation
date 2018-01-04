using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.Regulation.Core.Domain;
using Lykke.Service.Regulation.Core.Repositories;
using Lykke.Service.Regulation.Core.Services;
using Lykke.Service.Regulation.Core.Exceptions;

namespace Lykke.Service.Regulation.Services
{
    public class RegulationService : IRegulationService
    {
        private readonly IRegulationRepository _regulationRepository;
        private readonly IClientRegulationRepository _clientRegulationRepository;
        private readonly IWelcomeRegulationRuleRepository _welcomeRegulationRuleRepository;
        private readonly IMarginRegulationRuleRepository _marginRegulationRuleRepository;

        public RegulationService(
            IRegulationRepository regulationRepository,
            IClientRegulationRepository clientRegulationRepository,
            IWelcomeRegulationRuleRepository welcomeRegulationRuleRepository,
            IMarginRegulationRuleRepository marginRegulationRuleRepository)
        {
            _regulationRepository = regulationRepository;
            _clientRegulationRepository = clientRegulationRepository;
            _welcomeRegulationRuleRepository = welcomeRegulationRuleRepository;
            _marginRegulationRuleRepository = marginRegulationRuleRepository;
        }
        
        public async Task<IRegulation> GetAsync(string regulationId)
        {
            IRegulation regulation = await _regulationRepository.GetAsync(regulationId);

            if (regulation == null)
            {
                throw new ServiceException("Regulation not found.");
            }

            return regulation;
        }

        public async Task<IRegulation> GetByCountryAsync(string country)
        {
            if (string.IsNullOrEmpty(country))
                return null;

            IEnumerable<IWelcomeRegulationRule> welcomeRegulationRules =
                await _welcomeRegulationRuleRepository.GetByCountryAsync(country);

            IWelcomeRegulationRule welcomeRegulationRule = welcomeRegulationRules
                .OrderByDescending(o => o.Priority)
                .FirstOrDefault();

            if (welcomeRegulationRule == null)
            {
                welcomeRegulationRules =
                    await _welcomeRegulationRuleRepository.GetDefaultAsync();

                welcomeRegulationRule = welcomeRegulationRules
                    .OrderByDescending(o => o.Priority)
                    .FirstOrDefault();
            }

            if (welcomeRegulationRule == null)
                return null;

            return await _regulationRepository.GetAsync(welcomeRegulationRule.RegulationId);
        }

        public Task<IEnumerable<IRegulation>> GetAllAsync()
        {
            return _regulationRepository.GetAllAsync();
        }

        public async Task<IRegulation> GetMarginByCountryAsync(string country)
        {
            if (string.IsNullOrEmpty(country))
                return null;

            IEnumerable<IMarginRegulationRule> marginRegulationRules =
                await _marginRegulationRuleRepository.GetByCountryAsync(country);

            IMarginRegulationRule marginRegulationRule = marginRegulationRules
                .OrderByDescending(o => o.Priority)
                .FirstOrDefault();

            if (marginRegulationRule == null)
                return null;

            return await _regulationRepository.GetAsync(marginRegulationRule.RegulationId);
        }

        public async Task AddAsync(IRegulation regulation)
        {
            if (await _regulationRepository.GetAsync(regulation.Id) != null)
            {
                throw new ServiceException("Regulation already exists.");
            }

            await _regulationRepository.AddAsync(regulation);
        }

        public async Task UpdateAsync(IRegulation regulation)
        {
            if (await _regulationRepository.GetAsync(regulation.Id) == null)
            {
                throw new ServiceException("Regulation not found.");
            }

            await _regulationRepository.UpdateAsync(regulation);
        }

        public async Task DeleteAsync(string regulationId)
        {
            IRegulation regulation = await _regulationRepository.GetAsync(regulationId);

            if (regulation == null)
            {
                throw new ServiceException("Regulation not found.");
            }

            IEnumerable<IClientRegulation> clientAvailableRegulations =
                await _clientRegulationRepository.GetByRegulationIdAsync(regulationId);

            if (clientAvailableRegulations.Any())
            {
                throw new ServiceException("Can not delete regulation associated with one or more clients.");
            }

            IEnumerable<IWelcomeRegulationRule> welcomeRegulationRules =
                await _welcomeRegulationRuleRepository.GetByRegulationIdAsync(regulationId);

            if (welcomeRegulationRules.Any())
            {
                throw new ServiceException("Can not delete regulation associated with one or more welcome rules.");
            }

            await _regulationRepository.DeleteAsync(regulationId);
        }
    }
}
