namespace Lykke.Service.Regulation.Core.Domain
{
    public class Regulation : IRegulation
    {
        public string Id { get; set; }
        public bool RequiresKYC { get; set; }
    }
}
