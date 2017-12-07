using System;
using System.Threading.Tasks;
using Autofac;
using Common;
using Common.Log;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.Regulation.Core.Exceptions;
using Lykke.Service.Regulation.Core.Services;
using Lykke.Service.Regulation.Settings.ServiceSettings.Rabbit;

namespace Lykke.Service.Regulation.RabbitSubscribers
{
    public class ClientRegisteredSubscriber : IStartable, IStopable
    {
        private readonly ILog _log;
        private readonly IClientRegulationService _clientRegulationService;
        private readonly RegistrationQueue _settings;
        private RabbitMqSubscriber<ClientRegisteredMessage> _subscriber;

        public ClientRegisteredSubscriber(ILog log, IClientRegulationService clientRegulationService, RegistrationQueue settings)
        {
            _log = log;
            _clientRegulationService = clientRegulationService;
            _settings = settings;
        }

        public void Start()
        {
            var settings = RabbitMqSubscriptionSettings
                .CreateForSubscriber(_settings.ConnectionString, _settings.Exchange, "regulation")
                .MakeDurable();

            settings.DeadLetterExchangeName = null;

            _subscriber = new RabbitMqSubscriber<ClientRegisteredMessage>(settings,
                    new ResilientErrorHandlingStrategy(_log, settings,
                        TimeSpan.FromSeconds(10),
                        next: new DeadQueueErrorHandlingStrategy(_log, settings)))
                .SetMessageDeserializer(new JsonMessageDeserializer<ClientRegisteredMessage>())
                .SetMessageReadStrategy(new MessageReadQueueStrategy())
                .Subscribe(ProcessMessageAsync)
                .CreateDefaultBinding()
                .SetLogger(_log)
                .Start();
        }

        public void Dispose()
        {
            _subscriber?.Dispose();
        }

        public void Stop()
        {
            _subscriber.Stop();
        }

        private async Task ProcessMessageAsync(ClientRegisteredMessage message)
        {
            try
            {
                string countryCode = await _clientRegulationService.GetCountryCodeByPhoneAsync(message.Phone);
                await _clientRegulationService.SetDefaultAsync(message.ClientId, countryCode);
            }
            catch (ServiceException exception)
            {
                await _log.WriteWarningAsync(nameof(ClientRegisteredSubscriber), nameof(ProcessMessageAsync),
                    $"{exception.Message} {nameof(message.ClientId)}: {message.ClientId}");
            }
        }
    }
}
