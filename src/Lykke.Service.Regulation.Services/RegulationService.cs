using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.Regulation.Core.Domain;
using Lykke.Service.Regulation.Core.Repositories;
using Lykke.Service.Regulation.Core.Services;
using Lykke.Service.Regulation.Services.Exceptions;

namespace Lykke.Service.Regulation.Services
{
    public class RegulationService : IRegulationService
    {
        private readonly IRegulationRepository _regulationRepository;
        private readonly IClientRegulationRepository _clientRegulationRepository;
        private readonly IWelcomeRegulationRuleRepository _welcomeRegulationRuleRepository;

        public RegulationService(
            IRegulationRepository regulationRepository,
            IClientRegulationRepository clientRegulationRepository,
            IWelcomeRegulationRuleRepository welcomeRegulationRuleRepository)
        {
            _regulationRepository = regulationRepository;
            _clientRegulationRepository = clientRegulationRepository;
            _welcomeRegulationRuleRepository = welcomeRegulationRuleRepository;
        }
        
        public Task<IRegulation> GetAsync(string regulationId)
        {
            return _regulationRepository.GetAsync(regulationId);
        }

        public Task<IEnumerable<IRegulation>> GetAllAsync()
        {
            return _regulationRepository.GetAllAsync();
        }

        public Task AddAsync(IRegulation regulation)
        {
            return _regulationRepository.AddAsync(regulation);
        }

        public async Task DeleteAsync(string regulationId)
        {
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
