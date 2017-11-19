namespace Lykke.Service.Regulation.RabbitSubscribers
{
    public class ClientRegisteredMessage
    {
        public string ClientId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Ip { get; set; }
    }
}
