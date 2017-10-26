using Lykke.Service.Regulation.Core.Domain;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Regulation.AzureRepositories
{
    public class ClientRegulationEntity : TableEntity, IClientRegulation
    {
        public string ClientId => RowKey;

        public string RegulationId { get; set; }

        internal static string GeneratePartitionKey()
        {
            return "ClientRegulation";
        }

        internal static string GenerateRowKey(string clientId)
        {
            return clientId;
        }

        internal static ClientRegulationEntity Create(string clientId, string regulationId)
        {
            return new ClientRegulationEntity
            {
                RowKey = GenerateRowKey(clientId),
                PartitionKey = GeneratePartitionKey(),
                RegulationId = regulationId
            };
        }
    }
}
