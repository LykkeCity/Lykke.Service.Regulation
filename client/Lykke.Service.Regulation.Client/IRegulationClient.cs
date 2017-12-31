using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Regulation.Client.Exceptions;
using Lykke.Service.Regulation.Client.Models;

namespace Lykke.Service.Regulation.Client
{
    public interface IRegulationClient
    {
        void Dispose();

        /// <summary>
        /// Returns regulation details by specified id.
        /// </summary>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns>The <see cref="RegulationModel"/>.</returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        /// <exception cref="InvalidOperationException">Thrown if an unexpected response received.</exception>
        Task<RegulationModel> GetRegulationAsync(string regulationId);

        /// <summary>
        /// Returns all regulations.
        /// </summary>
        /// <returns>The list of regulations.</returns>
        Task<IEnumerable<RegulationModel>> GetRegulationsAsync();

        /// <summary>
        /// Adds the regulation.
        /// </summary>
        /// <param name="model">The model what describe a regulation.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        Task AddRegulationAsync(RegulationModel model);

        /// <summary>
        /// Deletes the regulation by specified id.
        /// </summary>
        /// <param name="regulationId">The id of regulation to delete.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        Task DeleteRegulationAsync(string regulationId);

        /// <summary>
        /// Returns welcome regulation rule by specified id.
        /// </summary>
        /// <param name="welcomeRegulationRuleId">The regulation rule id.</param>
        /// <returns>The <see cref="WelcomeRegulationRuleModel"/>.</returns>
        Task<WelcomeRegulationRuleModel> GetWelcomeRegulationRuleByIdAsync(string welcomeRegulationRuleId);

        /// <summary>
        /// Returns all welcome regulation rules.
        /// </summary>
        /// <returns>The list of welcome regulation rules.</returns>
        Task<IEnumerable<WelcomeRegulationRuleModel>> GetWelcomeRegulationRulesAsync();

        /// <summary>
        /// Returns all welcome regulation rules associated with specified country.
        /// </summary>
        /// <param name="country">The country name.</param>
        /// <returns>The list of welcome regulation rules.</returns>
        Task<IEnumerable<WelcomeRegulationRuleModel>> GetWelcomeRegulationRulesByCountryAsync(string country);

        /// <summary>
        /// Returns all welcome regulation rules associated with specified regulation.
        /// </summary>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns>The list of welcome regulation rules.</returns>
        Task<IEnumerable<WelcomeRegulationRuleModel>> GetWelcomeRegulationRulesByRegulationIdAsync(string regulationId);

        /// <summary>
        /// Adds the welcome regulation rule.
        /// </summary>
        /// <param name="model">The model what describe a welcome regulation rule.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        Task AddWelcomeRegulationRuleAsync(WelcomeRegulationRuleModel model);

        /// <summary>
        /// Updates welcome regulation rule.
        /// </summary>
        /// <param name="model">The model what describe a welcome regulation rule.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        Task UpdateWelcomeRegulationRuleAsync(WelcomeRegulationRuleModel model);

        /// <summary>
        /// Deletes the welcome regulation rule by specified regulation id.
        /// </summary>
        /// <param name="regulationRuleId">The welcome regulation rule id.</param>
        /// <returns></returns>
        Task DeleteWelcomeRegulationRuleAsync(string regulationRuleId);

        /// <summary>
        /// Returns a client regulation by specified client id and regulation id.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns>Client regulation.</returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        /// <exception cref="InvalidOperationException">Thrown if an unexpected response received.</exception>
        Task<ClientRegulationModel> GetClientRegulationAsync(string clientId, string regulationId);

        /// <summary>
        /// Returns a regulations associated with client.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <returns>The list of client regulations.</returns>
        Task<IEnumerable<ClientRegulationModel>> GetClientRegulationsByClientIdAsync(string clientId);

