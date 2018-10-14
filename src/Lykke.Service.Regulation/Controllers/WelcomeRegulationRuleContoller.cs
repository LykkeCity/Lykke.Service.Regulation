using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Common.Log;
using Lykke.Service.Regulation.Core.Domain;
using Lykke.Service.Regulation.Core.Services;
using Lykke.Common.Extensions;
using Lykke.Common.Log;
using Lykke.Service.Regulation.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using Lykke.Service.Regulation.Core.Exceptions;

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

        public WelcomeRegulationRuleContoller(IWelcomeRegulationRuleService welcomeRegulationRuleService, ILogFactory logFactory)
        {
            _welcomeRegulationRuleService = welcomeRegulationRuleService;
            _log = logFactory.CreateLog(this);
        }

        /// <summary>
        /// Returns welcome regulation rule by specified id.
        /// </summary>
        /// <returns>The welcome regulation rule.</returns>
        /// <response code="200">The welcome regulation rule.</response>
        /// <response code="400">Regulation rule with specified id not found.</response>
        [HttpGet]
        [Route("{welcomeRegulationRuleId}")]
        [SwaggerOperation("WelcomeRegulationRuleGetById")]
        [ProducesResponseType(typeof(WelcomeRegulationRuleModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetById(string welcomeRegulationRuleId)
        {
            IWelcomeRegulationRule regulationRule;

            try
            {
                regulationRule = await _welcomeRegulationRuleService.GetAsync(welcomeRegulationRuleId);
            }
            catch (ServiceException exception)
            {
                _log.Error(nameof(GetById), exception, $"{nameof(welcomeRegulationRuleId)}: {welcomeRegulationRuleId}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            WelcomeRegulationRuleModel model =
                Mapper.Map<IWelcomeRegulationRule, WelcomeRegulationRuleModel>(regulationRule);

            return Ok(model);
        }

        /// <summary>
        /// Returns all welcome regulation rules.
        /// </summary>
        /// <returns>The list of welcome regulation rules.</returns>
        /// <response code="200">The list of welcome regulation rules.</response>
        [HttpGet]
        [SwaggerOperation("WelcomeRegulationRuleGetAll")]
        [ProducesResponseType(typeof(IEnumerable<WelcomeRegulationRuleModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<IWelcomeRegulationRule> regulationRules = await _welcomeRegulationRuleService.GetAllAsync();

            IEnumerable<WelcomeRegulationRuleModel> model =
                Mapper.Map<IEnumerable<IWelcomeRegulationRule>, IEnumerable<WelcomeRegulationRuleModel>>(regulationRules);

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
        [SwaggerOperation("WelcomeRegulationRuleGetByCountry")]
        [ProducesResponseType(typeof(IEnumerable<WelcomeRegulationRuleModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetByCountry(string country)
        {
            IEnumerable<IWelcomeRegulationRule> regulationRules =
                await _welcomeRegulationRuleService.GetByCountryAsync(country);

            IEnumerable<WelcomeRegulationRuleModel> model =
                Mapper.Map<IEnumerable<IWelcomeRegulationRule>, IEnumerable<WelcomeRegulationRuleModel>>(regulationRules);

            return Ok(model);
        }

        /// <summary>
        /// Returns all welcome regulation rules associated with specified regulation.
        /// </summary>
        /// <param name="regulationId">The regulation id.</param>
        /// <returns>The list of welcome regulation rules.</returns>
        /// <response code="200">The list of welcome regulation rules.</response>
        /// <response code="400">Regulation with specified id not found.</response>
        [HttpGet]
        [Route("regulation/{regulationId}")]
        [SwaggerOperation("WelcomeRegulationRuleGetByRegulationId")]
        [ProducesResponseType(typeof(IEnumerable<WelcomeRegulationRuleModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetByRegulationId(string regulationId)
        {
            IEnumerable<IWelcomeRegulationRule> regulationRules;

            try
            {
                regulationRules = await _welcomeRegulationRuleService.GetByRegulationIdAsync(regulationId);
            }
            catch (ServiceException exception)
            {
                _log.Error(nameof(GetByRegulationId), exception, $"{nameof(regulationId)}: {regulationId}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            IEnumerable<WelcomeRegulationRuleModel> model =
                Mapper.Map<IEnumerable<IWelcomeRegulationRule>, IEnumerable<WelcomeRegulationRuleModel>>(regulationRules);

            return Ok(model);
        }

        /// <summary>
        /// Adds the welcome regulation rule.
        /// </summary>
        /// <param name="model">The model that describe a welcome regulation rule.</param>
        /// <response code="204">Welcome regulation rule successfully added.</response>
        /// <response code="400">Invalid model that describe a welcome regulation rule or specified regulation not found.</response>
        [HttpPost]
        [SwaggerOperation("WelcomeRegulationRuleAdd")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Add([FromBody] NewWelcomeRegulationRuleModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorResponse.Create("Invalid model.", ModelState));
            }

            var regulationRule = Mapper.Map<WelcomeRegulationRule>(model);

            try
            {
                await _welcomeRegulationRuleService.AddAsync(regulationRule);
            }
            catch (ServiceException exception)
            {
                _log.Error(nameof(Add), exception, $"{nameof(model)}: {model.ToJson()}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            _log.Info(nameof(Add), $"Welcome regulation rule added. {nameof(model)}: {model.ToJson()}. IP: {HttpContext.GetIp()}",
                model.RegulationId);

            return NoContent();
        }

        /// <summary>
        /// Updates welcome regulation rule.
        /// </summary>
        /// <param name="model">The model that describe a welcome regulation rule.</param>
        /// <response code="204">Welcome regulation rule active state successfully updated.</response>
        /// <response code="400">Invalid model that describe a welcome regulation rule or regulation rule not found or specified regulation not found.</response>
        [HttpPut]
        [Route("update")]
        [SwaggerOperation("WelcomeRegulationRuleUpdate")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Update([FromBody] WelcomeRegulationRuleModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorResponse.Create("Invalid model.", ModelState));
            }

            var regulationRule = Mapper.Map<WelcomeRegulationRule>(model);

            try
            {
                await _welcomeRegulationRuleService.UpdateAsync(regulationRule);
            }
            catch (ServiceException exception)
            {
                _log.Error(nameof(Update), exception, $"{nameof(model)}: {model.ToJson()}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            _log.Info(nameof(Update), $"Welcome regulation rule updated. {nameof(model)}: {model.ToJson()}. IP: {HttpContext.GetIp()}",
                model.RegulationId);

            return NoContent();
        }

        /// <summary>
        /// Deletes the welcome regulation rule by specified regulation id.
        /// </summary>
        /// <param name="regulationRuleId">The welcome regulation rule id.</param>
        /// <response code="204">Welcome regulation rule successfully deleted.</response>
        /// <response code="400">Regulation rule not found.</response>
        [HttpDelete]
        [Route("{regulationRuleId}")]
        [SwaggerOperation("WelcomeRegulationRuleDelete")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Delete(string regulationRuleId)
        {
            try
            {
                await _welcomeRegulationRuleService.DeleteAsync(regulationRuleId);
            }
            catch (ServiceException exception)
            {
                _log.Error(nameof(Delete), exception, $"{nameof(regulationRuleId)}: {regulationRuleId}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }
            
            _log.Info(nameof(Delete), $"Welcome regulation rule deleted. IP: {HttpContext.GetIp()}", regulationRuleId);

            return NoContent();
        }
    }
}
