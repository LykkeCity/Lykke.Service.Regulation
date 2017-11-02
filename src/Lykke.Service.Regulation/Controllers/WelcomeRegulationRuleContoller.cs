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
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using Lykke.Service.Regulation.Services.Exceptions;

namespace Lykke.Service.Regulation.Controllers
{
    /// <summary>
    /// Provides methods for working with welcome regulation rules.
    /// </summary>
    [Route("api/[controller]")]
    public class WelcomeRegulationRuleContoller : Controller
    {
        private readonly IWelcomeRegulationRuleService _welcomeRegulationRuleService;
        private readonly ILog _log;

        public WelcomeRegulationRuleContoller(IWelcomeRegulationRuleService welcomeRegulationRuleService, ILog log)
        {
            _welcomeRegulationRuleService = welcomeRegulationRuleService;
            _log = log;
        }

        /// <summary>
        /// Returns all welcome regulation rules.
        /// </summary>
        /// <returns>The list of welcome regulation rules.</returns>
        /// <response code="200">The list of welcome regulation rules.</response>
        [HttpGet]
        [SwaggerOperation("GetWelcomeRegulationRules")]
        [ProducesResponseType(typeof(IEnumerable<WelcomeRegulationRuleModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<IWelcomeRegulationRule> regulations = await _welcomeRegulationRuleService.GetAllAsync();

            IEnumerable<WelcomeRegulationRuleModel> model =
                Mapper.Map<IEnumerable<IWelcomeRegulationRule>, IEnumerable<WelcomeRegulationRuleModel>>(regulations);

            return Ok(model);
        }

        /// <summary>
        /// Returns all welcome regulation rules associated with specified country.
        /// </summary>
        /// <param name="country">The country name.</param>
        /// <returns>The list of welcome regulation rules.</returns>
        /// <response code="200">The list of welcome regulation rules.</response>
        [HttpGet]
        [Route("country/{country}")]
        [SwaggerOperation("GetWelcomeRegulationRulesByCountry")]
        [ProducesResponseType(typeof(IEnumerable<WelcomeRegulationRuleModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetByCountry(string country)
        {
            IEnumerable<IWelcomeRegulationRule> regulations =
                await _welcomeRegulationRuleService.GetByCountryAsync(country);

            IEnumerable<WelcomeRegulationRuleModel> model =
                Mapper.Map<IEnumerable<IWelcomeRegulationRule>, IEnumerable<WelcomeRegulationRuleModel>>(regulations);

            return Ok(model);
        }

        /// <summary>
        /// Returns all welcome regulation rules associated with specified regulation.
        /// </summary>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns>The list of welcome regulation rules.</returns>
        /// <response code="200">The list of welcome regulation rules.</response>
        [HttpGet]
        [Route("regulation/{regulationId}")]
        [SwaggerOperation("GetWelcomeRegulationRulesByRegulationId")]
        [ProducesResponseType(typeof(IEnumerable<WelcomeRegulationRuleModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetByRegulationId(string regulationId)
        {
            IEnumerable<IWelcomeRegulationRule> regulations =
                await _welcomeRegulationRuleService.GetByRegulationIdAsync(regulationId);

            IEnumerable<WelcomeRegulationRuleModel> model =
                Mapper.Map<IEnumerable<IWelcomeRegulationRule>, IEnumerable<WelcomeRegulationRuleModel>>(regulations);

            return Ok(model);
        }

        /// <summary>
        /// Adds the welcome regulation rule.
        /// </summary>
        /// <param name="model">The model what describe a welcome regulation rule.</param>
        /// <response code="204">Welcome regulation rule successfully added.</response>
        /// <response code="400">Invalid model what describe a welcome regulation rule or specified regulation not found.</response>
        [HttpPost]
        [SwaggerOperation("AddWelcomeRegulationRule")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Add([FromBody] WelcomeRegulationRuleModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorResponse.Create("Invalid model.", ModelState));
            }

            var regulation = Mapper.Map<WelcomeRegulationRule>(model);
            
            try
            {
                await _welcomeRegulationRuleService.AddAsync(regulation);
            }
            catch (ServiceException exception)
            {
                await _log.WriteWarningAsync(nameof(WelcomeRegulationRuleContoller), nameof(Add),
                    $"{exception.Message} Model: {model.ToJson()}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            await _log.WriteInfoAsync(nameof(WelcomeRegulationRuleContoller), nameof(Add),
                $"Welcome regulation rule added. Model: {model.ToJson()}. IP: {HttpContext.GetIp()}");

            return Ok();
        }

        /// <summary>
        /// Updates active state of welcome regulation rule.
        /// </summary>
        /// <param name="regulationRuleId">The welcome regulation rule id.</param>
        /// <param name="active">The welcome regulation rule active state.</param>
        /// <response code="204">Welcome regulation rule active state successfully updated.</response>
        /// <response code="400">Regulation rule not found.</response>
        [HttpPut]
        [Route("{regulationRuleId}/{active}")]
        [SwaggerOperation("UpdateWelcomeRegulationRuleActive")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateActive(string regulationRuleId, bool active)
        {
            try
            {
                await _welcomeRegulationRuleService.UpdateActiveAsync(regulationRuleId, active);
            }
            catch (ServiceException exception)
            {
                await _log.WriteWarningAsync(nameof(WelcomeRegulationRuleContoller), nameof(UpdateActive),
                    $"{exception.Message} RegulationRuleId: {regulationRuleId}. Active: {active}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            await _log.WriteInfoAsync(nameof(WelcomeRegulationRuleContoller), nameof(UpdateActive),
                $"Welcome regulation rule active state updated. RegulationRuleId: {regulationRuleId}. Active: {active}. IP: {HttpContext.GetIp()}");

            return NoContent();
        }

        /// <summary>
        /// Deletes the welcome regulation rule by specified regulation id.
        /// </summary>
        /// <param name="regulationId">The regulation id associated with welcome regulation rule.</param>
        /// <response code="204">Welcome regulation rule successfully deleted.</response>
        [HttpDelete]
        [Route("{regulationId}")]
        [SwaggerOperation("DeleteWelcomeRegulationRule")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete(string regulationId)
        {
            await _welcomeRegulationRuleService.DeleteAsync(regulationId);

            await _log.WriteInfoAsync(nameof(WelcomeRegulationRuleContoller), nameof(Delete),
                $"Welcome regulation rule deleted. Id: {regulationId}. IP: {HttpContext.GetIp()}");

            return Ok();
        }
    }
}
