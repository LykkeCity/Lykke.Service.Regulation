using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common.Log;
using Lykke.Common.Extensions;
using Lykke.Common.Log;
using Lykke.Service.Regulation.Core.Domain;
using Lykke.Service.Regulation.Core.Exceptions;
using Lykke.Service.Regulation.Core.Services;
using Lykke.Service.Regulation.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.Regulation.Controllers
{
    /// <summary>
    /// Provides methods for working with client margin regulation.
    /// </summary>
    [Route("api/[controller]")]
    public class ClientMarginRegulationController : Controller
    {
        private readonly IClientMarginRegulationService _clientMarginRegulationService;
        private readonly ILog _log;

        public ClientMarginRegulationController(
            IClientMarginRegulationService clientMarginRegulationService,
            ILogFactory logFactory)
        {
            _clientMarginRegulationService = clientMarginRegulationService;
            _log = logFactory.CreateLog(this);
        }

        /// <summary>
        /// Returns a client margin regulation by specified client id.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <returns>The client margin regulation.</returns>
        /// <response code="200">The client margin regulation.</response>
        /// <response code="404">Client margin regulation not found.</response>
        [HttpGet]
        [Route("client/{clientId}")]
        [SwaggerOperation("ClientMarginRegulationGetByClientId")]
        [ProducesResponseType(typeof(ClientMarginRegulationModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetByClientIdAsync(string clientId)
        {
            IClientMarginRegulation clientMarginRegulation = await _clientMarginRegulationService.GetAsync(clientId);

            if (clientMarginRegulation == null)
                return NotFound();

            var model = Mapper.Map<ClientMarginRegulationModel>(clientMarginRegulation);

            return Ok(model);
        }

        /// <summary>
        /// Adds the client margin regulation.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns></returns>
        /// <response code="204">Client margin regulation successfully added.</response>
        /// <response code="400">Regulation not found or client magin regulation already exists.</response>
        [HttpPost]
        [Route("client/{clientId}/regulation/{regulationId}")]
        [SwaggerOperation("ClientMarginRegulationAdd")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddAsync(string clientId, string regulationId)
        {
            try
            {
                await _clientMarginRegulationService.AddAsync(clientId, regulationId);
            }
            catch (ServiceException exception)
            {
                _log.Error(nameof(AddAsync), exception,
                    $"{nameof(clientId)}: {clientId}. {nameof(regulationId)}: {regulationId}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            _log.Info(nameof(AddAsync),
                $"Client regulation added. {nameof(regulationId)}: {regulationId}. IP: {HttpContext.GetIp()}", clientId);

            return NoContent();
        }

        /// <summary>
        /// Removes existing client margin regulation and adds new one.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns></returns>
        /// <response code="204">Client margin regulation successfully updated.</response>
        /// <response code="400">Regulation not found.</response>
        [HttpPut]
        [Route("client/{clientId}/regulation/{regulationId}")]
        [SwaggerOperation("ClientMarginRegulationSet")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SetAsync(string clientId, string regulationId)
        {
            try
            {
                await _clientMarginRegulationService.SetAsync(clientId, regulationId);
            }
            catch (ServiceException exception)
            {
                _log.Error(nameof(SetAsync), exception,
                    $"{nameof(clientId)}: {clientId}. {nameof(regulationId)}: {regulationId}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            _log.Info(nameof(SetAsync),
                $"Client mrgin regulation updated. {nameof(regulationId)}: {regulationId}. IP: {HttpContext.GetIp()}", clientId);

            return NoContent();
        }

        /// <summary>
        /// Deletes a client margin regulation.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <returns></returns>
        /// <response code="204">Client margin regulation successfully deleted.</response>
        [HttpDelete]
        [Route("client/{clientId}")]
        [SwaggerOperation("ClientMarginRegulationDelete")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteAsync(string clientId)
        {
            await _clientMarginRegulationService.DeleteAsync(clientId);

            _log.Info(nameof(DeleteAsync), $"Client margin regulation deleted. IP: {HttpContext.GetIp()}", clientId);

            return NoContent();
        }
    }
}
