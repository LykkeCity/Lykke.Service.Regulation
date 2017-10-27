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
        private readonly IClientAvailableRegulationRepository _clientAvailableRegulationRepository;

        public ClientRegulationService(
            IRegulationRepository regulationRepository,
            IClientRegulationRepository clientRegulationRepository, 
            IClientAvailableRegulationRepository clientAvailableRegulationRepository)
        {
            _regulationRepository = regulationRepository;
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

        public async Task AddAsync(IClientAvailableRegulation availableRegulation)
        {
            IRegulation result = await _regulationRepository.GetAsync(availableRegulation.RegulationId);

            if (result == null)
            {
                throw new ServiceException($"Regulation '{availableRegulation.RegulationId}' not found'.");
            }

            if (!result.RequiresKYC)
            {
                throw new ServiceException($"Regulation '{availableRegulation.RegulationId}' is not KYC'.");
            }

            await _clientAvailableRegulationRepository.AddAsync(availableRegulation);
        }

        public async Task SetAsync(IClientRegulation clientRegulation)
        {
            IEnumerable<IClientAvailableRegulation> availableRegulations =
                await _clientAvailableRegulationRepository.GetByClientIdAsync(clientRegulation.ClientId);

            if (availableRegulations.All(o => o.RegulationId != clientRegulation.RegulationId))
            {
                throw new ServiceException($"Regulation '{clientRegulation.RegulationId}' is not available for client '{clientRegulation.ClientId}'.");
            }

            await _clientRegulationRepository.AddAsync(clientRegulation);
        }

        public async Task RemoveAsync(string clientId)
        {
            await _clientRegulationRepository.RemoveAsync(clientId);
        }

        public async Task RemoveAvailableAsync(string clientId, string regulationId)
        {
            IClientRegulation currentClientRegulation = await _clientRegulationRepository.GetAsync(clientId);

            if (currentClientRegulation?.RegulationId == regulationId)
            {
                throw new ServiceException($"Can not remove regulation '{regulationId}'. It is current regulation of client '{clientId}'.");
            }

            await _clientAvailableRegulationRepository.RemoveAsync(clientId, regulationId);
        }
    }
}
