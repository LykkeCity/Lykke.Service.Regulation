using JetBrains.Annotations;
using Lykke.Sdk.Settings;
using Lykke.Service.Regulation.Settings.Clients;
using Lykke.Service.Regulation.Settings.ServiceSettings;

namespace Lykke.Service.Regulation.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings : BaseAppSettings
    {
        public RegulationSettings RegulationService { get; set; }

        public IpGeoLocationServiceClientSettings IpGeoLocationServiceClient { get; set; }
    }
}
