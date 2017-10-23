using System.Threading.Tasks;

namespace Lykke.Service.Regulation.Core.Services
{
    public interface IShutdownManager
    {
        Task StopAsync();
    }
}