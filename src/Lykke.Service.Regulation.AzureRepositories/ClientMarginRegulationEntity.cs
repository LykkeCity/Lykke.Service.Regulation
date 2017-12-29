using Lykke.Service.Regulation.Core.Domain;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Regulation.AzureRepositories
{
    public class ClientMarginRegulationEntity : TableEntity, IClientMarginRegulation
    {
        public ClientMarginRegulationEntity()
        {
        }

        public ClientMarginRegulationEntity(string partitionKey, string rowKey)
            : base(partitionKey, rowKey)
        {
        }

        public string Id => RowKey;
        public string ClientId { get; set; }
        public string RegulationId { get; set; }
    }
}
