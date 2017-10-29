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

        [HttpGet]
        [Route("regulation/{regulationId}")]
        [SwaggerOperation("GetWelcomeRegulationRulesByCountry")]
        [ProducesResponseType(typeof(IEnumerable<WelcomeRegulationRuleModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetByRegulationId(string regulationId)
        {
            IEnumerable<IWelcomeRegulationRule> regulations =
                await _welcomeRegulationRuleService.GetByRegulationIdAsync(regulationId);

            IEnumerable<WelcomeRegulationRuleModel> model =
                Mapper.Map<IEnumerable<IWelcomeRegulationRule>, IEnumerable<WelcomeRegulationRuleModel>>(regulations);

            return Ok(model);
        }

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

        [HttpDelete]
        [Route("{id}")]
        [SwaggerOperation("DeleteWelcomeRegulationRule")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Delete(string id)
        {
            await _welcomeRegulationRuleService.DeleteAsync(id);

            await _log.WriteInfoAsync(nameof(WelcomeRegulationRuleContoller), nameof(Delete),
                $"Welcome regulation rule deleted. Id: {id}. IP: {HttpContext.GetIp()}");

            return Ok();
        }
    }
}
