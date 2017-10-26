using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.Regulation.Core.Domain;
using Lykke.Service.Regulation.Core.Repositories;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Regulation.AzureRepositories
{
    public class ClientAvailableRegulationRepository : IClientAvailableRegulationRepository
    {
        private readonly INoSQLTableStorage<ClientAvailableRegulationEntity> _tableStorage;

        public ClientAvailableRegulationRepository(INoSQLTableStorage<ClientAvailableRegulationEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }
        
        public async Task<IEnumerable<IClientAvailableRegulation>> GetAllAsync()
        {
            var partitionKey = ClientAvailableRegulationEntity.GeneratePartitionKey();

            return await _tableStorage.GetDataAsync(partitionKey);
        }

        public async Task<IEnumerable<IClientAvailableRegulation>> GetByClientIdAsync(string clientId)
        {
            var partitionKey = ClientAvailableRegulationEntity.GeneratePartitionKey();

            string partitionKeyFilter = TableQuery
                .GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey);

            string clientIdFilter = TableQuery
                .GenerateFilterCondition("ClientId", QueryComparisons.Equal, clientId);

            var query = new TableQuery<ClientAvailableRegulationEntity>()
                .Where(TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And, clientIdFilter));

            return await _tableStorage.WhereAsync(query);
        }

        public async Task<IEnumerable<IClientAvailableRegulation>> GetByRegulationIdAsync(string regulationId)
        {
            var partitionKey = ClientAvailableRegulationEntity.GeneratePartitionKey();

            string partitionKeyFilter = TableQuery
                .GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey);

            string regulationIdFilter = TableQuery
                .GenerateFilterCondition("RegulationId", QueryComparisons.Equal, regulationId);

            var query = new TableQuery<ClientAvailableRegulationEntity>()
                .Where(TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And, regulationIdFilter));

            return await _tableStorage.WhereAsync(query);
        }

        public Task AddAsync(IClientAvailableRegulation clientRegulation)
        {
            ClientAvailableRegulationEntity entity =
                ClientAvailableRegulationEntity.Create(clientRegulation.ClientId, clientRegulation.RegulationId);

            return _tableStorage.InsertOrReplaceAsync(entity);
        }

        public async Task RemoveAsync(string clientId, string regulationId)
        {
            var partitionKey = ClientAvailableRegulationEntity.GeneratePartitionKey();
            var rowKey = ClientAvailableRegulationEntity.GenerateRowKey(clientId, regulationId);

            await _tableStorage.DeleteAsync(partitionKey, rowKey);
        }
    }
}
