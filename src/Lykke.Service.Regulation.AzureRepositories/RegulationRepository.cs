using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.Regulation.AzureRepositories.Extensions;
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
            return await _tableStorage.GetDataAsync(GetPartitionKey(), GetRowKey(regulationId));
        }

        public async Task<IEnumerable<IRegulation>> GetAllAsync()
        {
            return await _tableStorage.GetDataAsync(GetPartitionKey());
        }

        public Task AddAsync(IRegulation regulation)
        {
            return _tableStorage.InsertThrowConflict(Create(regulation.Id));
        }

        public async Task DeleteAsync(string regulationId)
        {
            await _tableStorage.DeleteAsync(GetPartitionKey(), GetRowKey(regulationId));
        }

        private static string GetPartitionKey()
            => "Regulation";

        private static string GetRowKey(string regulationId)
            => regulationId;

        private static RegulationEntity Create(string regulationId)
        {
            return new RegulationEntity
            {
                PartitionKey = GetPartitionKey(),
                RowKey = GetRowKey(regulationId)
            };
        }
    }
}
