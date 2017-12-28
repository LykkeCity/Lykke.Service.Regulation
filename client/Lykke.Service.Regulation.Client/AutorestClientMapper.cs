using Lykke.Service.Regulation.Client.Models;

namespace Lykke.Service.Regulation.Client
{
    public static class AutorestClientMapper
    {
        public static RegulationModel ToModel(this AutorestClient.Models.RegulationModel model)
        {
            return new RegulationModel
            {
                Id = model.Id,
                ProfileType = model.ProfileType,
                TermsOfUseUrl = model.TermsOfUseUrl,
                RiskDescriptionUrl = model.RiskDescriptionUrl,
                MarginTradingConditions = model.MarginTradingConditions
            };
        }

        public static ClientRegulationModel ToModel(this AutorestClient.Models.ClientRegulationModel model)
        {
            return new ClientRegulationModel
            {
                Id = model.Id,
                ClientId = model.ClientId,
                RegulationId = model.RegulationId,
                Kyc = model.Kyc ?? false,
                Active = model.Active ?? false
            };
        }

        public static WelcomeRegulationRuleModel ToModel(this AutorestClient.Models.WelcomeRegulationRuleModel model)
        {
            return new WelcomeRegulationRuleModel
            {
                Id = model.Id,
                Name = model.Name,
                RegulationId = model.RegulationId,
                Countries = model.Countries,
                Active = model.Active,
                Priority = model.Priority
            };
        }
    }
}
