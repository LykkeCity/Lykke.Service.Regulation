namespace Lykke.Service.Regulation.Core.Settings.ServiceSettings
{
    public class RegulationSettings
    {
        public DbSettings Db { get; set; }

        public RabbitMqSettings RabbitMq { get; set; }
    }
}
