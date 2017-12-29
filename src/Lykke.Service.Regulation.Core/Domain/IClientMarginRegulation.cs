namespace Lykke.Service.Regulation.Core.Domain
{
    public interface IClientMarginRegulation
    {
        string ClientId { get; }

        string RegulationId { get; }
    }
}
