using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.Regulation.Settings.ServiceSettings.Rabbit
{
    public class RegistrationQueue
    {
        [AmqpCheck]
        public string ConnectionString { get; set; }

        public string Exchange { get; set; }
    }
}
