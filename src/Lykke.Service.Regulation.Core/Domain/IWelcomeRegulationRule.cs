using System.Collections.Generic;

namespace Lykke.Service.Regulation.Core.Domain
{
    public interface IWelcomeRegulationRule
    {
        string Id { get; }
        string Name { get; }
        List<string> Countries { get; }
        string RegulationId { get; }
        bool Active { get; }
        int Priority { get; }
    }
}
