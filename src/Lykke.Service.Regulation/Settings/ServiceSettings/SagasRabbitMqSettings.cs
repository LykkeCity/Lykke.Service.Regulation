using System;

namespace Lykke.Service.Regulation.Settings.ServiceSettings
{
    public class SagasRabbitMqSettings
    {
        public string RabbitConnectionString { get; set; }
        public TimeSpan RetryDelay { get; set; }
    }
}
