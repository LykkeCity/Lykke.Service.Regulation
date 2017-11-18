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
    }
}
