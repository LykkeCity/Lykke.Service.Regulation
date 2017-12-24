using System;
using System.Threading.Tasks;
using Autofac;
using Common;
using Common.Log;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.IpGeoLocation;
using Lykke.Service.IpGeoLocation.Models;
using Lykke.Service.Regulation.Core.Exceptions;
using Lykke.Service.Regulation.Core.Services;
using Lykke.Service.Regulation.Settings.ServiceSettings.Rabbit;

namespace Lykke.Service.Regulation.RabbitSubscribers
{
    public class ClientRegisteredSubscriber : IStartable, IStopable
    {
        private readonly ILog _log;
        private readonly IClientRegulationService _clientRegulationService;
        private readonly IIpGeoLocationClient _geoLocationClient;
        private readonly RegistrationQueue _settings;
        private RabbitMqSubscriber<ClientRegisteredMessage> _subscriber;

        public ClientRegisteredSubscriber(
            ILog log,
            IClientRegulationService clientRegulationService,
            IIpGeoLocationClient geoLocationClient,
            RegistrationQueue settings)
        {
            _log = log;
            _clientRegulationService = clientRegulationService;
            _geoLocationClient = geoLocationClient;
            _settings = settings;
        }

        public void Start()
        {
            var settings = RabbitMqSubscriptionSettings
                .CreateForSubscriber(_settings.ConnectionString, _settings.Exchange, "regulation")
                .MakeDurable();

            settings.DeadLetterExchangeName = null;

            _subscriber = new RabbitMqSubscriber<ClientRegisteredMessage>(settings,
                    new ResilientErrorHandlingStrategy(_log, settings, TimeSpan.FromSeconds(10)))
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
                string countryCode = null;

                if (string.IsNullOrEmpty(message.Ip))
                {
                    await _log.WriteWarningAsync(nameof(ClientRegisteredSubscriber), message.ClientId, "No IP address.");
                }
                else
                {
                    IIpGeolocationData data = await _geoLocationClient.GetAsync(message.Ip);

                    countryCode = data?.CountryCode;

                    if (string.IsNullOrEmpty(countryCode))
                    {
                        await _log.WriteWarningAsync(nameof(ClientRegisteredSubscriber), message.ClientId,
                            $"Can not find country by IP address '{message.Ip}'.");
                    }
                    else
                    {
                        await _log.WriteInfoAsync(nameof(ClientRegisteredSubscriber), message.ClientId,
                            $"Country '{data.CountryCode}'.");
                    }
                }

                await _clientRegulationService.SetDefaultAsync(message.ClientId, countryCode);
            }
            catch (ServiceException exception)
            {
                await _log.WriteErrorAsync(nameof(ClientRegisteredSubscriber), message.ClientId, exception);
            }
        }
    }
}
