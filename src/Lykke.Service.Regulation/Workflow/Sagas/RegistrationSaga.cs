using System.Threading.Tasks;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Cqrs;
using Lykke.Service.IpGeoLocation;
using Lykke.Service.IpGeoLocation.Models;
using Lykke.Service.Registration.Contract.Events;
using Lykke.Service.Regulation.Core.Exceptions;
using Lykke.Service.Regulation.Core.Services;

namespace Lykke.Service.Regulation.Workflow.Sagas
{
    public class RegistrationSaga
    {
        private readonly IClientRegulationService _clientRegulationService;
        private readonly IIpGeoLocationClient _geoLocationClient;
        private readonly ILog _log;

        public RegistrationSaga(
            IClientRegulationService clientRegulationService,
            IIpGeoLocationClient geoLocationClient,
            ILogFactory logFactory)
        {
            _clientRegulationService = clientRegulationService;
            _geoLocationClient = geoLocationClient;
            _log = logFactory.CreateLog(this);
        }

        [UsedImplicitly]
        public async Task Handle(ClientRegisteredEvent evt, ICommandSender commandSender)
        {
            try
            {
                string countryCode = null;

                if (!string.IsNullOrEmpty(evt.CountryFromPOA))
                {
                    _log.Info(nameof(ClientRegisteredEvent), $"Getting country '{evt.CountryFromPOA}' from the message", evt.ClientId);
                    countryCode = evt.CountryFromPOA;
                }
                else if (!string.IsNullOrEmpty(evt.Ip))
                {
                    _log.Info(nameof(ClientRegisteredEvent), $"Try to get country by IP address '{evt.Ip}'", evt.ClientId);
                    IIpGeolocationData data = await _geoLocationClient.GetAsync(evt.Ip);

                    countryCode = data?.CountryCode;

                    if (string.IsNullOrEmpty(countryCode))
                    {
                        _log.Warning(nameof(ClientRegisteredEvent), $"Can not find country by IP address '{evt.Ip}'.", context: evt.ClientId);
                    }
                    else
                    {
                        _log.Info(nameof(ClientRegisteredEvent), $"Country '{data.CountryCode}'.", evt.ClientId);
                    }
                }

                if (!string.IsNullOrEmpty(countryCode))
                    await _clientRegulationService.SetDefaultAsync(evt.ClientId, countryCode);
            }
            catch (ServiceException exception)
            {
                _log.Error(nameof(ClientRegisteredEvent), exception, context: evt.ClientId);
            }
        }
    }
}
