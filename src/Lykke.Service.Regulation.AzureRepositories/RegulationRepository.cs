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
            return _tableStorage.InsertThrowConflict(Create(regulation));
        }

        public Task UpdateAsync(IRegulation regulation)
        {
            return _tableStorage.ReplaceAsync(GetPartitionKey(), GetRowKey(regulation.Id), entity =>
            {
                entity.ProfileType = regulation.ProfileType;
                entity.TermsOfUseUrl = regulation.TermsOfUseUrl;
                entity.RiskDescriptionUrl = regulation.RiskDescriptionUrl;
                entity.MarginTradingConditions = regulation.MarginTradingConditions;
                return entity;
            });
        }

        public async Task DeleteAsync(string regulationId)
        {
            await _tableStorage.DeleteAsync(GetPartitionKey(), GetRowKey(regulationId));
        }

        private static string GetPartitionKey()
            => "Regulation";

        private static string GetRowKey(string regulationId)
            => regulationId.ToLower();

        private static RegulationEntity Create(IRegulation regulation)
        {
            return new RegulationEntity
            {
                PartitionKey = GetPartitionKey(),
                RowKey = GetRowKey(regulation.Id),
                ProfileType = regulation.ProfileType,
                TermsOfUseUrl = regulation.TermsOfUseUrl,
                RiskDescriptionUrl = regulation.RiskDescriptionUrl,
                MarginTradingConditions = regulation.MarginTradingConditions
            };
        }
    }
}
