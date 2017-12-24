using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.Regulation.Core.Contracts;
using Lykke.Service.Regulation.Core.Domain;
using Lykke.Service.Regulation.Core.Exceptions;
using Lykke.Service.Regulation.Core.Repositories;
using Lykke.Service.Regulation.Core.Services;
using ClientRegulation = Lykke.Service.Regulation.Core.Domain.ClientRegulation;

namespace Lykke.Service.Regulation.Services
{
    public class ClientRegulationService : IClientRegulationService
    {
        private readonly IRegulationRepository _regulationRepository;
        private readonly IClientRegulationRepository _clientRegulationRepository;
        private readonly IWelcomeRegulationRuleRepository _welcomeRegulationRuleRepository;
        private readonly IClientRegulationPublisher _clientRegulationPublisher;
        private readonly ILog _log;

        public ClientRegulationService(
            IRegulationRepository regulationRepository,
            IClientRegulationRepository clientRegulationRepository,
            IWelcomeRegulationRuleRepository welcomeRegulationRuleRepository,
            IClientRegulationPublisher clientRegulationPublisher,
            ILog log)
        {
            _regulationRepository = regulationRepository;
            _clientRegulationRepository = clientRegulationRepository;
            _welcomeRegulationRuleRepository = welcomeRegulationRuleRepository;
            _clientRegulationPublisher = clientRegulationPublisher;
            _log = log;
        }

        public async Task<IClientRegulation> GetAsync(string clientId, string regulationId)
        {
            IClientRegulation clientRegulation = await _clientRegulationRepository.GetAsync(clientId, regulationId);

            if (clientRegulation == null)
            {
                throw new ServiceException("Client regulation not found.");
            }

            return clientRegulation;
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

        public async Task<IEnumerable<IClientRegulation>> GetByRegulationIdAsync(string regulationId)
        {
            IRegulation result = await _regulationRepository.GetAsync(regulationId);

            if (result == null)
            {
                throw new ServiceException("Regulation not found.");
            }

            return await _clientRegulationRepository.GetByRegulationIdAsync(regulationId);
        }

        public async Task AddAsync(IClientRegulation clientRegulation)
        {
            IRegulation result = await _regulationRepository.GetAsync(clientRegulation.RegulationId);

            if (result == null)
            {
                throw new ServiceException("Regulation not found.");
            }

            if (await _clientRegulationRepository.GetAsync(clientRegulation.ClientId, clientRegulation.RegulationId) != null)
            {
                throw new ServiceException("Client regulation already exists.");
            }

            await _clientRegulationRepository.AddAsync(clientRegulation);

            await PublishOnChangedAsync(clientRegulation.ClientId);

            await _log.WriteInfoAsync(nameof(ClientRegulationService), nameof(AddAsync),
                clientRegulation.ClientId, $"Regulation '{clientRegulation.RegulationId}' added for client.");
        }
        
        public async Task SetDefaultAsync(string clientId, string country)
        {
            IEnumerable<IClientRegulation> clientRegulations =
                await _clientRegulationRepository.GetByClientIdAsync(clientId);

            if (clientRegulations.Any())
            {
                throw new ServiceException("Client already have regulations.");
            }
            
            IWelcomeRegulationRule welcomeRegulationRule = null;

            if (!string.IsNullOrEmpty(country))
            {
                IEnumerable<IWelcomeRegulationRule> welcomeRegulationRules =
                    await _welcomeRegulationRuleRepository.GetByCountryAsync(country);

                welcomeRegulationRule = welcomeRegulationRules
                    .OrderByDescending(o => o.Priority)
                    .FirstOrDefault();
            }

            if (welcomeRegulationRule == null)
            {
                IEnumerable<IWelcomeRegulationRule> welcomeRegulationRules =
                    await _welcomeRegulationRuleRepository.GetDefaultAsync();

                welcomeRegulationRule = welcomeRegulationRules
                    .OrderByDescending(o => o.Priority)
                    .FirstOrDefault();
            }

            if (welcomeRegulationRule == null)
            {
                throw new ServiceException("No default regulations.");
            }

            var defaultClientRegulation = new ClientRegulation
            {
                ClientId = clientId,
                RegulationId = welcomeRegulationRule.RegulationId,
                Active = welcomeRegulationRule.Active,
                Kyc = false
            };

            await _clientRegulationRepository.AddAsync(defaultClientRegulation);

            await PublishOnChangedAsync(defaultClientRegulation.ClientId);

            await _log.WriteInfoAsync(nameof(ClientRegulationService), nameof(SetDefaultAsync),
                defaultClientRegulation.ClientId,
                $"Default regulation '{defaultClientRegulation.RegulationId}' added for client.");
        }
        
        public async Task UpdateKycAsync(string clientId, string regulationId, bool active)
        {
            IClientRegulation clientRegulation = await _clientRegulationRepository.GetAsync(clientId, regulationId);

            if (clientRegulation == null)
            {
                throw new ServiceException("Client regulation not found.");
            }

            clientRegulation.Kyc = active;

            await _clientRegulationRepository.UpdateAsync(clientRegulation);

            await PublishOnChangedAsync(clientId);
        }

        public async Task UpdateActiveAsync(string clientId, string regulationId, bool state)
        {
            IClientRegulation clientRegulation = await _clientRegulationRepository.GetAsync(clientId, regulationId);

            if (clientRegulation == null)
            {
                throw new ServiceException("Client regulation not found.");
            }

            clientRegulation.Active = state;

            await _clientRegulationRepository.UpdateAsync(clientRegulation);

            await PublishOnChangedAsync(clientId);
        }

        public async Task DeleteAsync(string clientId, string regulationId)
        {
            IClientRegulation clientRegulation = await _clientRegulationRepository.GetAsync(clientId, regulationId);

            if (clientRegulation == null)
            {
                throw new ServiceException("Client regulation not found.");
            }

            if (clientRegulation.Kyc)
            {
                throw new ServiceException("Can not delete KYC client regulation.");
            }

            if (clientRegulation.Active)
            {
                throw new ServiceException("Can not delete active client regulation.");
            }

            await _clientRegulationRepository.DeleteAsync(clientId, regulationId);

            await PublishOnChangedAsync(clientId);
        }

        private async Task PublishOnChangedAsync(string clientId)
        {
            IEnumerable<IClientRegulation> clientRegulations =
                await _clientRegulationRepository.GetByClientIdAsync(clientId);

            var message = new ClientRegulationsMessage
            {
                ClientId = clientId,
                Regulations = clientRegulations.Select(o => new Core.Contracts.ClientRegulation
                {
                    RegulationId = o.RegulationId,
                    Kyc = o.Kyc,
                    Active = o.Active
                }).ToList()
            };

            await _clientRegulationPublisher.PublishAsync(message);
        }
    }
}
