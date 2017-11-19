using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using Common;
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
            return await _tableStorage.GetDataAsync(GetPartitionKey(clientId), GetRowKey(regulationId));
        }

        public async Task<IEnumerable<IClientRegulation>> GetByClientIdAsync(string clientId)
        {
            return await _tableStorage.GetDataAsync(GetPartitionKey(clientId));
        }

        public async Task<IEnumerable<IClientRegulation>> GetByRegulationIdAsync(string regulationId)
        {
            string rowKeyFilter = TableQuery
                .GenerateFilterCondition(nameof(ClientRegulationEntity.RowKey), QueryComparisons.Equal,
                    GetRowKey(regulationId));
            
            var query = new TableQuery<ClientRegulationEntity>().Where(rowKeyFilter);

            return await _tableStorage.WhereAsync(query);
        }

        public async Task<IEnumerable<IClientRegulation>> GetActiveByClientIdAsync(string clientId)
        {
            string partitionKeyFilter = TableQuery
                .GenerateFilterCondition(nameof(ClientRegulationEntity.PartitionKey), QueryComparisons.Equal,
                    GetPartitionKey(clientId));
            
            string activeFilter = TableQuery
                .GenerateFilterConditionForBool(nameof(ClientRegulationEntity.Active), QueryComparisons.Equal, true);

            string filter = TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And, activeFilter);

            var query = new TableQuery<ClientRegulationEntity>().Where(filter);

            return await _tableStorage.WhereAsync(query);
        }

        public async Task<IEnumerable<IClientRegulation>> GetAvailableByClientIdAsync(string clientId)
        {
            string partitionKeyFilter = TableQuery
                .GenerateFilterCondition(nameof(ClientRegulationEntity.PartitionKey), QueryComparisons.Equal,
                    GetPartitionKey(clientId));

            string activeFilter = TableQuery
                .GenerateFilterConditionForBool(nameof(ClientRegulationEntity.Active), QueryComparisons.Equal, true);

            string kycFilter = TableQuery
                .GenerateFilterConditionForBool(nameof(ClientRegulationEntity.Kyc), QueryComparisons.Equal, true);

            string filter = TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And, activeFilter);
            filter = TableQuery.CombineFilters(filter, TableOperators.And, kycFilter);

            var query = new TableQuery<ClientRegulationEntity>().Where(filter);

            return await _tableStorage.WhereAsync(query);
        }

        public Task AddAsync(IClientRegulation clientRegulation)
        {
            return _tableStorage.InsertThrowConflict(Create(clientRegulation));
        }

        public Task AddAsync(IEnumerable<IClientRegulation> clientRegulations)
        {
            IEnumerable<ClientRegulationEntity> entities = clientRegulations.Select(Create);

            return _tableStorage.InsertOrReplaceBatchAsync(entities);
        }

        public Task UpdateAsync(IClientRegulation clientRegulation)
        {
            string partitionKey = GetPartitionKey(clientRegulation.ClientId);
            string rowKey = GetRowKey(clientRegulation.RegulationId);

            return _tableStorage.MergeAsync(partitionKey, rowKey, entity =>
            {
                entity.Kyc = clientRegulation.Kyc;
                entity.Active = clientRegulation.Active;
                return entity;
            });
        }

        public async Task DeleteAsync(string clientId, string regulationId)
        {
            await _tableStorage.DeleteAsync(GetPartitionKey(clientId), GetRowKey(regulationId));
        }

        private static string GetPartitionKey(string clientId)
            => clientId.ToLower();

        private static string GetRowKey(string regulationId)
            => $"regulation_{regulationId}".ToLowCase();

        private static ClientRegulationEntity Create(IClientRegulation clientRegulation)
        {
            return new ClientRegulationEntity
            {
                RowKey = GetRowKey(clientRegulation.RegulationId),
                PartitionKey = GetPartitionKey(clientRegulation.ClientId),
                ClientId = clientRegulation.ClientId,
                RegulationId = clientRegulation.RegulationId,
                Kyc = false,
                Active = clientRegulation.Active
            };
        }
    }
}
