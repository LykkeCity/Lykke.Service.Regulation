using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.Regulation.Core.Domain;
using Lykke.Service.Regulation.Core.Repositories;
using Lykke.Service.Regulation.Core.Services;

namespace Lykke.Service.Regulation.Services
{
    public class ClientRegulationService : IClientRegulationService
    {
        private readonly IClientRegulationRepository _clientRegulationRepository;
        private readonly IClientAvailableRegulationRepository _clientAvailableRegulationRepository;

        public ClientRegulationService(
            IClientRegulationRepository clientRegulationRepository, 
            IClientAvailableRegulationRepository clientAvailableRegulationRepository)
        {
            _clientRegulationRepository = clientRegulationRepository;
            _clientAvailableRegulationRepository = clientAvailableRegulationRepository;
        }

        public async Task<string> GetAsync(string clientId)
        {
            IClientRegulation regulation = await _clientRegulationRepository.GetAsync(clientId);

            return regulation?.RegulationId;
        }

        public async Task<IEnumerable<string>> GetAvailableAsync(string clientId)
        {
            IEnumerable<IClientAvailableRegulation> regulations = await _clientAvailableRegulationRepository.GetByClientIdAsync(clientId);

            return regulations.Select(o => o.RegulationId);
        }

        public async Task AddAsync(IClientAvailableRegulation regulation)
        {
            await _clientAvailableRegulationRepository.AddAsync(regulation);
        }

        public async Task SetAsync(IClientRegulation regulation)
        {
            IEnumerable<IClientAvailableRegulation> availableRegulations =
                await _clientAvailableRegulationRepository.GetByClientIdAsync(regulation.ClientId);

            if (availableRegulations.All(o => o.RegulationId != regulation.RegulationId))
            {
                throw new Exception($"Client '{regulation.ClientId}' have not available regulation '{regulation.RegulationId}'.");
            }

            await _clientRegulationRepository.AddAsync(regulation);
        }

        public async Task Remove(string clientId)
        {
            await _clientRegulationRepository.RemoveAsync(clientId);
        }

        public async Task RemoveAvailable(string clientId, string regulationId)
        {
            string currentClientRegulation = await GetAsync(clientId);

            if (currentClientRegulation == regulationId)
            {
                throw new Exception($"Can not remove regulation. It is a current client regulation.");
            }

            await _clientAvailableRegulationRepository.RemoveAsync(clientId, regulationId);
        }
    }
}
