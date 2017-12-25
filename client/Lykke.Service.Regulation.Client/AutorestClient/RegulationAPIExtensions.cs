// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Service.Regulation.Client.AutorestClient
{
    using Models;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for RegulationAPI.
    /// </summary>
    public static partial class RegulationAPIExtensions
    {
            /// <summary>
            /// Returns a client regulation by specified client id and regulation id.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='clientId'>
            /// The client id.
            /// </param>
            /// <param name='regulationId'>
            /// The regulation id.
            /// </param>
            public static object GetClientRegulation(this IRegulationAPI operations, string clientId, string regulationId)
            {
                return operations.GetClientRegulationAsync(clientId, regulationId).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Returns a client regulation by specified client id and regulation id.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='clientId'>
            /// The client id.
            /// </param>
            /// <param name='regulationId'>
            /// The regulation id.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> GetClientRegulationAsync(this IRegulationAPI operations, string clientId, string regulationId, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetClientRegulationWithHttpMessagesAsync(clientId, regulationId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Deletes the regulation associated with client by specified id.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='clientId'>
            /// The client id.
            /// </param>
            /// <param name='regulationId'>
            /// The id of regulation to delete.
            /// </param>
            public static ErrorResponse DeleteClientRegulation(this IRegulationAPI operations, string clientId, string regulationId)
            {
                return operations.DeleteClientRegulationAsync(clientId, regulationId).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Deletes the regulation associated with client by specified id.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='clientId'>
            /// The client id.
            /// </param>
            /// <param name='regulationId'>
            /// The id of regulation to delete.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ErrorResponse> DeleteClientRegulationAsync(this IRegulationAPI operations, string clientId, string regulationId, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.DeleteClientRegulationWithHttpMessagesAsync(clientId, regulationId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Returns a regulations associated with client.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='clientId'>
            /// The client id.
            /// </param>
            public static IList<ClientRegulationModel> GetClientRegulationsByClientId(this IRegulationAPI operations, string clientId)
            {
                return operations.GetClientRegulationsByClientIdAsync(clientId).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Returns a regulations associated with client.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='clientId'>
            /// The client id.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<ClientRegulationModel>> GetClientRegulationsByClientIdAsync(this IRegulationAPI operations, string clientId, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetClientRegulationsByClientIdWithHttpMessagesAsync(clientId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Returns a client regulations associated with regulation.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='regulationId'>
            /// The regulation id.
            /// </param>
            public static object GetClientRegulationsByRegulationId(this IRegulationAPI operations, string regulationId)
            {
                return operations.GetClientRegulationsByRegulationIdAsync(regulationId).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Returns a client regulations associated with regulation.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='regulationId'>
            /// The regulation id.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> GetClientRegulationsByRegulationIdAsync(this IRegulationAPI operations, string regulationId, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetClientRegulationsByRegulationIdWithHttpMessagesAsync(regulationId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Returns an active regulations associated with client.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='clientId'>
            /// The client id.
            /// </param>
            public static IList<ClientRegulationModel> GetActiveClientRegulationsByClientId(this IRegulationAPI operations, string clientId)
            {
                return operations.GetActiveClientRegulationsByClientIdAsync(clientId).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Returns an active regulations associated with client.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='clientId'>
            /// The client id.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<ClientRegulationModel>> GetActiveClientRegulationsByClientIdAsync(this IRegulationAPI operations, string clientId, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetActiveClientRegulationsByClientIdWithHttpMessagesAsync(clientId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Returns an active and KYC regulations associated with client.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='clientId'>
            /// The client id.
            /// </param>
            public static IList<ClientRegulationModel> GetAvailableClientRegulationsByClientId(this IRegulationAPI operations, string clientId)
            {
                return operations.GetAvailableClientRegulationsByClientIdAsync(clientId).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Returns an active and KYC regulations associated with client.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='clientId'>
            /// The client id.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<ClientRegulationModel>> GetAvailableClientRegulationsByClientIdAsync(this IRegulationAPI operations, string clientId, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetAvailableClientRegulationsByClientIdWithHttpMessagesAsync(clientId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Adds the client regulation.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// The model what describe a client regulation.
            /// </param>
            public static ErrorResponse AddClientRegulation(this IRegulationAPI operations, NewClientRegulationModel model = default(NewClientRegulationModel))
            {
                return operations.AddClientRegulationAsync(model).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Adds the client regulation.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// The model what describe a client regulation.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ErrorResponse> AddClientRegulationAsync(this IRegulationAPI operations, NewClientRegulationModel model = default(NewClientRegulationModel), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.AddClientRegulationWithHttpMessagesAsync(model, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Initializes client regulations using rules associated with country.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='clientId'>
            /// The client id.
            /// </param>
            /// <param name='country'>
            /// The country name.
            /// </param>
            public static ErrorResponse SetDefaultClientRegulations(this IRegulationAPI operations, string clientId, string country)
            {
                return operations.SetDefaultClientRegulationsAsync(clientId, country).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Initializes client regulations using rules associated with country.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='clientId'>
            /// The client id.
            /// </param>
            /// <param name='country'>
            /// The country name.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ErrorResponse> SetDefaultClientRegulationsAsync(this IRegulationAPI operations, string clientId, string country, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.SetDefaultClientRegulationsWithHttpMessagesAsync(clientId, country, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Updates the client regulation KYC status.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='clientId'>
            /// The client id.
            /// </param>
            /// <param name='regulationId'>
            /// The regulation id.
            /// </param>
            /// <param name='active'>
            /// The client regulation KYC status.
            /// </param>
            public static ErrorResponse UpdateClientRegulationKyc(this IRegulationAPI operations, string clientId, string regulationId, bool active)
            {
                return operations.UpdateClientRegulationKycAsync(clientId, regulationId, active).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Updates the client regulation KYC status.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='clientId'>
            /// The client id.
            /// </param>
            /// <param name='regulationId'>
            /// The regulation id.
            /// </param>
            /// <param name='active'>
            /// The client regulation KYC status.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ErrorResponse> UpdateClientRegulationKycAsync(this IRegulationAPI operations, string clientId, string regulationId, bool active, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.UpdateClientRegulationKycWithHttpMessagesAsync(clientId, regulationId, active, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Sets the client regulation active status to {true}.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='clientId'>
            /// The client id.
            /// </param>
            /// <param name='regulationId'>
            /// The regulation id.
            /// </param>
            public static ErrorResponse ActivateClientRegulation(this IRegulationAPI operations, string clientId, string regulationId)
            {
                return operations.ActivateClientRegulationAsync(clientId, regulationId).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Sets the client regulation active status to {true}.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='clientId'>
            /// The client id.
            /// </param>
            /// <param name='regulationId'>
            /// The regulation id.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ErrorResponse> ActivateClientRegulationAsync(this IRegulationAPI operations, string clientId, string regulationId, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ActivateClientRegulationWithHttpMessagesAsync(clientId, regulationId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Sets the client regulation active status to {false}.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='clientId'>
            /// The client id.
            /// </param>
            /// <param name='regulationId'>
            /// The regulation id.
            /// </param>
            public static ErrorResponse DeactivateClientRegulation(this IRegulationAPI operations, string clientId, string regulationId)
            {
                return operations.DeactivateClientRegulationAsync(clientId, regulationId).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Sets the client regulation active status to {false}.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='clientId'>
            /// The client id.
            /// </param>
            /// <param name='regulationId'>
            /// The regulation id.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ErrorResponse> DeactivateClientRegulationAsync(this IRegulationAPI operations, string clientId, string regulationId, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.DeactivateClientRegulationWithHttpMessagesAsync(clientId, regulationId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Checks service is alive
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static object IsAlive(this IRegulationAPI operations)
            {
                return operations.IsAliveAsync().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Checks service is alive
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> IsAliveAsync(this IRegulationAPI operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.IsAliveWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Returns a regulation details by specified id.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='regulationId'>
            /// The regulation id.
            /// </param>
            public static object GetRegulation(this IRegulationAPI operations, string regulationId)
            {
                return operations.GetRegulationAsync(regulationId).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Returns a regulation details by specified id.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='regulationId'>
            /// The regulation id.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> GetRegulationAsync(this IRegulationAPI operations, string regulationId, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetRegulationWithHttpMessagesAsync(regulationId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Deletes the regulation by specified id.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='regulationId'>
            /// The id of regulation to delete.
            /// </param>
            public static ErrorResponse DeleteRegulation(this IRegulationAPI operations, string regulationId)
            {
                return operations.DeleteRegulationAsync(regulationId).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Deletes the regulation by specified id.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='regulationId'>
            /// The id of regulation to delete.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ErrorResponse> DeleteRegulationAsync(this IRegulationAPI operations, string regulationId, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.DeleteRegulationWithHttpMessagesAsync(regulationId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Returns a regulation details by country code using welcome regulation
            /// rules.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='country'>
            /// The country code.
            /// </param>
            public static RegulationModel GetRegulationByCountry(this IRegulationAPI operations, string country)
            {
                return operations.GetRegulationByCountryAsync(country).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Returns a regulation details by country code using welcome regulation
            /// rules.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='country'>
            /// The country code.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<RegulationModel> GetRegulationByCountryAsync(this IRegulationAPI operations, string country, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetRegulationByCountryWithHttpMessagesAsync(country, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Returns all regulations.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static IList<RegulationModel> GetRegulations(this IRegulationAPI operations)
            {
                return operations.GetRegulationsAsync().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Returns all regulations.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<RegulationModel>> GetRegulationsAsync(this IRegulationAPI operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetRegulationsWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Adds the regulation.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// The model what describe a regulation.
            /// </param>
            public static ErrorResponse AddRegulation(this IRegulationAPI operations, NewRegulationModel model = default(NewRegulationModel))
            {
                return operations.AddRegulationAsync(model).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Adds the regulation.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// The model what describe a regulation.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ErrorResponse> AddRegulationAsync(this IRegulationAPI operations, NewRegulationModel model = default(NewRegulationModel), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.AddRegulationWithHttpMessagesAsync(model, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Returns welcome regulation rule by specified id.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='welcomeRegulationRuleId'>
            /// </param>
            public static object GetWelcomeRegulationRuleById(this IRegulationAPI operations, string welcomeRegulationRuleId)
            {
                return operations.GetWelcomeRegulationRuleByIdAsync(welcomeRegulationRuleId).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Returns welcome regulation rule by specified id.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='welcomeRegulationRuleId'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> GetWelcomeRegulationRuleByIdAsync(this IRegulationAPI operations, string welcomeRegulationRuleId, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetWelcomeRegulationRuleByIdWithHttpMessagesAsync(welcomeRegulationRuleId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Returns all welcome regulation rules.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static IList<WelcomeRegulationRuleModel> GetWelcomeRegulationRules(this IRegulationAPI operations)
            {
                return operations.GetWelcomeRegulationRulesAsync().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Returns all welcome regulation rules.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<WelcomeRegulationRuleModel>> GetWelcomeRegulationRulesAsync(this IRegulationAPI operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetWelcomeRegulationRulesWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Adds the welcome regulation rule.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// The model what describe a welcome regulation rule.
            /// </param>
            public static ErrorResponse AddWelcomeRegulationRule(this IRegulationAPI operations, NewWelcomeRegulationRuleModel model = default(NewWelcomeRegulationRuleModel))
            {
                return operations.AddWelcomeRegulationRuleAsync(model).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Adds the welcome regulation rule.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// The model what describe a welcome regulation rule.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ErrorResponse> AddWelcomeRegulationRuleAsync(this IRegulationAPI operations, NewWelcomeRegulationRuleModel model = default(NewWelcomeRegulationRuleModel), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.AddWelcomeRegulationRuleWithHttpMessagesAsync(model, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Returns all welcome regulation rules associated with specified country.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='country'>
            /// The country name.
            /// </param>
            public static IList<WelcomeRegulationRuleModel> GetWelcomeRegulationRulesByCountry(this IRegulationAPI operations, string country)
            {
                return operations.GetWelcomeRegulationRulesByCountryAsync(country).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Returns all welcome regulation rules associated with specified country.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='country'>
            /// The country name.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<WelcomeRegulationRuleModel>> GetWelcomeRegulationRulesByCountryAsync(this IRegulationAPI operations, string country, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetWelcomeRegulationRulesByCountryWithHttpMessagesAsync(country, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Returns all welcome regulation rules associated with specified regulation.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='regulationId'>
            /// The regulation id.
            /// </param>
            public static object GetWelcomeRegulationRulesByRegulationId(this IRegulationAPI operations, string regulationId)
            {
                return operations.GetWelcomeRegulationRulesByRegulationIdAsync(regulationId).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Returns all welcome regulation rules associated with specified regulation.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='regulationId'>
            /// The regulation id.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> GetWelcomeRegulationRulesByRegulationIdAsync(this IRegulationAPI operations, string regulationId, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetWelcomeRegulationRulesByRegulationIdWithHttpMessagesAsync(regulationId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Updates welcome regulation rule.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// The model what describe a welcome regulation rule.
            /// </param>
            public static ErrorResponse UpdateWelcomeRegulationRule(this IRegulationAPI operations, WelcomeRegulationRuleModel model = default(WelcomeRegulationRuleModel))
            {
                return operations.UpdateWelcomeRegulationRuleAsync(model).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Updates welcome regulation rule.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// The model what describe a welcome regulation rule.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ErrorResponse> UpdateWelcomeRegulationRuleAsync(this IRegulationAPI operations, WelcomeRegulationRuleModel model = default(WelcomeRegulationRuleModel), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.UpdateWelcomeRegulationRuleWithHttpMessagesAsync(model, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Deletes the welcome regulation rule by specified regulation id.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='regulationRuleId'>
            /// The welcome regulation rule id.
            /// </param>
            public static ErrorResponse DeleteWelcomeRegulationRule(this IRegulationAPI operations, string regulationRuleId)
            {
                return operations.DeleteWelcomeRegulationRuleAsync(regulationRuleId).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Deletes the welcome regulation rule by specified regulation id.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='regulationRuleId'>
            /// The welcome regulation rule id.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ErrorResponse> DeleteWelcomeRegulationRuleAsync(this IRegulationAPI operations, string regulationRuleId, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.DeleteWelcomeRegulationRuleWithHttpMessagesAsync(regulationRuleId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}
