using Lykke.Service.Regulation.Settings.ServiceSettings.Db;
using Lykke.Service.Regulation.Settings.ServiceSettings.Rabbit;

namespace Lykke.Service.Regulation.Settings.ServiceSettings
{
    public class RegulationSettings
    {
        public DbSettings Db { get; set; }

        public RabbitMqSettings RabbitMq { get; set; }
    }
}
