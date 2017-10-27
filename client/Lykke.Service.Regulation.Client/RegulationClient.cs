using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.Regulation.Client.AutorestClient;
using Lykke.Service.Regulation.Client.AutorestClient.Models;
using Lykke.Service.Regulation.Client.Exceptions;
using ClientAvailableRegulationModel = Lykke.Service.Regulation.Client.Models.ClientAvailableRegulationModel;
using ClientRegulationModel = Lykke.Service.Regulation.Client.Models.ClientRegulationModel;
using RegulationModel = Lykke.Service.Regulation.Client.Models.RegulationModel;

namespace Lykke.Service.Regulation.Client
{
    /// <summary>
    /// Contains methods for work with regulation service.
    /// </summary>
    public class RegulationClient : IRegulationClient, IDisposable
    {
        private readonly ILog _log;
        private RegulationAPI _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegulationClient"/> class.
        /// </summary>
        /// <param name="serviceUrl">The regulation service url.</param>
        /// <param name="log">The logging service.</param>
        public RegulationClient(string serviceUrl, ILog log)
        {
            _log = log;
            _service = new RegulationAPI(new Uri(serviceUrl));
        }

        public void Dispose()
        {
            if (_service == null)
                return;
            _service.Dispose();
            _service = null;
        }

        /// <summary>
        /// Returns regulation by id.
        /// </summary>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns>The <see cref="RegulationModel"/>.</returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        /// <exception cref="InvalidOperationException">Thrown if an unexpected response received.</exception>
        public async Task<RegulationModel> GetRegulationByIdAsync(string regulationId)
        {
            object result = await _service.GetRegulationByIdAsync(regulationId);

            if (result is AutorestClient.Models.RegulationModel regulationModel)
                return regulationModel.ToModel();

            if (result is ErrorResponse errorResponse)
                throw new ErrorResponseException(errorResponse.ErrorMessage);

            throw new InvalidOperationException($"Unexpected response type: {result?.GetType()}");
        }

        /// <summary>
        /// Returns all regulations.
        /// </summary>
        /// <returns>The list of regulations.</returns>
        public async Task<IEnumerable<RegulationModel>> GetRegulationsAsync()
        {
            IEnumerable<AutorestClient.Models.RegulationModel> regulations = await _service.GetRegulationsAsync();

            return regulations.Select(o => o.ToModel());
        }

        /// <summary>
        /// Adds regulations.
        /// </summary>
        /// <param name="model">The model what describe a regulation.</param>
        /// <returns></returns>
        public async Task AddRegulationAsync(RegulationModel model)
        {
            await _service.AddRegulationAsync(new AutorestClient.Models.RegulationModel(model.RequiresKYC, model.Id));
        }

        /// <summary>
        /// Removes the regulation.
        /// </summary>
        /// <param name="regulationId">The regulation id for remove.</param>
        /// <returns></returns>
        public async Task RemoveRegulationAsync(string regulationId)
        {
            await _service.RemoveRegulationAsync(regulationId);
        }

        /// <summary>
        /// Updates regulation.
        /// </summary>
        /// <param name="model">The model what describe a regulation.</param>
        /// <returns></returns>
        public async Task UpdateRegulationAsync(RegulationModel model)
        {
            await _service.UpdateRegulationAsync(new AutorestClient.Models.RegulationModel(model.RequiresKYC, model.Id));
        }

        /// <summary>
        /// Returns client current regulation.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <returns>Client current regulation id.</returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        /// <exception cref="InvalidOperationException">Thrown if an unexpected response received.</exception>
        public async Task<string> GetClientRegulationAsync(string clientId)
        {
            object result = await _service.GetClientRegulationAsync(clientId);

            if (result is string regulation)
                return regulation;

            if (result is ErrorResponse errorResponse)
                throw new ErrorResponseException(errorResponse.ErrorMessage);

            throw new InvalidOperationException($"Unexpected response type: {result?.GetType()}");
        }

        /// <summary>
        /// Returns client available regulations.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <returns>The list of client available regulations.</returns>
        public Task<IList<string>> GetClientAvailableRegulationsAsync(string clientId)
        {
            return _service.GetClientAvailableRegulationsAsync(clientId);
        }

        /// <summary>
        /// Adds client available regulation.
        /// </summary>
        /// <param name="model">The model what describe a client available regulation.</param>
        /// <returns></returns>
        public Task AddClientAvailableRegulationAsync(ClientAvailableRegulationModel model)
        {
            return _service.AddClientAvailableRegulationAsync(
                new AutorestClient.Models.ClientAvailableRegulationModel(model.ClientId, model.RegulationId));
        }

        /// <summary>
        /// Sets client regulation.
        /// </summary>
        /// <param name="model">The model what describe a client regulation.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        public async Task SetClientRegulationAsync(ClientRegulationModel model)
        {
            ErrorResponse errorResponse = await _service.SetClientRegulationAsync(
                new AutorestClient.Models.ClientRegulationModel(model.ClientId, model.RegulationId));

            if (errorResponse != null)
                throw new ErrorResponseException(errorResponse.ErrorMessage);
        }

        /// <summary>
        /// Removes client regulation.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <returns></returns>
        public Task RemoveClientRegulationAsync(string clientId)
        {
            return _service.RemoveClientRegulationAsync(clientId);
        }

        /// <summary>
        /// Removes client available regulation.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        public async Task RemoveClientAvailableRegulationAsync(string clientId, string regulationId)
        {
            ErrorResponse errorResponse = await _service.RemoveClientAvailableRegulationAsync(clientId, regulationId);

            if (errorResponse != null)
                throw new ErrorResponseException(errorResponse.ErrorMessage);
        }
    }
}
