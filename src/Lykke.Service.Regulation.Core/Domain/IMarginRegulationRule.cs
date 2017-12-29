using System.Collections.Generic;

namespace Lykke.Service.Regulation.Core.Domain
{
    public interface IMarginRegulationRule
    {
        string Id { get; }
        string Name { get; }
        List<string> Countries { get; }
        string RegulationId { get; }
        int Priority { get; }
    }
}
