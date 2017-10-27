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
        private readonly IRegulationService _regulationService;
        private readonly IClientRegulationService _clientRegulationService;
        private readonly ILog _log;

        public ClientRegulationController(
            IRegulationService regulationService,
            IClientRegulationService clientRegulationService,
            ILog log)
        {
            _regulationService = regulationService;
            _clientRegulationService = clientRegulationService;
            _log = log;
        }

        [HttpGet]
        [Route("{clientId}")]
        [SwaggerOperation("GetClientRegulation")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(string clientId)
        {
            string regulation = await _clientRegulationService.GetAsync(clientId);

            if (string.IsNullOrEmpty(regulation))
            {
                await _log.WriteWarningAsync(nameof(ClientRegulationController), nameof(Get),
                    $"Client '{clientId}' have not regulation. IP: {HttpContext.GetIp()}");
                return NotFound(ErrorResponse.Create("Client have not regulation"));
            }

            return Ok(regulation);
        }

        [HttpGet]
        [Route("available/{clientId}")]
        [SwaggerOperation("GetClientAvailableRegulations")]
        [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAvailable(string clientId)
        {
            IEnumerable<string> regulations = await _clientRegulationService.GetAvailableAsync(clientId);

            return Ok(regulations);
        }

        [HttpPost]
        [Route("available/add")]
        [SwaggerOperation("AddClientAvailableRegulation")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task Add([FromBody] ClientAvailableRegulationModel model)
        {
            var clientAvailableRegulation = Mapper.Map<ClientAvailableRegulation>(model);
            await _clientRegulationService.AddAsync(clientAvailableRegulation);
            
            await _log.WriteInfoAsync(nameof(ClientRegulationController), nameof(Add),
                $"New available regulation added to client. Model ${model.ToJson()}. IP: {HttpContext.GetIp()}");

            string regulationId = await _clientRegulationService.GetAsync(model.ClientId);
            IRegulation regulation = null;

            if (!string.IsNullOrEmpty(regulationId))
            {
                regulation = await _regulationService.GetAsync(regulationId);
            }

            if (regulation == null || !regulation.RequiresKYC)
            {
                await _clientRegulationService.SetAsync(new ClientRegulation
                {
                    RegulationId = model.RegulationId,
                    ClientId = model.ClientId
                });

                await _log.WriteInfoAsync(nameof(ClientRegulationController), nameof(Add),
                    $"Regulation assigned to client. Model ${model.ToJson()}. IP: {HttpContext.GetIp()}");
            }
        }

        [HttpPost]
        [Route("set")]
        [SwaggerOperation("SetClientRegulation")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Set([FromBody] ClientRegulationModel model)
        {
            var regulation = Mapper.Map<ClientRegulation>(model);

            try
            {
                await _clientRegulationService.SetAsync(regulation);
            }
            catch (ServiceException exception)
            {
                await _log.WriteWarningAsync(nameof(ClientRegulationController), nameof(Set),
                    $"{exception.Message} Model ${model.ToJson()}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            await _log.WriteInfoAsync(nameof(ClientRegulationController), nameof(Set),
                $"Regulation assigned to client. Model ${model.ToJson()}. IP: {HttpContext.GetIp()}");

            return Ok();
        }

        [HttpDelete]
        [Route("{clientId}")]
        [SwaggerOperation("RemoveClientRegulation")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task Remove(string clientId)
        {
            await _clientRegulationService.RemoveAsync(clientId);

            await _log.WriteInfoAsync(nameof(ClientRegulationController), nameof(Remove),
                $"Client current regulation removed. ClientId ${clientId}. IP: {HttpContext.GetIp()}");
        }

        [HttpDelete]
        [Route("available/{clientId}/{regulationId}")]
        [SwaggerOperation("RemoveClientAvailableRegulation")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveAvailable(string clientId, string regulationId)
        {
            try
            {
                await _clientRegulationService.RemoveAvailableAsync(clientId, regulationId);
            }
            catch (ServiceException exception)
            {
                await _log.WriteWarningAsync(nameof(ClientRegulationController), nameof(RemoveAvailable),
                    $"{exception.Message} ClientId: ${clientId}. RegulationId: {regulationId}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            await _log.WriteInfoAsync(nameof(ClientRegulationController), nameof(RemoveAvailable),
                $"Client available regulation removed. ClientId: ${clientId}. RegulationId: {regulationId}. IP: {HttpContext.GetIp()}");

            return Ok();
        }
    }
}
