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
            CreateMap<IRegulation, RegulationModel>();
            CreateMap<ClientRegulationModel, ClientRegulation>();
            CreateMap<ClientAvailableRegulationModel, ClientAvailableRegulation>();
        }

        public override string ProfileName => "Default profile";
    }
}
