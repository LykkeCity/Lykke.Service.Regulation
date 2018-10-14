using System.Threading.Tasks;
using Lykke.Common.Log;
using Lykke.Sdk;

namespace Lykke.Service.Regulation.Services
{
    // NOTE: Sometimes, shutdown process should be expressed explicitly. 
    // If this is your case, use this class to manage shutdown.
    // For example, sometimes some state should be saved only after all incoming message processing and 
    // all periodical handler was stopped, and so on.
    
    public class ShutdownManager : IShutdownManager
    {
        private readonly ILogFactory _logFactory;

        public ShutdownManager(ILogFactory logFactory)
        {
            _logFactory = logFactory;
        }

        public async Task StopAsync()
        {
            // TODO: Implement your shutdown logic here. Good idea is to log every step

            await Task.CompletedTask;
        }
    }
}
