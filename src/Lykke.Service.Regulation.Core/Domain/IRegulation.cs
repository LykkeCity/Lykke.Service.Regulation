namespace Lykke.Service.Regulation.Core.Domain
{
    public interface IRegulation
    {
        string Id { get; }

        string ProfileType { get; }

        string TermsOfUseUrl { get; }

        string RiskDescriptionUrl { get; }

        string MarginTradingConditions { get; }
    }
}
