using System;
using System.Threading.Tasks;
using Autofac;
using Common;
using Common.Log;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.IpGeoLocation;
using Lykke.Service.IpGeoLocation.Models;
using Lykke.Service.Registration.Contract.Events;
using Lykke.Service.Regulation.Core.Exceptions;
using Lykke.Service.Regulation.Core.Services;
using Lykke.Service.Regulation.Settings.ServiceSettings.Rabbit;

namespace Lykke.Service.Regulation.RabbitSubscribers
{
    public class ClientRegisteredSubscriber : IStartable, IStopable
    {
        private readonly ILog _log;
        private readonly ILogFactory _logFactory;
        private readonly IClientRegulationService _clientRegulationService;
        private readonly IIpGeoLocationClient _geoLocationClient;
        private readonly RegistrationQueue _settings;
        private RabbitMqSubscriber<ClientRegisteredEvent> _subscriber;

        public ClientRegisteredSubscriber(
            ILogFactory logFactory,
            IClientRegulationService clientRegulationService,
            IIpGeoLocationClient geoLocationClient,
            RegistrationQueue settings)
        {
            _logFactory = logFactory;
            _log = logFactory.CreateLog(this);
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

            _subscriber = new RabbitMqSubscriber<ClientRegisteredEvent>(_logFactory, settings,
                    new ResilientErrorHandlingStrategy(_logFactory, settings, TimeSpan.FromSeconds(10)))
                .SetMessageDeserializer(new JsonMessageDeserializer<ClientRegisteredEvent>())
                .SetMessageReadStrategy(new MessageReadQueueStrategy())
                .Subscribe(ProcessMessageAsync)
                .CreateDefaultBinding()
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

        private async Task ProcessMessageAsync(ClientRegisteredEvent message)
        {
            try
            {
                string countryCode = null;

                if (!string.IsNullOrEmpty(message.CountryFromPOA))
                {
                    _log.Info(nameof(ProcessMessageAsync), $"Getting country '{message.CountryFromPOA}' from the message", message.ClientId);
                    countryCode = message.CountryFromPOA;
                }
                else if (!string.IsNullOrEmpty(message.Ip))
                {
                    _log.Info(nameof(ProcessMessageAsync), $"Try to get country by IP address '{message.Ip}'", message.ClientId);
                    IIpGeolocationData data = await _geoLocationClient.GetAsync(message.Ip);

                    countryCode = data?.CountryCode;

                    if (string.IsNullOrEmpty(countryCode))
                    {
                        _log.Warning(nameof(ProcessMessageAsync), $"Can not find country by IP address '{message.Ip}'.", context: message.ClientId);
                    }
                    else
                    {
                        _log.Info(nameof(ProcessMessageAsync), $"Country '{data.CountryCode}'.", message.ClientId);
                    }
                }

                if (!string.IsNullOrEmpty(countryCode))
                    await _clientRegulationService.SetDefaultAsync(message.ClientId, countryCode);
            }
            catch (ServiceException exception)
            {
                _log.Error(nameof(ProcessMessageAsync), exception, context: message.ClientId);
            }
        }
    }
}
