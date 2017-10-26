using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.Regulation.Core.Domain;
using Lykke.Service.Regulation.Core.Repositories;

namespace Lykke.Service.Regulation.AzureRepositories
{
    public class RegulationRepository : IRegulationRepository
    {
        private readonly INoSQLTableStorage<RegulationEntity> _tableStorage;

        public RegulationRepository(INoSQLTableStorage<RegulationEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IRegulation> GetAsync(string regulationId)
        {
            var partitionKey = RegulationEntity.GeneratePartitionKey();
            var rowKey = RegulationEntity.GenerateRowKey(regulationId);

            return await _tableStorage.GetDataAsync(partitionKey, rowKey);
        }

        public async Task<IEnumerable<IRegulation>> GetAllAsync()
        {
            var partitionKey = RegulationEntity.GeneratePartitionKey();

            return await _tableStorage.GetDataAsync(partitionKey);
        }

        public Task AddAsync(IRegulation regulation)
        {
            RegulationEntity entity = RegulationEntity.Create(regulation.Id, regulation.RequiresKYC);

            return _tableStorage.InsertOrReplaceAsync(entity);
        }

        public async Task RemoveAsync(string regulationId)
        {
            var partitionKey = RegulationEntity.GeneratePartitionKey();
            var rowKey = RegulationEntity.GenerateRowKey(regulationId);

            await _tableStorage.DeleteAsync(partitionKey, rowKey);
        }

        public async Task UpdateAsync(IRegulation regulation)
        {
            var partitionKey = RegulationEntity.GeneratePartitionKey();
            var rowKey = RegulationEntity.GenerateRowKey(regulation.Id);

            await _tableStorage.MergeAsync(partitionKey, rowKey, item =>
            {
                item.RequiresKYC = regulation.RequiresKYC;
                return item;
            });
        }
    }
}
