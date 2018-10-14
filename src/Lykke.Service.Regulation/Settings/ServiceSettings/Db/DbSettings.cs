using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.Regulation.Settings.ServiceSettings.Db
{
    public class DbSettings
    {
        [AzureTableCheck]
        public string LogsConnString { get; set; }

        [AzureTableCheck]
        public string DataConnString { get; set; }
    }
}
