using System.Threading.Tasks;
using Lykke.Service.Regulation.Core.Domain;

namespace Lykke.Service.Regulation.Core.Repositories
{
    public interface IClientMarginRegulationRepository
    {
        Task<IClientMarginRegulation> GetAsync(string clientId);

        Task InsertAsync(IClientMarginRegulation clientMarginRegulation);

        Task DeleteAsync(string clientId);
    }
}
