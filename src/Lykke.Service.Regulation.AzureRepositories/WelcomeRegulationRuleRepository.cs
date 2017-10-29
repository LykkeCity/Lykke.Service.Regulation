using System.Collections.Generic;
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

        public async Task<IEnumerable<IWelcomeRegulationRule>> GetAllAsync()
        {
            var partitionKey = WelcomeRegulationRuleEntity.GeneratePartitionKey();

            return await _tableStorage.GetDataAsync(partitionKey);
        }

        public async Task<IEnumerable<IWelcomeRegulationRule>> GetByCountryAsync(string country)
        {
            var partitionKey = WelcomeRegulationRuleEntity.GeneratePartitionKey();

            string partitionKeyFilter = TableQuery
                .GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey);

            string countryFilter = TableQuery
                .GenerateFilterCondition("Country", QueryComparisons.Equal, country);

            var query = new TableQuery<WelcomeRegulationRuleEntity>()
                .Where(TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And, countryFilter));

            return await _tableStorage.WhereAsync(query);
        }

        public async Task<IEnumerable<IWelcomeRegulationRule>> GetByRegulationIdAsync(string regulationId)
        {
            var partitionKey = WelcomeRegulationRuleEntity.GeneratePartitionKey();

            string partitionKeyFilter = TableQuery
                .GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey);

            string regulationIdFilter = TableQuery
                .GenerateFilterCondition("RegulationId", QueryComparisons.Equal, regulationId);

            var query = new TableQuery<WelcomeRegulationRuleEntity>()
                .Where(TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And, regulationIdFilter));

            return await _tableStorage.WhereAsync(query);
        }

        public Task AddAsync(IWelcomeRegulationRule welcomeRegulationRule)
        {
            WelcomeRegulationRuleEntity entity = WelcomeRegulationRuleEntity
                .Create(welcomeRegulationRule.Country, welcomeRegulationRule.RegulationId);

            return _tableStorage.InsertOrReplaceAsync(entity);
        }

        public async Task DeleteAsync(string id)
        {
            var partitionKey = WelcomeRegulationRuleEntity.GeneratePartitionKey();
            
            await _tableStorage.DeleteAsync(partitionKey, id);
        }
    }
}
