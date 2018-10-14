using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Common.Log;
using Lykke.Common.Extensions;
using Lykke.Common.Log;
using Lykke.Service.Regulation.Core.Domain;
using Lykke.Service.Regulation.Core.Services;
using Lykke.Service.Regulation.Models;
using Lykke.Service.Regulation.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.Regulation.Controllers
{
    /// <summary>
    /// Provides methods for working with client regulations.
    /// </summary>
    [Route("api/[controller]")]
    public class ClientRegulationController : Controller
    {
        private readonly IClientRegulationService _clientRegulationService;
        private readonly ILog _log;

        public ClientRegulationController(IClientRegulationService clientRegulationService, ILogFactory logFactory)
        {
            _clientRegulationService = clientRegulationService;
            _log = logFactory.CreateLog(this);
        }

        /// <summary>
        /// Returns a client regulation by specified client id and regulation id.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns>The client regulation.</returns>
        /// <response code="200">The client regulation.</response>
        /// <response code="400">Client regulation not found.</response>
        [HttpGet]
        [Route("{clientId}/{regulationId}")]
        [SwaggerOperation("ClientRegulationGet")]
        [ProducesResponseType(typeof(ClientRegulationModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Get(string clientId, string regulationId)
        {
            IClientRegulation clientRegulation;

            try
            {
                clientRegulation = await _clientRegulationService.GetAsync(clientId, regulationId);
            }
            catch (ServiceException exception)
            {
                _log.Error(nameof(Get), exception,
                    $"{nameof(clientId)}: {clientId}. {nameof(regulationId)}: {regulationId}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }
            
            var model = Mapper.Map<ClientRegulationModel>(clientRegulation);

            return Ok(model);
        }

        /// <summary>
        /// Returns a regulations associated with client.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <returns>The list of client regulations.</returns>
        /// <response code="200">The list of client regulations.</response>
        [HttpGet]
        [Route("client/{clientId}")]
        [SwaggerOperation("ClientRegulationGetByClientId")]
        [ProducesResponseType(typeof(IEnumerable<ClientRegulationModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetByClientId(string clientId)
        {
            IEnumerable<IClientRegulation> clientRegulation = 
                await _clientRegulationService.GetByClientIdAsync(clientId);

            var model = Mapper.Map<IEnumerable<ClientRegulationModel>>(clientRegulation);

            return Ok(model);
        }

        /// <summary>
        /// Returns a client regulations associated with regulation.
        /// </summary>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns>The list of client regulations.</returns>
        /// <response code="200">The list of client regulations.</response>
        /// <response code="400">Regulation not found.</response>
        [HttpGet]
        [Route("regulation/{regulationId}")]
        [SwaggerOperation("ClientRegulationGetByRegulationId")]
        [ProducesResponseType(typeof(IEnumerable<ClientRegulationModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetByRegulationId(string regulationId)
        {
            IEnumerable<IClientRegulation> clientRegulations;

            try
            {
                clientRegulations  = await _clientRegulationService.GetByRegulationIdAsync(regulationId);
            }
            catch (ServiceException exception)
            {
                _log.Error(nameof(GetByRegulationId), exception,
                    $"{nameof(regulationId)}: {regulationId}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            var model = Mapper.Map<IEnumerable<ClientRegulationModel>>(clientRegulations);

            return Ok(model);
        }

        /// <summary>
        /// Returns an active regulations associated with client.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <returns>The list of client regulations.</returns>
        /// <response code="200">The list of client regulations.</response>
        [HttpGet]
        [Route("active/{clientId}")]
        [SwaggerOperation("ClientRegulationGetActiveByClientId")]
        [ProducesResponseType(typeof(IEnumerable<ClientRegulationModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetActiveByClientId(string clientId)
        {
            IEnumerable<IClientRegulation> clientRegulations =
                await _clientRegulationService.GetActiveByClientIdAsync(clientId);

            var model = Mapper.Map<IEnumerable<ClientRegulationModel>>(clientRegulations);

            return Ok(model);
        }

        /// <summary>
        /// Returns an active and KYC regulations associated with client.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <returns>The list of client regulations.</returns>
        /// <response code="200">The list of client regulations.</response>
        [HttpGet]
        [Route("available/{clientId}")]
        [SwaggerOperation("ClientRegulationGetAvailableByClientId")]
        [ProducesResponseType(typeof(IEnumerable<ClientRegulationModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAvailableByClientId(string clientId)
        {
            IEnumerable<IClientRegulation> clientRegulations =
                await _clientRegulationService.GetAvailableByClientIdAsync(clientId);

            var model = Mapper.Map<IEnumerable<ClientRegulationModel>>(clientRegulations);

            return Ok(model);
        }

        /// <summary>
        /// Adds the client regulation.
        /// </summary>
        /// <param name="model">The model that describe a client regulation.</param>
        /// <returns></returns>
        /// <response code="204">Client regulation successfully added.</response>
        /// <response code="400">Invalid model that describe a client regulation or regulation not found.</response>
        [HttpPost]
        [Route("add")]
        [SwaggerOperation("ClientRegulationAdd")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Add([FromBody] NewClientRegulationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorResponse.Create("Invalid model.", ModelState));
            }

            var clientRegulation = Mapper.Map<ClientRegulation>(model);
            
            try
            {
                await _clientRegulationService.AddAsync(clientRegulation);
            }
            catch (ServiceException exception)
            {
                _log.Error(nameof(Add), exception,
                    $"{nameof(model)}: {model.ToJson()}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            _log.Info(nameof(Add), $"Client regulation added. {nameof(model)}: {model.ToJson()}. IP: {HttpContext.GetIp()}",
                model.ClientId);

            return NoContent();
        }

        /// <summary>
        /// Removes existing client regulations and adds new one.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns></returns>
        /// <response code="204">Client regulation successfully updated.</response>
        /// <response code="400">Regulation not found.</response>
        [HttpPut]
        [Route("client/{clientId}/regulation/{regulationId}")]
        [SwaggerOperation("ClientRegulationSet")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SetAsync(string clientId, string regulationId)
        {
            try
            {
                await _clientRegulationService.SetAsync(clientId, regulationId);
            }
            catch (ServiceException exception)
            {
                _log.Error(nameof(SetAsync), exception,
                    $"{nameof(clientId)}: {clientId}. {nameof(regulationId)}: {regulationId}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            _log.Info(nameof(Add), $"Client regulation updated. {nameof(regulationId)}: {regulationId}. IP: {HttpContext.GetIp()}", clientId);

            return NoContent();
        }

        /// <summary>
        /// Initializes client regulations using rules associated with country.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="country">The country name.</param>
        /// <returns></returns>
        /// <response code="204">Default regulations successfully added to client.</response>
        /// <response code="400">Client already have regulations or no default regulations for specified country.</response>
        [HttpPost]
        [Route("default/{clientId}/{country}")]
        [SwaggerOperation("ClientRegulationSetDefault")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SetDefault(string clientId, string country)
        {
            try
            {
                await _clientRegulationService.SetDefaultAsync(clientId, country);
            }
            catch (ServiceException exception)
            {
                _log.Error(nameof(SetDefault), exception,
                    $"{nameof(clientId)}: {clientId}. {nameof(country)}: {country}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            _log.Info(nameof(SetDefault), clientId,
                $"Default regulations was assigned for client. {nameof(country)}: {country}. IP: {HttpContext.GetIp()}");

            return NoContent();
        }

        /// <summary>
        /// Change client regulations using rules associated with country.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="country">The country name.</param>
        /// <returns></returns>
        /// <response code="204">Default regulations successfully added to client.</response>
        /// <response code="400">Client already have regulations or no default regulations for specified country.</response>
        [HttpPost]
        [Route("change/{clientId}/{country}")]
        [SwaggerOperation("ChangeClientRegulation")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ChangeClientRegulation(string clientId, string country)
        {
            try
            {
                await _clientRegulationService.ChangeRegulationAsync(clientId, country);
            }
            catch (ServiceException exception)
            {
                _log.Error(nameof(ChangeClientRegulation), exception,
                    $"{nameof(clientId)}: {clientId}. {nameof(country)}: {country}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            return NoContent();
        }

        /// <summary>
        /// Updates the client regulation KYC status.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="regulationId">The regulation id.</param>
        /// <param name="active">The client regulation KYC status.</param>
        /// <returns></returns>
        /// <response code="204">Client regulation KYC status successfully updated.</response>
        /// <response code="400">Client regulation not found.</response>
        [HttpPost]
        [Route("kyc/{clientId}/{regulationId}/{active}")]
        [SwaggerOperation("ClientRegulationUpdateKyc")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateKyc(string clientId, string regulationId, bool active)
        {
            try
            {
                await _clientRegulationService.UpdateKycAsync(clientId, regulationId, active);
            }
            catch (ServiceException exception)
            {
                _log.Error(nameof(UpdateKyc), exception,
                    $"{nameof(clientId)}: {clientId}. {nameof(regulationId)}: {regulationId}. {nameof(active)}: {active}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            _log.Info(nameof(UpdateKyc), $"Client regulation KYC updated. {nameof(regulationId)}: {regulationId}. {nameof(active)}: {active}. IP: {HttpContext.GetIp()}",
                clientId);

            return NoContent();
        }

        /// <summary>
        /// Sets the client regulation active status to <c>true</c>.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns></returns>
        /// <response code="204">Client regulation active status successfully updated.</response>
        /// <response code="400">Client regulation not found.</response>
        [HttpPost]
        [Route("activate/{clientId}/{regulationId}")]
        [SwaggerOperation("ClientRegulationActivate")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Activate(string clientId, string regulationId)
        {
            try
            {
                await _clientRegulationService.UpdateActiveAsync(clientId, regulationId, true);
            }
            catch (ServiceException exception)
            {
                _log.Error(nameof(Activate), exception,
                    $"{nameof(clientId)}: {clientId}. {nameof(regulationId)}: {regulationId}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            _log.Info(nameof(Activate), $"Client regulation activated. {nameof(regulationId)}: {regulationId}. IP: {HttpContext.GetIp()}",
                clientId);

            return NoContent();
        }

        /// <summary>
        /// Sets the client regulation active status to <c>false</c>.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns></returns>
        /// <response code="204">Client regulation active status successfully updated.</response>
        /// <response code="400">Client regulation not found.</response>
        [HttpPost]
        [Route("deactivate/{clientId}/{regulationId}")]
        [SwaggerOperation("ClientRegulationDeactivate")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Deactivate(string clientId, string regulationId)
        {
            try
            {
                await _clientRegulationService.UpdateActiveAsync(clientId, regulationId, false);
            }
            catch (ServiceException exception)
            {
                _log.Error(nameof(Deactivate), exception,
                    $"{nameof(clientId)}: {clientId}. {nameof(regulationId)}: {regulationId}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            _log.Info(nameof(Deactivate), $"Client regulation deactivated. {nameof(regulationId)}: {regulationId}. IP: {HttpContext.GetIp()}",
                clientId);

            return NoContent();
        }

        /// <summary>
        /// Deletes the regulation associated with client by specified id.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="regulationId">The id of regulation to delete.</param>
        /// <returns></returns>
        /// <response code="204">Client regulation successfully deleted.</response>
        /// <response code="400">Can not delete KYC or active client regulation.</response>
        [HttpDelete]
        [Route("{clientId}/{regulationId}")]
        [SwaggerOperation("ClientRegulationDelete")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Delete(string clientId, string regulationId)
        {
            try
            {
                await _clientRegulationService.DeleteAsync(clientId, regulationId);
            }
            catch (ServiceException exception)
            {
                _log.Error(nameof(Delete), exception,
                    $"{nameof(clientId)}: {clientId}. {nameof(regulationId)}: {regulationId}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            _log.Info(nameof(Delete), $"Client current regulation deleted. {nameof(regulationId)}: {regulationId}. IP: {HttpContext.GetIp()}",
                clientId);

            return NoContent();
        }
    }
}
