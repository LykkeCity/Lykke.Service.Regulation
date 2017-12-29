using System.Collections.Generic;

namespace Lykke.Service.Regulation.Core.Domain
{
    public class MarginRegulationRule : IMarginRegulationRule
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Countries { get; set; }
        public string RegulationId { get; set; }
        public int Priority { get; set; }
    }
}
