namespace Lykke.Service.Regulation.Core.Domain
{
    public interface IRegulation
    {
        string Id { get; }

        bool RequiresKYC { get; }
    }
}
