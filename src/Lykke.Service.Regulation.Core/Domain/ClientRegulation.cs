namespace Lykke.Service.Regulation.Core.Domain
{
    public class ClientRegulation : IClientRegulation
    {
        public string Id { get; set; }

        public string ClientId { get; set; }

        public string RegulationId { get; set; }

        public bool Kyc { get; set; }

        public bool Active { get; set; }
    }
}
