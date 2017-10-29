namespace Lykke.Service.Regulation.Core.Domain
{
    public class WelcomeRegulationRule : IWelcomeRegulationRule
    {
        public string Id { get; set; }
        public string Country { get; set; }
        public string RegulationId { get; set; }
    }
}
