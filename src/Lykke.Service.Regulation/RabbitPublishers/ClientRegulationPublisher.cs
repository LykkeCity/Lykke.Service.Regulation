using System.Threading.Tasks;
using Autofac;
using Common;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker.Publisher;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.Regulation.Core.Contracts;
using Lykke.Service.Regulation.Core.Services;
using Lykke.Service.Regulation.Settings.ServiceSettings.Rabbit;

namespace Lykke.Service.Regulation.RabbitPublishers
{
    public class ClientRegulationPublisher : IClientRegulationPublisher, IStartable, IStopable
    {
        private readonly ILogFactory _logFactory;
        private readonly RegulationExchange _settings;
        private RabbitMqPublisher<ClientRegulationsMessage> _publisher;

        public ClientRegulationPublisher(ILogFactory logFactory, RegulationExchange settings)
        {
            _logFactory = logFactory;
            _settings = settings;
        }

        public void Start()
        {
            var settings = RabbitMqSubscriptionSettings
                .CreateForPublisher(_settings.ConnectionString, "regulation")
                .MakeDurable();

            _publisher = new RabbitMqPublisher<ClientRegulationsMessage>(_logFactory, settings)
                .SetSerializer(new JsonMessageSerializer<ClientRegulationsMessage>())
                .SetPublishStrategy(new DefaultFanoutPublishStrategy(settings))
                .PublishSynchronously()
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
