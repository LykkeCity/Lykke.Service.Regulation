using Lykke.Service.Regulation.Settings.Clients;
using Lykke.Service.Regulation.Settings.ServiceSettings;
using Lykke.Service.Regulation.Settings.SlackNotifications;

namespace Lykke.Service.Regulation.Settings
{
    public class AppSettings
    {
        public RegulationSettings RegulationService { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }
        public IpGeoLocationServiceClientSettings IpGeoLocationServiceClient { get; set; }
    }
}
