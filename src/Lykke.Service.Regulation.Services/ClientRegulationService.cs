using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.Regulation.Core.Domain;
using Lykke.Service.Regulation.Core.Repositories;
using Lykke.Service.Regulation.Core.Services;
using Lykke.Service.Regulation.Services.Exceptions;

namespace Lykke.Service.Regulation.Services
{
    public class ClientRegulationService : IClientRegulationService
    {
        private readonly IRegulationRepository _regulationRepository;
        private readonly IClientRegulationRepository _clientRegulationRepository;
        private readonly IWelcomeRegulationRuleRepository _welcomeRegulationRuleRepository;

        public ClientRegulationService(
            IRegulationRepository regulationRepository,
            IClientRegulationRepository clientRegulationRepository,
            IWelcomeRegulationRuleRepository welcomeRegulationRuleRepository)
        {
            _regulationRepository = regulationRepository;
            _clientRegulationRepository = clientRegulationRepository;
            _welcomeRegulationRuleRepository = welcomeRegulationRuleRepository;
        }

        public Task<IClientRegulation> GetAsync(string clientId, string regulationId)
        {
            return _clientRegulationRepository.GetAsync(clientId, regulationId);
        }

        public Task<IEnumerable<IClientRegulation>> GetByClientIdAsync(string clientId)
        {
            return _clientRegulationRepository.GetByClientIdAsync(clientId);
        }

        public Task<IEnumerable<IClientRegulation>> GetActiveByClientIdAsync(string clientId)
        {
            return _clientRegulationRepository.GetActiveByClientIdAsync(clientId);
        }

        public Task<IEnumerable<IClientRegulation>> GetAvailableByClientIdAsync(string clientId)
        {
            return _clientRegulationRepository.GetAvailableByClientIdAsync(clientId);
        }

        public Task<IEnumerable<IClientRegulation>> GetByRegulationIdAsync(string regulationId)
        {
            return _clientRegulationRepository.GetByRegulationIdAsync(regulationId);
        }

        public async Task AddAsync(IClientRegulation clientRegulation)
        {
            IRegulation result = await _regulationRepository.GetAsync(clientRegulation.RegulationId);

            if (result == null)
            {
                throw new ServiceException("Regulation not found.");
            }

            await _clientRegulationRepository.AddAsync(clientRegulation);
        }

        public async Task SetDefaultAsync(string clientId, string country)
        {
            IEnumerable<IClientRegulation> clientRegulations =
                await _clientRegulationRepository.GetByClientIdAsync(clientId);

            if (clientRegulations.Any())
            {
                throw new ServiceException("Client already have regulations.");
            }

            List<IWelcomeRegulationRule> welcomeRegulationRules =
                (await _welcomeRegulationRuleRepository.GetByCountryAsync(country)).ToList();

            if (!welcomeRegulationRules.Any())
            {
                throw new ServiceException("No default regulations for country.");
            }

            IEnumerable<ClientRegulation> defaultClientRegulations =
                welcomeRegulationRules.Select(o => new ClientRegulation
                {
                    ClientId = clientId,
                    RegulationId = o.RegulationId,
                    Active = false,
                    Kyc = false
                });

            await _clientRegulationRepository.AddAsync(defaultClientRegulations);
        }

        public async Task SetKycAsync(string clientId, string regulationId)
        {
            IClientRegulation clientRegulation = await _clientRegulationRepository.GetAsync(clientId, regulationId);

            if (clientRegulation == null)
            {
                throw new ServiceException("Client regulation not found.");
            }

            clientRegulation.Kyc = true;

            await _clientRegulationRepository.UpdateAsync(clientRegulation);
        }

        public async Task SetActiveAsync(string clientId, string regulationId, bool state)
        {
            IClientRegulation clientRegulation = await _clientRegulationRepository.GetAsync(clientId, regulationId);

            if (clientRegulation == null)
            {
                throw new ServiceException("Client regulation not found.");
            }

            clientRegulation.Active = state;

            await _clientRegulationRepository.UpdateAsync(clientRegulation);
        }

        public async Task DeleteAsync(string clientId, string regulationId)
        {
            IClientRegulation clientRegulation = await _clientRegulationRepository.GetAsync(clientId, regulationId);

            if (clientRegulation == null)
                return;

            if (clientRegulation.Kyc)
            {
                throw new ServiceException("Can not delete KYC client regulation.");
            }

            if (clientRegulation.Active)
            {
                throw new ServiceException("Can not delete active client regulation.");
            }

            await _clientRegulationRepository.DeleteAsync(clientId, regulationId);
        }
    }
}
