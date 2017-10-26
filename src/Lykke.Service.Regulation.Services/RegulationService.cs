using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.Regulation.Core.Domain;
using Lykke.Service.Regulation.Core.Repositories;
using Lykke.Service.Regulation.Core.Services;

namespace Lykke.Service.Regulation.Services
{
    public class RegulationService : IRegulationService
    {
        private readonly IRegulationRepository _regulationRepository;
        private readonly IClientAvailableRegulationRepository _clientAvailableRegulationRepository;

        public RegulationService(
            IRegulationRepository regulationRepository,
            IClientAvailableRegulationRepository clientAvailableRegulationRepository)
        {
            _regulationRepository = regulationRepository;
            _clientAvailableRegulationRepository = clientAvailableRegulationRepository;
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

        public async Task RemoveAsync(string regulationId)
        {
            IEnumerable<IClientAvailableRegulation> clientAvailableRegulations =
                await _clientAvailableRegulationRepository.GetByRegulationIdAsync(regulationId);

            if (clientAvailableRegulations.Any())
            {
                throw new Exception("Can not remove regulation. It assosiated with one or more clients.");
            }

            await _regulationRepository.RemoveAsync(regulationId);
        }

        public Task UpdateAsync(IRegulation regulation)
        {
            return _regulationRepository.UpdateAsync(regulation);
        }
    }
}
