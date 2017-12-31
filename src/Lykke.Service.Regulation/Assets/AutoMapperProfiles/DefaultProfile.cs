using AutoMapper;
using Lykke.Service.Regulation.Core.Domain;
using Lykke.Service.Regulation.Models;

namespace Lykke.Service.Regulation.Assets.AutoMapperProfiles
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            CreateMap<RegulationModel, Core.Domain.Regulation>();
            CreateMap<NewRegulationModel, Core.Domain.Regulation>();
            CreateMap<IRegulation, RegulationModel>();
            CreateMap<ClientRegulationModel, ClientRegulation>();
            CreateMap<IWelcomeRegulationRule, WelcomeRegulationRuleModel>();
            CreateMap<WelcomeRegulationRuleModel, WelcomeRegulationRule>();
            CreateMap<NewWelcomeRegulationRuleModel, WelcomeRegulationRule>()
                .ForMember(model => model.Id, option => option.Ignore());
            CreateMap<IClientRegulation, ClientRegulationModel>();
            CreateMap<NewClientRegulationModel, ClientRegulation>()
                .ForMember(model => model.Id, option => option.Ignore())
                .ForMember(model => model.Kyc, option => option.Ignore());

            CreateMap<IMarginRegulationRule, MarginRegulationRuleModel>();
            CreateMap<MarginRegulationRuleModel, MarginRegulationRule>();
            CreateMap<NewMarginRegulationRuleModel, MarginRegulationRule>()
                .ForMember(model => model.Id, option => option.Ignore());

            CreateMap<IClientMarginRegulation, ClientRegulationModel>(MemberList.Source)
                .ForSourceMember(src => src.ClientId, opt => opt.Ignore());
        }

        public override string ProfileName => "Default profile";
    }
}
