namespace Lykke.Service.Regulation.Core.Domain
{
    public class ClientMarginRegulation : IClientMarginRegulation
    {
        public string ClientId { get; set; }
        public string RegulationId { get; set; }
    }
}
