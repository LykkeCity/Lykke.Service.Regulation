namespace Lykke.Service.Regulation.Core.Domain
{
    public interface IClientAvailableRegulation
    {
        string ClientId { get; }

        string RegulationId { get; }
    }
}
