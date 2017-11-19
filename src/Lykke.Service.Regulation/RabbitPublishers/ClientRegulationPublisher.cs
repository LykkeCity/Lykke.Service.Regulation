using System.Threading.Tasks;
using Autofac;
using Common;
using Common.Log;
using Lykke.RabbitMqBroker.Publisher;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.Regulation.Core.Contracts;
using Lykke.Service.Regulation.Core.Services;
using Lykke.Service.Regulation.Core.Settings.ServiceSettings;

namespace Lykke.Service.Regulation.RabbitPublishers
{
    public class ClientRegulationPublisher : IClientRegulationPublisher, IStartable, IStopable
    {
        private readonly ILog _log;
        private readonly RegulationExchange _settings;
        private RabbitMqPublisher<ClientRegulationsMessage> _publisher;

        public ClientRegulationPublisher(ILog log, RegulationExchange settings)
        {
            _log = log;
            _settings = settings;
        }

        public void Start()
        {
            var settings = RabbitMqSubscriptionSettings
                .CreateForPublisher(_settings.ConnectionString, "regulation")
                .MakeDurable();

            _publisher = new RabbitMqPublisher<ClientRegulationsMessage>(settings)
                .SetSerializer(new JsonMessageSerializer<ClientRegulationsMessage>())
                .SetPublishStrategy(new DefaultFanoutPublishStrategy(settings))
                .PublishSynchronously()
                .SetLogger(_log)
                .Start();
        }

        public void Dispose()
        {
            _publisher?.Dispose();
        }

        public void Stop()
        {
            _publisher?.Stop();
        }

        public async Task PublishAsync(ClientRegulationsMessage message)
        {
            await _publisher.ProduceAsync(message);
        }
    }
}
