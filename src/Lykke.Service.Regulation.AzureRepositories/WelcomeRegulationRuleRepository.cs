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
    public class WelcomeRegulationRuleRepository : IWelcomeRegulationRuleRepository
    {
        private readonly INoSQLTableStorage<WelcomeRegulationRuleEntity> _tableStorage;

        public WelcomeRegulationRuleRepository(INoSQLTableStorage<WelcomeRegulationRuleEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IWelcomeRegulationRule> GetAsync(string regulationRuleId)
        {
            return await _tableStorage.GetDataAsync(GetPartitionKey(), regulationRuleId);
        }

        public async Task<IEnumerable<IWelcomeRegulationRule>> GetAllAsync()
        {
            return await _tableStorage.GetDataAsync(GetPartitionKey());
        }

        public async Task<IEnumerable<IWelcomeRegulationRule>> GetByCountryAsync(string country)
        {
            IEnumerable<IWelcomeRegulationRule> entities = await GetAllAsync();

            return entities
                .Where(o => o.Countries.Any(p => p.Equals(country, StringComparison.CurrentCultureIgnoreCase)));
        }

        public async Task<IEnumerable<IWelcomeRegulationRule>> GetByRegulationIdAsync(string regulationId)
        {
            string partitionKeyFilter = TableQuery
                .GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, GetPartitionKey());

            string regulationIdFilter = TableQuery
                .GenerateFilterCondition("RegulationId", QueryComparisons.Equal, regulationId);

            var query = new TableQuery<WelcomeRegulationRuleEntity>()
                .Where(TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And, regulationIdFilter));

            return await _tableStorage.WhereAsync(query);
        }

        public Task AddAsync(IWelcomeRegulationRule welcomeRegulationRule)
        {
            return _tableStorage.InsertAsync(Create(welcomeRegulationRule));
        }

        public Task UpdateAsync(IWelcomeRegulationRule welcomeRegulationRule)
        {
            return _tableStorage.MergeAsync(GetPartitionKey(), welcomeRegulationRule.Id, entity =>
            {
                entity.Name = welcomeRegulationRule.Name;
                entity.Countries = welcomeRegulationRule.Countries;
                entity.RegulationId = welcomeRegulationRule.RegulationId;
                entity.Active = welcomeRegulationRule.Active;
                entity.Priority = welcomeRegulationRule.Priority;
                return entity;
            });
        }

        public async Task DeleteAsync(string regulationRuleId)
        {
            await _tableStorage.DeleteAsync(GetPartitionKey(), regulationRuleId);
        }

        private static string GetPartitionKey()
            => "WelcomeRegulationRule";

        private static string GetRowKey()
            => Guid.NewGuid().ToString("D");

        private static WelcomeRegulationRuleEntity Create(IWelcomeRegulationRule welcomeRegulationRule)
            => new WelcomeRegulationRuleEntity
            {
                RowKey = GetRowKey(),
                PartitionKey = GetPartitionKey(),
                Name = welcomeRegulationRule.Name,
                Countries = welcomeRegulationRule.Countries,
                RegulationId = welcomeRegulationRule.RegulationId,
                Active = welcomeRegulationRule.Active,
                Priority = welcomeRegulationRule.Priority
            };
    }
}
