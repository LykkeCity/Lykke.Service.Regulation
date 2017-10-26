namespace Lykke.Service.Regulation.Core.Domain
{
    public class ClientAvailableRegulation : IClientAvailableRegulation
    {
        public string ClientId { get; set; }

        public string RegulationId { get; set; }
    }
}
