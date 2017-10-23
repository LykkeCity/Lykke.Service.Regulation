using Lykke.Service.Regulation.Core.Settings.ServiceSettings;
using Lykke.Service.Regulation.Core.Settings.SlackNotifications;

namespace Lykke.Service.Regulation.Core.Settings
{
    public class AppSettings
    {
        public RegulationSettings RegulationService { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }
    }
}
