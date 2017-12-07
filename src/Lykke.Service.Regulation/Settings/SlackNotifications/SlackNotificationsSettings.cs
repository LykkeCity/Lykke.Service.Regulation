using Lykke.AzureQueueIntegration;

namespace Lykke.Service.Regulation.Settings.SlackNotifications
{
    public class SlackNotificationsSettings
    {
        public AzureQueueSettings AzureQueue { get; set; }
    }
}
