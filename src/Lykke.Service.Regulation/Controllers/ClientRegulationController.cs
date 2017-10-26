using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.Regulation.Core.Domain;
using Lykke.Service.Regulation.Core.Services;
using Lykke.Service.Regulation.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;

namespace Lykke.Service.Regulation.Controllers
{
    [Route("api/[controller]")]
    public class ClientRegulationController : Controller
    {
        private readonly IClientRegulationService _clientRegulationService;

        public ClientRegulationController(IClientRegulationService clientRegulationService)
        {
            _clientRegulationService = clientRegulationService;
        }

        [HttpGet]
        [Route("{clientId}")]
        [SwaggerOperation("Get")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAsync(string clientId)
        {
            string regulation = await _clientRegulationService.GetAsync(clientId);

            if (string.IsNullOrEmpty(regulation))
            {
                return NotFound(ErrorResponse.Create("Client have not regulation"));
            }

            return Ok(regulation);
        }

        [HttpGet]
        [Route("available/{clientId}")]
        [SwaggerOperation("GetAvailable")]
        [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAvailableAsync(string clientId)
        {
            IEnumerable<string> regulations = await _clientRegulationService.GetAvailableAsync(clientId);

            return Ok(regulations);
        }

        [HttpPost]
        [Route("available/add")]
        [SwaggerOperation("Add")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task Add([FromBody] ClientAvailableRegulationModel model)
        {
            var regulation = Mapper.Map<ClientAvailableRegulation>(model);
            await _clientRegulationService.AddAsync(regulation);
        }

        [HttpPost]
        [Route("set")]
        [SwaggerOperation("Set")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Set([FromBody] ClientRegulationModel model)
        {
            var regulation = Mapper.Map<ClientRegulation>(model);

            // TODO: Make custom exception
            try
            {
                await _clientRegulationService.SetAsync(regulation);
            }
            catch (Exception exception)
            {
                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            return Ok();
        }

        [HttpDelete]
        [Route("{clientId}")]
        [SwaggerOperation("Remove")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task RemoveAsync(string clientId)
        {
            await _clientRegulationService.Remove(clientId);
        }

        [HttpDelete]
        [Route("available/{clientId}/{regulationId}")]
        [SwaggerOperation("RemoveAvailable")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveAvailableAsync(string clientId, string regulationId)
        {
            // TODO: Make custom exception
            try
            {
                await _clientRegulationService.RemoveAvailable(clientId, regulationId);
            }
            catch (Exception exception)
            {
                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            return Ok();
        }
    }
}
