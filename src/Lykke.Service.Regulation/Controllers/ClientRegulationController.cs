using System.Collections.Generic;
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

        [HttpGet]
        [Route("active/{clientId}")]
        [SwaggerOperation("GetClientRegulationsActiveByClientId")]
        [ProducesResponseType(typeof(IEnumerable<ClientRegulationModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetActiveByClientId(string clientId)
        {
            IEnumerable<IClientRegulation> clientRegulations =
                await _clientRegulationService.GetActiveByClientIdAsync(clientId);

            var model = Mapper.Map<IEnumerable<ClientRegulationModel>>(clientRegulations);

            return Ok(model);
        }

        [HttpGet]
        [Route("available/{clientId}")]
        [SwaggerOperation("GetClientRegulationsAvailableByClientId")]
        [ProducesResponseType(typeof(IEnumerable<ClientRegulationModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAvailableByClientId(string clientId)
        {
            IEnumerable<IClientRegulation> clientRegulations =
                await _clientRegulationService.GetAvailableByClientIdAsync(clientId);

            var model = Mapper.Map<IEnumerable<ClientRegulationModel>>(clientRegulations);

            return Ok(model);
        }

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

            return Ok();
        }

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

            return Ok();
        }

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

            return Ok();
        }

        [HttpPost]
        [Route("deactivate/{clientId}/{regulationId}")]
        [SwaggerOperation("ActivateClientRegulation")]
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

            return Ok();
        }

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

            return Ok();
        }
    }
}
