using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common;
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
    /// Provides methods for working with margin regulation rules.
    /// </summary>
    [Route("api/[controller]")]
    public class MarginRegulationRuleController : Controller
    {
        private readonly IMarginRegulationRuleService _marginRegulationRuleService;
        private readonly ILog _log;

        public MarginRegulationRuleController(
            IMarginRegulationRuleService marginRegulationRuleService,
            ILogFactory logFactory)
        {
            _marginRegulationRuleService = marginRegulationRuleService;
            _log = logFactory.CreateLog(this);
        }

        /// <summary>
        /// Returns margin regulation rule by specified id.
        /// </summary>
        /// <returns>The margin regulation rule.</returns>
        /// <response code="200">The margin regulation rule.</response>
        /// <response code="400">Margin regulation rule with specified id not found.</response>
        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation("MarginRegulationRuleGetById")]
        [ProducesResponseType(typeof(MarginRegulationRuleModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            IMarginRegulationRule regulationRule;

            try
            {
                regulationRule = await _marginRegulationRuleService.GetAsync(id);
            }
            catch (ServiceException exception)
            {
                _log.Error(nameof(GetByIdAsync), exception, $"{nameof(id)}: {id}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            var model = Mapper.Map<MarginRegulationRuleModel>(regulationRule);

            return Ok(model);
        }

        /// <summary>
        /// Returns all margin regulation rules.
        /// </summary>
        /// <returns>The collection of margin regulation rules.</returns>
        /// <response code="200">The collection of margin regulation rules.</response>
        [HttpGet]
        [SwaggerOperation("MarginRegulationRuleGetAll")]
        [ProducesResponseType(typeof(IEnumerable<MarginRegulationRuleModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllAsync()
        {
            IEnumerable<IMarginRegulationRule> regulationRules = await _marginRegulationRuleService.GetAllAsync();

            var model = Mapper.Map<List<MarginRegulationRuleModel>>(regulationRules);

            return Ok(model);
        }

        /// <summary>
        /// Adds a margin regulation rule.
        /// </summary>
        /// <param name="model">The model what describe a margin regulation rule.</param>
        /// <response code="204">The margin regulation rule successfully added.</response>
        /// <response code="400">Invalid model what describe a margin regulation rule or specified regulation not found.</response>
        [HttpPost]
        [SwaggerOperation("MarginRegulationRuleAdd")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddAsync([FromBody] NewMarginRegulationRuleModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorResponse.Create("Invalid model.", ModelState));
            }

            var regulationRule = Mapper.Map<MarginRegulationRule>(model);

            try
            {
                await _marginRegulationRuleService.AddAsync(regulationRule);
            }
            catch (ServiceException exception)
            {
                _log.Error(nameof(AddAsync), exception, $"{nameof(model)}: {model.ToJson()}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            _log.Info(nameof(AddAsync), $"Margin regulation rule added. {nameof(model)}: {model.ToJson()}. IP: {HttpContext.GetIp()}", model.RegulationId);

            return NoContent();
        }

        /// <summary>
        /// Updates a margin regulation rule.
        /// </summary>
        /// <param name="model">The model what describes a margin regulation rule.</param>
        /// <response code="204">The margin regulation rule successfully updated.</response>
        /// <response code="400">Invalid model what describes a margin regulation rule or regulation rule not found or specified regulation not found.</response>
        [HttpPut]
        [SwaggerOperation("MarginRegulationRuleUpdate")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateAsync([FromBody] MarginRegulationRuleModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorResponse.Create("Invalid model.", ModelState));
            }

            var regulationRule = Mapper.Map<MarginRegulationRule>(model);

            try
            {
                await _marginRegulationRuleService.UpdateAsync(regulationRule);
            }
            catch (ServiceException exception)
            {
                _log.Error(nameof(UpdateAsync), exception, $"{nameof(model)}: {model.ToJson()}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            _log.Info(nameof(UpdateAsync), $"Margin regulation rule updated. {nameof(model)}: {model.ToJson()}. IP: {HttpContext.GetIp()}", model.RegulationId);

            return NoContent();
        }

        /// <summary>
        /// Deletes the margin regulation rule by specified id.
        /// </summary>
        /// <param name="id">The margin regulation rule id.</param>
        /// <response code="204">The margin regulation rule successfully deleted.</response>
        /// <response code="400">The margin regulation rule not found.</response>
        [HttpDelete]
        [Route("{id}")]
        [SwaggerOperation("MarginRegulationRuleDelete")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            try
            {
                await _marginRegulationRuleService.DeleteAsync(id);
            }
            catch (ServiceException exception)
            {
                _log.Error(nameof(DeleteAsync), exception, $"{nameof(id)}: {id}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            _log.Info(nameof(DeleteAsync), $"Margin regulation rule deleted. IP: {HttpContext.GetIp()}", id);

            return NoContent();
        }
    }
}
