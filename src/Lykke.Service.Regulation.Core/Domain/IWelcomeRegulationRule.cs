namespace Lykke.Service.Regulation.Core.Domain
{
    public interface IWelcomeRegulationRule
    {
        string Id { get; }
        string Country { get; }
        string RegulationId { get; }
    }
}
