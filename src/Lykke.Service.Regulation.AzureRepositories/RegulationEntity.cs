using Lykke.Service.Regulation.Core.Domain;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Regulation.AzureRepositories
{
    public class RegulationEntity : TableEntity, IRegulation
    {
        public string Id => RowKey;

        public string ProfileType { get; set; }
    }
}
