using System;
using Lykke.Service.Regulation.Core.Domain;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Regulation.AzureRepositories
{
    public class ClientRegulationEntity : TableEntity, IClientRegulation
    {
        public string Id => RowKey;

        public string ClientId { get; set; }

        public string RegulationId { get; set; }

        public bool Kyc { get; set; }

        public bool Active { get; set; }

        internal static string GeneratePartitionKey()
        {
            return "ClientRegulation";
        }

        internal static string GenerateRowKey(string regulationId, string clientId)
        {
            return $"{clientId}_{regulationId}";
        }

        internal static ClientRegulationEntity Create(string clientId, string regulationId)
        {
            return new ClientRegulationEntity
            {
                RowKey = GenerateRowKey(clientId, regulationId),
                PartitionKey = GeneratePartitionKey(),
                ClientId = clientId,
                RegulationId = regulationId,
                Kyc = false,
                Active = false
            };
        }
    }
}
