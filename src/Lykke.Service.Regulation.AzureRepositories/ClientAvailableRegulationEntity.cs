using Lykke.Service.Regulation.Core.Domain;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Regulation.AzureRepositories
{
    public class ClientAvailableRegulationEntity : TableEntity, IClientAvailableRegulation
    {
        public string ClientId { get; set; } 

        public string RegulationId { get; set; }

        internal static string GeneratePartitionKey()
        {
            return "ClientAvailableRegulation";
        }

        internal static string GenerateRowKey(string clientId, string regulationId)
        {
            return $"{clientId}_{regulationId}"; 
        }

        internal static ClientAvailableRegulationEntity Create(string clientId, string regulationId)
        {
            return new ClientAvailableRegulationEntity
            {
                RowKey = GenerateRowKey(clientId, regulationId),
                PartitionKey = GeneratePartitionKey(),
                ClientId = clientId,
                RegulationId = regulationId
            };
        }
    }
}
