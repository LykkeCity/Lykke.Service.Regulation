using System;
using Common.Log;

namespace Lykke.Service.Regulation.Client
{
    public class RegulationClient : IRegulationClient, IDisposable
    {
        private readonly ILog _log;

        public RegulationClient(string serviceUrl, ILog log)
        {
            _log = log;
        }

        public void Dispose()
        {
            //if (_service == null)
            //    return;
            //_service.Dispose();
            //_service = null;
        }
    }
}
