
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Regulation.Client.Models;

namespace Lykke.Service.Regulation.Client
{
    public interface IRegulationClient
    {
        Task<RegulationModel> GetRegulationByIdAsync(string regulationId);

        Task<IEnumerable<RegulationModel>> GetRegulationsAsync();

        Task AddRegulationAsync(RegulationModel model);

        Task RemoveRegulationAsync(string regulationId);

        Task UpdateRegulationAsync(RegulationModel model);

        Task<string> GetClientRegulationAsync(string clientId);

        Task<IList<string>> GetClientAvailableRegulationsAsync(string clientId);

        Task AddClientAvailableRegulationAsync(ClientAvailableRegulationModel model);

        Task SetClientRegulationAsync(ClientRegulationModel model);

        Task RemoveClientRegulationAsync(string clientId);

        Task RemoveClientAvailableRegulationAsync(string clientId, string regulationId);
    }
}
