namespace Lykke.Service.Regulation.Core.Settings.ServiceSettings
{
    public class RabbitMqSettings
    {
        public RegistrationQueue RegistrationQueue { get; set; }

        public RegulationExchange RegulationExchange { get; set; }
    }
}
