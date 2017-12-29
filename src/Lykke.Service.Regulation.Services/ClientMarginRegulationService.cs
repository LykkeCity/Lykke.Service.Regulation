using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.Regulation.Core.Domain;
using Lykke.Service.Regulation.Core.Exceptions;
using Lykke.Service.Regulation.Core.Repositories;
using Lykke.Service.Regulation.Core.Services;

namespace Lykke.Service.Regulation.Services
{
    public class ClientMarginRegulationService : IClientMarginRegulationService
    {
        private readonly IRegulationRepository _regulationRepository;
        private readonly IClientMarginRegulationRepository _clientMarginRegulationRepository;
        private readonly ILog _log;

        public ClientMarginRegulationService(
            IRegulationRepository regulationRepository,
            IClientMarginRegulationRepository clientMarginRegulationRepository,
            ILog log)
        {
            _regulationRepository = regulationRepository;
            _clientMarginRegulationRepository = clientMarginRegulationRepository;
            _log = log;
        }

        public async Task<IClientMarginRegulation> GetAsync(string clientId)
        {
            return await _clientMarginRegulationRepository.GetAsync(clientId);
        }

        public async Task SetAsync(string clientId, string regulationId)
        {
            IRegulation result = await _regulationRepository.GetAsync(regulationId);

            if (result == null)
            {
                throw new ServiceException("Regulation not found.");
            }

            await _clientMarginRegulationRepository.DeleteAsync(clientId);

            await _clientMarginRegulationRepository.InsertAsync(new ClientMarginRegulation
            {
                ClientId = clientId,
                RegulationId = regulationId
            });
        }

        public async Task AddAsync(string clientId, string regulationId)
        {
            IRegulation result = await _regulationRepository.GetAsync(regulationId);

            if (result == null)
            {
                throw new ServiceException("Regulation not found.");
            }

            if (await _clientMarginRegulationRepository.GetAsync(clientId) != null)
            {
                throw new ServiceException("Client margin regulation already exists.");
            }

            await _clientMarginRegulationRepository.InsertAsync(new ClientMarginRegulation
            {
                ClientId = clientId,
                RegulationId = regulationId
            });

            await _log.WriteInfoAsync(nameof(ClientMarginRegulationService), nameof(AddAsync),
                clientId, $"Margin regulation '{regulationId}' added.");
        }

        public async Task DeleteAsync(string clientId)
        {
            await _clientMarginRegulationRepository.DeleteAsync(clientId);

            await _log.WriteInfoAsync(nameof(ClientMarginRegulationService), nameof(DeleteAsync),
                clientId, "Margin regulation deleted.");
        }
    }
}
