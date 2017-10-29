using System;
using Lykke.Service.Regulation.Core.Domain;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Regulation.AzureRepositories
{
    public class WelcomeRegulationRuleEntity : TableEntity, IWelcomeRegulationRule
    {
        public string Id => RowKey;

        public string Country { get; set; }

        public string RegulationId { get; set; }

        internal static string GeneratePartitionKey()
        {
            return "WelcomeRegulationRule";
        }

        internal static string GenerateRowKey()
        {
            return Guid.NewGuid().ToString("D");
        }

        internal static WelcomeRegulationRuleEntity Create(string country, string regulationId)
        {
            return new WelcomeRegulationRuleEntity
            {
                RowKey = GenerateRowKey(),
                PartitionKey = GeneratePartitionKey(),
                Country = country,
                RegulationId = regulationId
            };
        }
    }
}
