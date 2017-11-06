using System;
using System.Collections.Generic;
using System.Linq;
using Lykke.Service.Regulation.Core.Domain;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Regulation.AzureRepositories
{
    public class WelcomeRegulationRuleEntity : TableEntity, IWelcomeRegulationRule
    {
        public string Id => RowKey;

        public string Name { get; set; }
        
        public string Country { get; set; }

        public string RegulationId { get; set; }

        public bool Active { get; set; }

        public int Priority { get; set; }

        [IgnoreProperty]
        public List<string> Countries { get; set; }

        public override void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
            base.ReadEntity(properties, operationContext);

            Countries = (Country ?? "").Split("|", StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public override IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            Country = string.Join("|", Countries);

            return base.WriteEntity(operationContext);
        }

        internal static string GeneratePartitionKey()
        {
            return "WelcomeRegulationRule";
        }

        internal static string GenerateRowKey()
        {
            return Guid.NewGuid().ToString("D");
        }

        internal void Update(IWelcomeRegulationRule welcomeRegulationRule)
        {
            Name = welcomeRegulationRule.Name;
            Countries = welcomeRegulationRule.Countries;
            RegulationId = welcomeRegulationRule.RegulationId;
            Active = welcomeRegulationRule.Active;
            Priority = welcomeRegulationRule.Priority;
        }

        internal static WelcomeRegulationRuleEntity Create(IWelcomeRegulationRule welcomeRegulationRule)
        {
            return new WelcomeRegulationRuleEntity
            {
                RowKey = GenerateRowKey(),
                PartitionKey = GeneratePartitionKey(),
                Name =welcomeRegulationRule.Name,
                Countries = welcomeRegulationRule.Countries,
                RegulationId = welcomeRegulationRule.RegulationId,
                Active = welcomeRegulationRule.Active,
                Priority = welcomeRegulationRule.Priority
            };
        }
    }
}
