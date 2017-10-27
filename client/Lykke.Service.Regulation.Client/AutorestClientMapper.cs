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
                RequiresKYC = model.RequiresKYC
            };
        }

        public static ClientRegulationModel ToModel(this AutorestClient.Models.ClientRegulationModel model)
        {
            return new ClientRegulationModel
            {
                ClientId = model.ClientId,
                RegulationId = model.RegulationId
            };
        }

        public static ClientAvailableRegulationModel ToModel(this AutorestClient.Models.ClientAvailableRegulationModel model)
        {
            return new ClientAvailableRegulationModel
            {
                ClientId = model.ClientId,
                RegulationId = model.RegulationId
            };
        }
    }
}