        /// <summary>
        /// Returns a regulations associated with regulation.
        /// </summary>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns>The list of client regulations.</returns>
        Task<IEnumerable<ClientRegulationModel>> GetClientRegulationsByRegulationIdAsync(string regulationId);

        /// <summary>
        /// Returns an active client regulations associated with client.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <returns>The list of client regulations.</returns>
        Task<IEnumerable<ClientRegulationModel>> GetActiveClientRegulationsByClientIdAsync(string clientId);

        /// <summary>
        /// Returns an active and KYC client regulations associated with client.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <returns>The list of client regulations.</returns>
        Task<IEnumerable<ClientRegulationModel>> GetAvailableClientRegulationsByClientIdAsync(string clientId);

        /// <summary>
        /// Adds the client regulation.
        /// </summary>
        /// <param name="model">The model what describe a client regulation.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        Task AddClientRegulationAsync(ClientRegulationModel model);

        /// <summary>
        /// Removes existing client regulations and adds new one.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        Task SetClientRegulationAsync(string clientId, string regulationId);

        /// <summary>
        /// Adds default regulations to client associated with country.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="country">The country name.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        Task SetDefaultClientRegulationsAsync(string clientId, string country);

        /// <summary>
        /// Sets the client regulation KYC status to <c>true</c>.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="regulationId">The regulation id.</param>
        /// <param name="active">The client regulation KYC status.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        Task UpdateClientRegulationKycAsync(string clientId, string regulationId, bool active);

        /// <summary>
        /// Sets the client regulation active status to <c>true</c>.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        Task ActivateClientRegulationAsync(string clientId, string regulationId);

        /// <summary>
        /// Sets the client regulation active status to <c>false</c>.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        Task DeactivateClientRegulationAsync(string clientId, string regulationId);

        /// <summary>
        /// Deletes the regulation associated with client by specified id.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        Task DeleteClientRegulationAsync(string clientId, string regulationId);

        /// <summary>
        /// Returns margin regulation rule by specified id.
        /// </summary>
        /// <param name="id">The margin regulation rule id.</param>
        /// <returns>The <see cref="MarginRegulationRuleModel"/>.</returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        /// <exception cref="InvalidOperationException">Thrown if an unexpected response received.</exception>
        Task<MarginRegulationRuleModel> GetMarginRegulationRuleByIdAsync(string id);

        /// <summary>
        /// Returns all margin regulation rules.
        /// </summary>
        /// <returns>The collection of margin regulation rules</returns>
        Task<IEnumerable<MarginRegulationRuleModel>> GetMarginRegulationRulesAsync();

        /// <summary>
        /// Adds a margin regulation rule.
        /// </summary>
        /// <param name="model">The model what describe a margin regulation rule.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        Task AddMarginRegulationRuleAsync(MarginRegulationRuleModel model);

        /// <summary>
        /// Updates a margin regulation rule.
        /// </summary>
        /// <param name="model">The model what describes a margin regulation rule.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        Task UpdateMarginRegulationRuleAsync(MarginRegulationRuleModel model);

        /// <summary>
        /// Deletes the margin regulation rule by specified id.
        /// </summary>
        /// <param name="id">The margin regulation rule id.</param>
        /// <returns></returns>
        Task DeleteMarginRegulationRuleAsync(string id);

        /// <summary>
        /// Returns a client margin regulation by specified client id.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <returns>Client margin regulation id.</returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        /// <exception cref="InvalidOperationException">Thrown if an unexpected response received.</exception>
        Task<string> GetClientMarginRegulationAsync(string clientId);

        /// <summary>
        /// Adds the client margin regulation.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        Task AddClientMarginRegulationAsync(string clientId, string regulationId);

        /// <summary>
        /// Removes existing client margin regulation and adds new one.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        Task SetClientMarginRegulationAsync(string clientId, string regulationId);

        /// <summary>
        /// Deletes a client margin regulation.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <returns></returns>
        Task DeleteClientmarginRegulationAsync(string clientId);
    }
}
