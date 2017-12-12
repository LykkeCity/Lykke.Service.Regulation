using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.Regulation.Client.AutorestClient;
using Lykke.Service.Regulation.Client.AutorestClient.Models;
using Lykke.Service.Regulation.Client.Exceptions;
using WelcomeRegulationRuleModel = Lykke.Service.Regulation.Client.Models.WelcomeRegulationRuleModel;
using ClientRegulationModel = Lykke.Service.Regulation.Client.Models.ClientRegulationModel;
using RegulationModel = Lykke.Service.Regulation.Client.Models.RegulationModel;

namespace Lykke.Service.Regulation.Client
{
    /// <summary>
    /// Contains methods for work with regulation service.
    /// </summary>
    public class RegulationClient : IDisposable, IRegulationClient
    {
        private RegulationAPI _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegulationClient"/> class.
        /// </summary>
        /// <param name="serviceUrl">The regulation service url.</param>
        public RegulationClient(string serviceUrl)
        {
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
        /// Returns regulation details by specified id.
        /// </summary>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns>The <see cref="RegulationModel"/>.</returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        /// <exception cref="InvalidOperationException">Thrown if an unexpected response received.</exception>
        public async Task<RegulationModel> GetRegulationAsync(string regulationId)
        {
            object result = await _service.GetRegulationAsync(regulationId);

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
        /// Adds the regulation.
        /// </summary>
        /// <param name="model">The model that describe a regulation.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        public async Task AddRegulationAsync(RegulationModel model)
        {
            ErrorResponse errorResponse =
                await _service.AddRegulationAsync(new NewRegulationModel(model.Id, model.ProfileType));

            if(errorResponse != null)
                throw new ErrorResponseException(errorResponse.ErrorMessage);
        }

        /// <summary>
        /// Updates the regulation.
        /// </summary>
        /// <param name="model">The model that describe a regulation.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        public async Task UpdateRegulationAsync(RegulationModel model)
        {
            ErrorResponse errorResponse =
                await _service.UpdateRegulationAsync(new NewRegulationModel(model.Id, model.ProfileType));

            if (errorResponse != null)
                throw new ErrorResponseException(errorResponse.ErrorMessage);
        }

        /// <summary>
        /// Deletes the regulation by specified id.
        /// </summary>
        /// <param name="regulationId">The id of regulation to delete.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        public async Task DeleteRegulationAsync(string regulationId)
        {
            ErrorResponse errorResponse = await _service.DeleteRegulationAsync(regulationId);

            if (errorResponse != null)
                throw new ErrorResponseException(errorResponse.ErrorMessage);
        }

        /// <summary>
        /// Returns welcome regulation rule by specified id.
        /// </summary>
        /// <param name="welcomeRegulationRuleId">The regulation rule id.</param>
        /// <returns>The <see cref="WelcomeRegulationRuleModel"/>.</returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        /// <exception cref="InvalidOperationException">Thrown if an unexpected response received.</exception>
        public async Task<WelcomeRegulationRuleModel> GetWelcomeRegulationRuleByIdAsync(string welcomeRegulationRuleId)
        {
            object result = await _service.GetWelcomeRegulationRuleByIdAsync(welcomeRegulationRuleId);

            if (result is AutorestClient.Models.WelcomeRegulationRuleModel regulationModel)
                return regulationModel.ToModel();

            if (result is ErrorResponse errorResponse)
                throw new ErrorResponseException(errorResponse.ErrorMessage);

            throw new InvalidOperationException($"Unexpected response type: {result?.GetType()}");
        }

        /// <summary>
        /// Returns all welcome regulation rules.
        /// </summary>
        /// <returns>The list of welcome regulation rules.</returns>
        public async Task<IEnumerable<WelcomeRegulationRuleModel>> GetWelcomeRegulationRulesAsync()
        {
            IEnumerable<AutorestClient.Models.WelcomeRegulationRuleModel> regulations = 
                await _service.GetWelcomeRegulationRulesAsync();

            return regulations.Select(o => o.ToModel());
        }

        /// <summary>
        /// Returns all welcome regulation rules associated with specified country.
        /// </summary>
        /// <param name="country">The country name.</param>
        /// <returns>The list of welcome regulation rules.</returns>
        public async Task<IEnumerable<WelcomeRegulationRuleModel>> GetWelcomeRegulationRulesByCountryAsync(string country)
        {
            IEnumerable<AutorestClient.Models.WelcomeRegulationRuleModel> regulations =
                await _service.GetWelcomeRegulationRulesByCountryAsync(country);

            return regulations.Select(o => o.ToModel());
        }

        /// <summary>
        /// Returns all welcome regulation rules associated with specified regulation.
        /// </summary>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns>The list of welcome regulation rules.</returns>
        public async Task<IEnumerable<WelcomeRegulationRuleModel>> GetWelcomeRegulationRulesByRegulationIdAsync(string regulationId)
        {
            object result = await _service.GetWelcomeRegulationRulesByRegulationIdAsync(regulationId);

            if (result is IEnumerable<AutorestClient.Models.WelcomeRegulationRuleModel> regulations)
                return regulations.Select(o => o.ToModel());

            if (result is ErrorResponse errorResponse)
                throw new ErrorResponseException(errorResponse.ErrorMessage);

            throw new InvalidOperationException($"Unexpected response type: {result?.GetType()}");
        }

        /// <summary>
        /// Adds the welcome regulation rule.
        /// </summary>
        /// <param name="model">The model that describe a welcome regulation rule.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        public async Task AddWelcomeRegulationRuleAsync(WelcomeRegulationRuleModel model)
        {
            ErrorResponse errorResponse =
                await _service.AddWelcomeRegulationRuleAsync(
                    new NewWelcomeRegulationRuleModel
                    {
                        Name = model.Name,
                        RegulationId = model.RegulationId,
                        Countries = model.Countries,
                        Active = model.Active,
                        Priority = model.Priority
                    });

            if (errorResponse != null)
                throw new ErrorResponseException(errorResponse.ErrorMessage);
        }

        /// <summary>
        /// Updates welcome regulation rule.
        /// </summary>
        /// <param name="model">The model that describe a welcome regulation rule.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        public async Task UpdateWelcomeRegulationRuleAsync(WelcomeRegulationRuleModel model)
        {
            ErrorResponse errorResponse =
                await _service.UpdateWelcomeRegulationRuleAsync(new AutorestClient.Models.WelcomeRegulationRuleModel
                {
                    Id = model.Id,
                    Name = model.Name,
                    RegulationId = model.RegulationId,
                    Countries = model.Countries,
                    Active = model.Active,
                    Priority = model.Priority
                });

            if (errorResponse != null)
                throw new ErrorResponseException(errorResponse.ErrorMessage);
        }

        /// <summary>
        /// Deletes the welcome regulation rule by specified regulation id.
        /// </summary>
        /// <param name="regulationRuleId">The welcome regulation rule id.</param>
        /// <returns></returns>
        public async Task DeleteWelcomeRegulationRuleAsync(string regulationRuleId)
        {
            ErrorResponse errorResponse =
                await _service.DeleteWelcomeRegulationRuleAsync(regulationRuleId);

            if (errorResponse != null)
                throw new ErrorResponseException(errorResponse.ErrorMessage);
        }

        /// <summary>
        /// Returns a client regulation by specified client id and regulation id.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns>Client regulation.</returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        /// <exception cref="InvalidOperationException">Thrown if an unexpected response received.</exception>
        public async Task<ClientRegulationModel> GetClientRegulationAsync(string clientId, string regulationId)
        {
            object result = await _service.GetClientRegulationAsync(clientId, regulationId);

            if (result is AutorestClient.Models.ClientRegulationModel regulation)
                return regulation.ToModel();

            if (result is ErrorResponse errorResponse)
                throw new ErrorResponseException(errorResponse.ErrorMessage);

            throw new InvalidOperationException($"Unexpected response type: {result?.GetType()}");
        }

        /// <summary>
        /// Returns a regulations associated with client.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <returns>The list of client regulations.</returns>
        public async Task<IEnumerable<ClientRegulationModel>> GetClientRegulationsByClientIdAsync(string clientId)
        {
            IEnumerable<AutorestClient.Models.ClientRegulationModel> regulations
                = await _service.GetClientRegulationsByClientIdAsync(clientId);

            return regulations.Select(o => o.ToModel());
        }

        /// <summary>
        /// Returns a regulations associated with regulation.
        /// </summary>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns>The list of client regulations.</returns>
        public async Task<IEnumerable<ClientRegulationModel>> GetClientRegulationsByRegulationIdAsync(string regulationId)
        {
            object result = await _service.GetClientRegulationsByRegulationIdAsync(regulationId);

            if (result is IEnumerable<AutorestClient.Models.ClientRegulationModel> regulations)
                return regulations.Select(o => o.ToModel());

            if (result is ErrorResponse errorResponse)
                throw new ErrorResponseException(errorResponse.ErrorMessage);

            throw new InvalidOperationException($"Unexpected response type: {result?.GetType()}");
        }

        /// <summary>
        /// Returns an active client regulations associated with client.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <returns>The list of client regulations.</returns>
        public async Task<IEnumerable<ClientRegulationModel>> GetActiveClientRegulationsByClientIdAsync(string clientId)
        {
            IEnumerable<AutorestClient.Models.ClientRegulationModel> regulations
                = await _service.GetActiveClientRegulationsByClientIdAsync(clientId);

            return regulations.Select(o => o.ToModel());
        }

        /// <summary>
        /// Returns an active and KYC client regulations associated with client.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <returns>The list of client regulations.</returns>
        public async Task<IEnumerable<ClientRegulationModel>> GetAvailableClientRegulationsByClientIdAsync(string clientId)
        {
            IEnumerable<AutorestClient.Models.ClientRegulationModel> regulations
                = await _service.GetAvailableClientRegulationsByClientIdAsync(clientId);

            return regulations.Select(o => o.ToModel());
        }

        /// <summary>
        /// Adds the client regulation.
        /// </summary>
        /// <param name="model">The model that describe a client regulation.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        public async Task AddClientRegulationAsync(ClientRegulationModel model)
        {
            ErrorResponse errorResponse =
                await _service.AddClientRegulationAsync(
                    new NewClientRegulationModel
                    {
                        ClientId = model.ClientId,
                        RegulationId = model.RegulationId,
                        Active = model.Active
                    });

            if (errorResponse != null)
                throw new ErrorResponseException(errorResponse.ErrorMessage);
        }

        /// <summary>
        /// Adds default regulations to client associated with country.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="country">The country name.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        public async Task SetDefaultClientRegulationsAsync(string clientId, string country)
        {
            ErrorResponse errorResponse =
                await _service.SetDefaultClientRegulationsAsync(clientId, country);

            if (errorResponse != null)
                throw new ErrorResponseException(errorResponse.ErrorMessage);
        }

        /// <summary>
        /// Sets the client regulation KYC status to <c>true</c>.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="regulationId">The regulation id.</param>
        /// <param name="active">The client regulation KYC status.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        public async Task UpdateClientRegulationKycAsync(string clientId, string regulationId, bool active)
        {
            ErrorResponse errorResponse =
                await _service.UpdateClientRegulationKycAsync(clientId, regulationId, active);

            if (errorResponse != null)
                throw new ErrorResponseException(errorResponse.ErrorMessage);
        }

        /// <summary>
        /// Sets the client regulation active status to <c>true</c>.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        public async Task ActivateClientRegulationAsync(string clientId, string regulationId)
        {
            ErrorResponse errorResponse =
                await _service.ActivateClientRegulationAsync(clientId, regulationId);

            if (errorResponse != null)
                throw new ErrorResponseException(errorResponse.ErrorMessage);
        }

        /// <summary>
        /// Sets the client regulation active status to <c>false</c>.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        public async Task DeactivateClientRegulationAsync(string clientId, string regulationId)
        {
            ErrorResponse errorResponse =
                await _service.DeactivateClientRegulationAsync(clientId, regulationId);

            if (errorResponse != null)
                throw new ErrorResponseException(errorResponse.ErrorMessage);
        }

        /// <summary>
        /// Deletes the regulation associated with client by specified id.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        public async Task DeleteClientRegulationAsync(string clientId, string regulationId)
        {
            ErrorResponse errorResponse = await _service.DeleteClientRegulationAsync(clientId, regulationId);

            if (errorResponse != null)
                throw new ErrorResponseException(errorResponse.ErrorMessage);
        }
    }
}
