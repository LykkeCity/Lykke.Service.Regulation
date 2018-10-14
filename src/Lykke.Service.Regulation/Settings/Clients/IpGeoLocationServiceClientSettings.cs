using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.Regulation.Settings.Clients
{
    public class IpGeoLocationServiceClientSettings
    {
        [HttpCheck("/api/isalive")]
        public string ServiceUrl { get; set; }
    }
}
