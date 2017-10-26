using Lykke.Service.Regulation.Core.Domain;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Regulation.AzureRepositories
{
    public class RegulationEntity : TableEntity, IRegulation
    {
        public string Id => RowKey;

        public bool RequiresKYC { get; set; }

        internal static string GeneratePartitionKey()
        {
            return "Regulation";
        }

        internal static string GenerateRowKey(string id)
        {
            return id;
        }

        internal static RegulationEntity Create(string id, bool requiresKYC)
        {
            return new RegulationEntity
            {
                RowKey = GenerateRowKey(id),
                PartitionKey = GeneratePartitionKey(),
                RequiresKYC = requiresKYC
            };
        }
    }
}
