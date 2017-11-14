using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.Regulation.AzureRepositories.Extensions;
using Lykke.Service.Regulation.Core.Domain;
using Lykke.Service.Regulation.Core.Repositories;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Regulation.AzureRepositories
{
    public class ClientRegulationRepository : IClientRegulationRepository
    {
        private readonly INoSQLTableStorage<ClientRegulationEntity> _tableStorage;

        public ClientRegulationRepository(INoSQLTableStorage<ClientRegulationEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IClientRegulation> GetAsync(string clientId, string regulationId)
        {
            var partitionKey = ClientRegulationEntity.GeneratePartitionKey();
            var rowKey = ClientRegulationEntity.GenerateRowKey(clientId, regulationId);

            return await _tableStorage.GetDataAsync(partitionKey, rowKey);
        }

        public async Task<IEnumerable<IClientRegulation>> GetByClientIdAsync(string clientId)
        {
            var partitionKey = ClientRegulationEntity.GeneratePartitionKey();

            string partitionKeyFilter = TableQuery
                .GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey);

            string clientIdFilter = TableQuery
                .GenerateFilterCondition("ClientId", QueryComparisons.Equal, clientId);

            string filter = TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And, clientIdFilter);

            var query = new TableQuery<ClientRegulationEntity>().Where(filter);

            return await _tableStorage.WhereAsync(query);
        }

        public async Task<IEnumerable<IClientRegulation>> GetByRegulationIdAsync(string regulationId)
        {
            var partitionKey = ClientRegulationEntity.GeneratePartitionKey();

            string partitionKeyFilter = TableQuery
                .GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey);

            string regulationIdFilter = TableQuery
                .GenerateFilterCondition("RegulationId", QueryComparisons.Equal, regulationId);

            string filter = TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And, regulationIdFilter);

            var query = new TableQuery<ClientRegulationEntity>().Where(filter);

            return await _tableStorage.WhereAsync(query);
        }

        public async Task<IEnumerable<IClientRegulation>> GetActiveByClientIdAsync(string clientId)
        {
            var partitionKey = ClientRegulationEntity.GeneratePartitionKey();

            string partitionKeyFilter = TableQuery
                .GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey);

            string clientIdFilter = TableQuery
                .GenerateFilterCondition("ClientId", QueryComparisons.Equal, clientId);

            string activeFilter = TableQuery
                .GenerateFilterConditionForBool("Active", QueryComparisons.Equal, true);

            string filter = TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And, clientIdFilter);
            filter = TableQuery.CombineFilters(filter, TableOperators.And, activeFilter);

            var query = new TableQuery<ClientRegulationEntity>().Where(filter);

            return await _tableStorage.WhereAsync(query);
        }

        public async Task<IEnumerable<IClientRegulation>> GetAvailableByClientIdAsync(string clientId)
        {
            var partitionKey = ClientRegulationEntity.GeneratePartitionKey();

            string partitionKeyFilter = TableQuery
                .GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey);

            string clientIdFilter = TableQuery
                .GenerateFilterCondition("ClientId", QueryComparisons.Equal, clientId);

            string activeFilter = TableQuery
                .GenerateFilterConditionForBool("Active", QueryComparisons.Equal, true);

            string kycFilter = TableQuery
                .GenerateFilterConditionForBool("Kyc", QueryComparisons.Equal, true);

            string filter = TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And, clientIdFilter);
            filter = TableQuery.CombineFilters(filter, TableOperators.And, activeFilter);
            filter = TableQuery.CombineFilters(filter, TableOperators.And, kycFilter);

            var query = new TableQuery<ClientRegulationEntity>().Where(filter);

            return await _tableStorage.WhereAsync(query);
        }

        public Task AddAsync(IClientRegulation clientRegulation)
        {
            ClientRegulationEntity entity = ClientRegulationEntity.Create(clientRegulation);

            return _tableStorage.InsertNoConflict(entity);
        }

        public Task AddAsync(IEnumerable<IClientRegulation> clientRegulations)
        {
            IEnumerable<ClientRegulationEntity> entities = clientRegulations.Select(ClientRegulationEntity.Create);

            return _tableStorage.InsertOrReplaceBatchAsync(entities);
        }

        public Task UpdateAsync(IClientRegulation clientRegulation)
        {
            var partitionKey = ClientRegulationEntity.GeneratePartitionKey();
            var rowKey = ClientRegulationEntity.GenerateRowKey(clientRegulation.ClientId, clientRegulation.RegulationId);

            return _tableStorage.MergeAsync(partitionKey, rowKey, entity =>
            {
                entity.Kyc = clientRegulation.Kyc;
                entity.Active = clientRegulation.Active;
                return entity;
            });
        }

        public async Task DeleteAsync(string clientId, string regulationId)
        {
            var partitionKey = ClientRegulationEntity.GeneratePartitionKey();
            var rowKey = ClientRegulationEntity.GenerateRowKey(clientId, regulationId);
            
            await _tableStorage.DeleteAsync(partitionKey, rowKey);
        }
    }
}
