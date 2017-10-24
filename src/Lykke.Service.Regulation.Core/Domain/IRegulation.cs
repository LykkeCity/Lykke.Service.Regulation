namespace Lykke.Service.Regulation.Core.Domain
{
    public interface IRegulation
    {
        string Id { get; }
        string Name { get; }
        bool RequiresKYC { get; }
    }
}
