using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.Regulation.Core.Domain;
using Lykke.Service.Regulation.Core.Repositories;

namespace Lykke.Service.Regulation.AzureRepositories
{
    public class ClientRegulationRepository: IClientRegulationRepository
    {
        private readonly INoSQLTableStorage<ClientRegulationEntity> _tableStorage;

        public ClientRegulationRepository(INoSQLTableStorage<ClientRegulationEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IClientRegulation> GetAsync(string clientId)
        {
            var partitionKey = ClientRegulationEntity.GeneratePartitionKey();
            var rowKey = ClientRegulationEntity.GenerateRowKey(clientId);

            return await _tableStorage.GetDataAsync(partitionKey, rowKey);
        }

        public Task AddAsync(IClientRegulation clientAvailableRegulation)
        {
            ClientRegulationEntity entity =
                ClientRegulationEntity.Create(clientAvailableRegulation.ClientId, clientAvailableRegulation.RegulationId);

            return _tableStorage.InsertOrReplaceAsync(entity);
        }

        public async Task RemoveAsync(string clientId)
        {
            var partitionKey = ClientRegulationEntity.GeneratePartitionKey();
            var rowKey = ClientRegulationEntity.GenerateRowKey(clientId);

            await _tableStorage.DeleteAsync(partitionKey, rowKey);
        }
    }
}
