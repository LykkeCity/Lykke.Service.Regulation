using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.Regulation.Core.Domain;
using Lykke.Service.Regulation.Core.Repositories;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Regulation.AzureRepositories
{
    public class MarginRegulationRuleRepository : IMarginRegulationRuleRepository
    {
        private readonly INoSQLTableStorage<MarginRegulationRuleEntity> _storage;

        public MarginRegulationRuleRepository(INoSQLTableStorage<MarginRegulationRuleEntity> storage)
        {
            _storage = storage;
        }

        public async Task<IMarginRegulationRule> GetAsync(string ruleId)
        {
            return await _storage.GetDataAsync(GetPartitionKey(), ruleId);
        }

        public async Task<IEnumerable<IMarginRegulationRule>> GetAllAsync()
        {
            return await _storage.GetDataAsync(GetPartitionKey());
        }

        public async Task<IEnumerable<IMarginRegulationRule>> GetByCountryAsync(string country)
        {
            return await _storage.GetDataAsync(GetPartitionKey(),
                entity => entity.Countries.Any(p => p.Equals(country, StringComparison.CurrentCultureIgnoreCase)));
        }

        public async Task<IEnumerable<IMarginRegulationRule>> GetByRegulationIdAsync(string regulationId)
        {
            string partitionKeyFilter = TableQuery
                .GenerateFilterCondition(nameof(MarginRegulationRuleEntity.PartitionKey), QueryComparisons.Equal, GetPartitionKey());

            string regulationIdFilter = TableQuery
                .GenerateFilterCondition(nameof(MarginRegulationRuleEntity.RegulationId), QueryComparisons.Equal, regulationId);

            var query = new TableQuery<MarginRegulationRuleEntity>()
                .Where(TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And, regulationIdFilter));

            return await _storage.WhereAsync(query);
        }

        public async Task InsertAsync(IMarginRegulationRule rule)
        {
            var entity = new MarginRegulationRuleEntity
            {
                PartitionKey = GetPartitionKey(),
                RowKey = GetRowKey(),
                Name = rule.Name,
                RegulationId = rule.RegulationId,
                Countries = rule.Countries,
                Priority = rule.Priority
            };

            await _storage.InsertAsync(entity);
        }

        public async Task UpdateAsync(IMarginRegulationRule rule)
        {
            await _storage.MergeAsync(GetPartitionKey(), rule.Id, entity =>
            {
                entity.Name = rule.Name;
                entity.Countries = rule.Countries;
                entity.RegulationId = rule.RegulationId;
                entity.Priority = rule.Priority;
                return entity;
            });
        }

        public async Task DeleteAsync(string ruleId)
        {
            await _storage.DeleteAsync(GetPartitionKey(), ruleId);
        }

        private static string GetPartitionKey()
            => "MarginRegulationRule";

        private static string GetRowKey()
            => Guid.NewGuid().ToString("D");
    }
}
