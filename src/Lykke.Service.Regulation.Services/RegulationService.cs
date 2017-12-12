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

        public RegulationService(
            IRegulationRepository regulationRepository,
            IClientRegulationRepository clientRegulationRepository,
            IWelcomeRegulationRuleRepository welcomeRegulationRuleRepository)
        {
            _regulationRepository = regulationRepository;
            _clientRegulationRepository = clientRegulationRepository;
            _welcomeRegulationRuleRepository = welcomeRegulationRuleRepository;
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

        public Task<IEnumerable<IRegulation>> GetAllAsync()
        {
            return _regulationRepository.GetAllAsync();
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
