using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.Regulation.AzureRepositories.Extensions;
using Lykke.Service.Regulation.Core.Domain;
using Lykke.Service.Regulation.Core.Repositories;

namespace Lykke.Service.Regulation.AzureRepositories
{
    public  class ClientMarginRegulationRepository : IClientMarginRegulationRepository
    {
        private readonly INoSQLTableStorage<ClientMarginRegulationEntity> _storage;

        public ClientMarginRegulationRepository(INoSQLTableStorage<ClientMarginRegulationEntity> storage)
        {
            _storage = storage;
        }

        public async Task<IClientMarginRegulation> GetAsync(string clientId)
        {
            IEnumerable<IClientMarginRegulation> regulations = await _storage.GetDataAsync(GetPartitionKey(clientId));

            return regulations?.FirstOrDefault();
        }

        public async Task InsertAsync(IClientMarginRegulation clientMarginRegulation)
        {
            await _storage.InsertThrowConflict(new ClientMarginRegulationEntity
            {
                PartitionKey = GetPartitionKey(clientMarginRegulation.ClientId),
                RowKey = GetRowKey(clientMarginRegulation.RegulationId),
                RegulationId = clientMarginRegulation.RegulationId,
                ClientId = clientMarginRegulation.ClientId
            });
        }

        public async Task DeleteAsync(string clientId)
        {
            IEnumerable<ClientMarginRegulationEntity> entities =
                await _storage.GetDataAsync(GetPartitionKey(clientId));

            await _storage.DeleteAsync(entities);
        }

        private static string GetPartitionKey(string clientId)
            => clientId.ToLower();

        private static string GetRowKey(string regulationId)
            => regulationId.ToLower();
    }
}
