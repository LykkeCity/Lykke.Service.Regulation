using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.Regulation.Settings.ServiceSettings.Rabbit
{
    public class RegulationExchange
    {
        [AmqpCheck]
        public string ConnectionString { get; set; }
    }
}
