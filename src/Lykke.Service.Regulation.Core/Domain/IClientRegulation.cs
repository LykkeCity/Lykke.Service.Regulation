namespace Lykke.Service.Regulation.Core.Domain
{
    public interface IClientRegulation
    {
        string ClientId { get; }

        string RegulationId { get; }
    }
}
