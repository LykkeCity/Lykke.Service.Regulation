using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Regulation.Client.Exceptions;
using Lykke.Service.Regulation.Client.Models;

namespace Lykke.Service.Regulation.Client
{
    public interface IRegulationClient
    {
        /// <summary>
        /// Returns regulation.
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
        /// Adds regulations.
        /// </summary>
        /// <param name="model">The model what describe a regulation.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        Task AddRegulationAsync(RegulationModel model);

        /// <summary>
        /// Removes regulation.
        /// </summary>
        /// <param name="regulationId">The regulation id for remove.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        Task DeleteRegulationAsync(string regulationId);

        /// <summary>
        /// Returns all welcome regulation rules.
        /// </summary>
        /// <returns>The list of welcome regulation rules.</returns>
        Task<IEnumerable<WelcomeRegulationRuleModel>> GetWelcomeRegulationRulesAsync();

        /// <summary>
        /// Returns all welcome regulation rules by country.
        /// </summary>
        /// <param name="country">The country name.</param>
        /// <returns>The list of welcome regulation rules.</returns>
        Task<IEnumerable<WelcomeRegulationRuleModel>> GetWelcomeRegulationRulesByCountryAsync(string country);

        /// <summary>
        /// Returns all welcome regulation rules by regulation.
        /// </summary>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns>The list of welcome regulation rules.</returns>
        Task<IEnumerable<WelcomeRegulationRuleModel>> GetWelcomeRegulationRulesByRegulationIdAsync(string regulationId);

        /// <summary>
        /// Adds welcome regulation rule.
        /// </summary>
        /// <param name="model">The model what describe a welcome regulation rule.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        Task AddWelcomeRegulationRuleAsync(WelcomeRegulationRuleModel model);

        /// <summary>
        /// Removes welcome regulation rule.
        /// </summary>
        /// <param name="regulationId">The regulation id for remove.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        Task DeleteWelcomeRegulationRuleAsync(string regulationId);

        /// <summary>
        /// Returns client regulation.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns>Client regulation.</returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        /// <exception cref="InvalidOperationException">Thrown if an unexpected response received.</exception>
        Task<string> GetClientRegulationAsync(string clientId, string regulationId);

        /// <summary>
        /// Returns all client regulations.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <returns>The list of client regulations.</returns>
        Task<IEnumerable<ClientRegulationModel>> GetClientRegulationsByClientIdAsync(string clientId);

        /// <summary>
        /// Returns client regulations by regulation.
        /// </summary>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns>The list of client regulations.</returns>
        Task<IEnumerable<ClientRegulationModel>> GetClientRegulationsByRegulationIdAsync(string regulationId);

        /// <summary>
        /// Returns client active regulations.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <returns>The list of client regulations.</returns>
        Task<IEnumerable<ClientRegulationModel>> GetActiveClientRegulationsByClientIdAsync(string clientId);

        /// <summary>
        /// Returns client available regulations.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <returns>The list of client regulations.</returns>
        Task<IEnumerable<ClientRegulationModel>> GetAvailableClientRegulationsByClientIdAsync(string clientId);

        /// <summary>
        /// Adds client regulation.
        /// </summary>
        /// <param name="model">The model what describe a client regulation.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        Task AddClientRegulationAsync(ClientRegulationModel model);

        /// <summary>
        /// Sets default client regulations.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="country">The country assosiated with client.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        Task SetDefaultClientRegulationsAsync(string clientId, string country);

        /// <summary>
        /// Sets client regulation KYC to <c>true</c>.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        Task SetClientRegulationKycAsync(string clientId, string regulationId);

        /// <summary>
        /// Sets client regulation active state to <c>true</c>.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        Task ActivateClientRegulationAsync(string clientId, string regulationId);

        /// <summary>
        /// Sets client regulation active state to <c>false</c>.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        Task DeactivateClientRegulationAsync(string clientId, string regulationId);

        /// <summary>
        /// Removes client regulation.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">Thrown if an error response received from service.</exception>
        Task DeleteClientRegulationAsync(string clientId, string regulationId);
    }
}
