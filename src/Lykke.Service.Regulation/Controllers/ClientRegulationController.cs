﻿using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Common.Log;
using Lykke.Service.Regulation.Core.Domain;
using Lykke.Service.Regulation.Core.Services;
using Lykke.Service.Regulation.Extensions;
using Lykke.Service.Regulation.Models;
using Lykke.Service.Regulation.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;

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

        public ClientRegulationController(
            IClientRegulationService clientRegulationService,
            ILog log)
        {
            _clientRegulationService = clientRegulationService;
            _log = log;
        }

        /// <summary>
        /// Returns a client regulation by specified client id and regulation id.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns>The client regulation.</returns>
        /// <response code="200">The client regulation.</response>
        /// <response code="404">Client regulation not found.</response>
        [HttpGet]
        [Route("{clientId}/{regulationId}")]
        [SwaggerOperation("GetClientRegulation")]
        [ProducesResponseType(typeof(ClientRegulationModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(string clientId, string regulationId)
        {
            IClientRegulation clientRegulation =
                await _clientRegulationService.GetAsync(clientId, regulationId);

            if (clientRegulation == null)
            {
                await _log.WriteWarningAsync(nameof(ClientRegulationController), nameof(Get),
                    $"Client '{clientId}' have not regulation. IP: {HttpContext.GetIp()}");
                return NotFound(ErrorResponse.Create("Client regulation not found."));
            }

            var model = Mapper.Map<ClientRegulationModel>(clientRegulation);

            return Ok(model);
        }

        /// <summary>
        /// Returns a client regulations associated with client.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <returns>The list of client regulations.</returns>
        /// <response code="200">The list of client regulations.</response>
        [HttpGet]
        [Route("client/{clientId}")]
        [SwaggerOperation("GetClientRegulationsByClientId")]
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
        [HttpGet]
        [Route("regulation/{regulationId}")]
        [SwaggerOperation("GetClientRegulationsByRegulationId")]
        [ProducesResponseType(typeof(IEnumerable<ClientRegulationModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetByRegulationId(string regulationId)
        {
            IEnumerable<IClientRegulation> clientRegulations =
                await _clientRegulationService.GetByRegulationIdAsync(regulationId);

            var model = Mapper.Map<IEnumerable<ClientRegulationModel>>(clientRegulations);

            return Ok(model);
        }

        /// <summary>
        /// Returns an active client regulations associated with client.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <returns>The list of client regulations.</returns>
        /// <response code="200">The list of client regulations.</response>
        [HttpGet]
        [Route("active/{clientId}")]
        [SwaggerOperation("GetActiveClientRegulationsByClientId")]
        [ProducesResponseType(typeof(IEnumerable<ClientRegulationModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetActiveByClientId(string clientId)
        {
            IEnumerable<IClientRegulation> clientRegulations =
                await _clientRegulationService.GetActiveByClientIdAsync(clientId);

            var model = Mapper.Map<IEnumerable<ClientRegulationModel>>(clientRegulations);

            return Ok(model);
        }

        /// <summary>
        /// Returns an active and KYC client regulations associated with client.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <returns>The list of client regulations.</returns>
        /// <response code="200">The list of client regulations.</response>
        [HttpGet]
        [Route("available/{clientId}")]
        [SwaggerOperation("GetAvailableClientRegulationsByClientId")]
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
        /// <param name="model">The model what describe a client regulation.</param>
        /// <returns></returns>
        /// <response code="204">Client regulation successfully added.</response>
        /// <response code="400">Invalid model what describe a client regulation.</response>
        [HttpPost]
        [Route("add")]
        [SwaggerOperation("AddClientRegulation")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Add([FromBody] ClientRegulationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorResponse.Create("Invalid model.", ModelState));
            }

            var clientRegulation = Mapper.Map<ClientRegulation>(model);
            await _clientRegulationService.AddAsync(clientRegulation);
            
            await _log.WriteInfoAsync(nameof(ClientRegulationController), nameof(Add),
                $"Client regulation added. Model {model.ToJson()}. IP: {HttpContext.GetIp()}");

            return NoContent();
        }

        /// <summary>
        /// Adds default regulations to client associated with country.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="country">The country name.</param>
        /// <returns></returns>
        /// <response code="204">Default regulations successfully added to client.</response>
        /// <response code="400">Client already have regulations or no default regulations for specified country.</response>
        [HttpPost]
        [Route("default/{clientId}/{country}")]
        [SwaggerOperation("SetDefaultClientRegulations")]
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
                await _log.WriteWarningAsync(nameof(ClientRegulationController), nameof(SetDefault),
                    $"{exception.Message} ClientId: {clientId}. Country: {country}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            await _log.WriteInfoAsync(nameof(ClientRegulationController), nameof(SetDefault),
                $"Default regulations was assigned for client. ClientId: {clientId}. Country: {country}. IP: {HttpContext.GetIp()}");

            return NoContent();
        }

        /// <summary>
        /// Sets the client regulation KYC status to <c>true</c>.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns></returns>
        /// <response code="204">Client regulation KYC status successfully updated.</response>
        /// <response code="400">Client regulation not found.</response>
        [HttpPost]
        [Route("kyc/{clientId}/{regulationId}")]
        [SwaggerOperation("SetClientRegulationKyc")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SetKyc(string clientId, string regulationId)
        {
            try
            {
                await _clientRegulationService.SetKycAsync(clientId, regulationId);
            }
            catch (ServiceException exception)
            {
                await _log.WriteWarningAsync(nameof(ClientRegulationController), nameof(SetKyc),
                    $"{exception.Message} ClientId: {clientId}. RegulationId: {regulationId}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            await _log.WriteInfoAsync(nameof(ClientRegulationController), nameof(SetKyc),
                $"Client regulation KYC updated. ClientId: {clientId}. RegulationId: {regulationId}. IP: {HttpContext.GetIp()}");

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
        [SwaggerOperation("ActivateClientRegulation")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Activate(string clientId, string regulationId)
        {
            try
            {
                await _clientRegulationService.SetActiveAsync(clientId, regulationId, true);
            }
            catch (ServiceException exception)
            {
                await _log.WriteWarningAsync(nameof(ClientRegulationController), nameof(Activate),
                    $"{exception.Message} ClientId: {clientId}. RegulationId: {regulationId}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            await _log.WriteInfoAsync(nameof(ClientRegulationController), nameof(Activate),
                $"Client regulation activated. ClientId: {clientId}. RegulationId: {regulationId}. IP: {HttpContext.GetIp()}");

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
        [SwaggerOperation("DeactivateClientRegulation")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Deactivate(string clientId, string regulationId)
        {
            try
            {
                await _clientRegulationService.SetActiveAsync(clientId, regulationId, false);
            }
            catch (ServiceException exception)
            {
                await _log.WriteWarningAsync(nameof(ClientRegulationController), nameof(Deactivate),
                    $"{exception.Message} ClientId: {clientId}. RegulationId: {regulationId}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            await _log.WriteInfoAsync(nameof(ClientRegulationController), nameof(Deactivate),
                $"Client regulation deactivated. ClientId: {clientId}. RegulationId: {regulationId}. IP: {HttpContext.GetIp()}");

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
        [SwaggerOperation("DeleteClientRegulation")]
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
                await _log.WriteWarningAsync(nameof(ClientRegulationController), nameof(Delete),
                    $"{exception.Message} ClientId: {clientId}. RegulationId: {regulationId}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            await _log.WriteInfoAsync(nameof(ClientRegulationController), nameof(Delete),
                $"Client current regulation deleted. ClientId {clientId}. RegulationId: {regulationId}. IP: {HttpContext.GetIp()}");

            return NoContent();
        }
    }
}
