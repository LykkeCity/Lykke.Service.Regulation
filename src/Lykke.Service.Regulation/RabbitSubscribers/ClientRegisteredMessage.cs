namespace Lykke.Service.Regulation.RabbitSubscribers
{
    public class ClientRegisteredMessage
    {
        public string ClientId { get; set; }
        public string Ip { get; set; }
    }
}
