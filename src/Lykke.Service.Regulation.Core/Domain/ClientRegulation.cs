namespace Lykke.Service.Regulation.Core.Domain
{
    public class ClientRegulation : IClientRegulation
    {
        public string ClientId { get; set; }

        public string RegulationId { get; set; }
    }
}
