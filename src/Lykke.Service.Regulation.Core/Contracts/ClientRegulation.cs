namespace Lykke.Service.Regulation.Core.Contracts
{
    public class ClientRegulation
    {
        public string RegulationId { get; set; }

        public bool Kyc { get; set; }

        public bool Active { get; set; }
    }
}
