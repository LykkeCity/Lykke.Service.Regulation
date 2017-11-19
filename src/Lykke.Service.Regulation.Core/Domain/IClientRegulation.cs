namespace Lykke.Service.Regulation.Core.Domain
{
    public interface IClientRegulation
    {
        string Id { get; }

        string ClientId { get; }

        string RegulationId { get; }

        bool Kyc { get; set; }

        bool Active { get; set; }
    }
}
