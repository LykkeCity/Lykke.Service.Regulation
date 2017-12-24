using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Common.Log;
using Lykke.Service.Regulation.Core.Services;
using Lykke.Service.Regulation.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using Lykke.Service.Regulation.Core.Domain;
using Lykke.Common.Extensions;
using Lykke.Service.Regulation.Core.Exceptions;

namespace Lykke.Service.Regulation.Controllers
{
    /// <summary>
    /// Provides methods for working with regulations.
    /// </summary>
    [Route("api/[controller]")]
    public class RegulationController : Controller
    {
        private readonly IRegulationService _regulationService;
        private readonly ILog _log;

        public RegulationController(IRegulationService regulationService, ILog log)
        {
            _regulationService = regulationService;
            _log = log;
        }

        /// <summary>
        /// Returns a regulation details by specified id.
        /// </summary>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns>The regulation if exists, otherwise <see cref="ErrorResponse"/>.</returns>
        /// <response code="200">The regulation.</response>
        /// <response code="400">Regulation not found.</response>
        [HttpGet]
        [Route("{regulationId}")]
        [SwaggerOperation("GetRegulation")]
        [ProducesResponseType(typeof(RegulationModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Get(string regulationId)
        {
            IRegulation regulation;

            try
            {
                regulation =  await _regulationService.GetAsync(regulationId);
            }
            catch (ServiceException exception)
            {
                await _log.WriteErrorAsync(nameof(RegulationController),
                    $"{nameof(regulationId)}: {regulationId}. IP: {HttpContext.GetIp()}", exception);

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            var model = Mapper.Map<RegulationModel>(regulation);

            return Ok(model);
        }

        /// <summary>
        /// Returns all regulations.
        /// </summary>
        /// <returns>The list of regulations.</returns>
        /// <response code="200">The list of regulations.</response>
        [HttpGet]
        [SwaggerOperation("GetRegulations")]
        [ProducesResponseType(typeof(IEnumerable<RegulationModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<IRegulation> regulations = await _regulationService.GetAllAsync();

            IEnumerable<RegulationModel> model =
                Mapper.Map<IEnumerable<IRegulation>, IEnumerable<RegulationModel>>(regulations);

            return Ok(model);
        }

        /// <summary>
        /// Adds the regulation.
        /// </summary>
        /// <param name="model">The model that describe a regulation.</param>
        /// <returns></returns>
        /// <response code="204">Regulation successfully added.</response>
        /// <response code="400">Invalid model that describe a regulation.</response>
        [HttpPost]
        [SwaggerOperation("AddRegulation")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Add([FromBody] NewRegulationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorResponse.Create("Invalid model.", ModelState));
            }

            var regulation = Mapper.Map<Core.Domain.Regulation>(model);

            try
            {
                await _regulationService.AddAsync(regulation);
            }
            catch (ServiceException exception)
            {
                await _log.WriteErrorAsync(nameof(RegulationController),
                    $"{nameof(model)}: {model.ToJson()}. IP: {HttpContext.GetIp()}", exception);

                return BadRequest(ErrorResponse.Create(exception.Message));
            }
            
            await _log.WriteInfoAsync(nameof(RegulationController), nameof(Add),
                $"Regulation added. {nameof(model)}: {model.ToJson()}. IP: {HttpContext.GetIp()}");

            return NoContent();
        }

        /// <summary>
        /// Updates regulation.
        /// </summary>
        /// <param name="model">The model that describe a regulation.</param>
        /// <response code="204">Regulation successfully updated.</response>
        /// <response code="400">Invalid model that describe a regulation.</response>
        [HttpPut]
        [SwaggerOperation("UpdateRegulation")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Update([FromBody] NewRegulationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorResponse.Create("Invalid model.", ModelState));
            }

            var regulation = Mapper.Map<Core.Domain.Regulation>(model);

            try
            {
                await _regulationService.UpdateAsync(regulation);
            }
            catch (ServiceException exception)
            {
                await _log.WriteWarningAsync(nameof(RegulationController), nameof(Update),
                    $"{exception.Message} {nameof(model)}: {model.ToJson()}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            await _log.WriteInfoAsync(nameof(RegulationController), nameof(Update),
                $"Regulation updated. {nameof(model)}: {model.ToJson()}. IP: {HttpContext.GetIp()}");

            return NoContent();
        }

        /// <summary>
        /// Deletes the regulation by specified id.
        /// </summary>
        /// <param name="regulationId">The id of regulation to delete.</param>
        /// <returns></returns>
        /// <response code="204">Regulation successfully deleted.</response>
        /// <response code="400">Can not delete regulation associated with client or welcome regulation rule or regulation not found.</response>
        [HttpDelete]
        [Route("{regulationId}")]
        [SwaggerOperation("DeleteRegulation")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Delete(string regulationId)
        {
            try
            {
                await _regulationService.DeleteAsync(regulationId);
            }
            catch (ServiceException exception)
            {
                await _log.WriteErrorAsync(nameof(RegulationController),
                    $"{nameof(regulationId)}: {regulationId}. IP: {HttpContext.GetIp()}", exception);

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            await _log.WriteInfoAsync(nameof(RegulationController), nameof(Delete),
                $"Regulation deleted. {nameof(regulationId)}: {regulationId}. IP: {HttpContext.GetIp()}");

            return NoContent();
        }
    }
}
