using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Common.Log;
using Lykke.Service.Regulation.Core.Services;
using Lykke.Service.Regulation.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using Lykke.Service.Regulation.Core.Domain;
using Lykke.Service.Regulation.Extensions;
using Lykke.Service.Regulation.Services.Exceptions;

namespace Lykke.Service.Regulation.Controllers
{
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

        [HttpGet]
        [Route("{regulationId}")]
        [SwaggerOperation("GetRegulationById")]
        [ProducesResponseType(typeof(RegulationModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(string regulationId)
        {
            IRegulation regulation = await _regulationService.GetAsync(regulationId);

            if (regulation == null)
            {
                await _log.WriteWarningAsync(nameof(RegulationController), nameof(Get),
                    $"Regulation not found. RegulationId: ${regulationId}. IP: {HttpContext.GetIp()}");

                return NotFound(ErrorResponse.Create("Regulation not found"));
            }

            var model = Mapper.Map<RegulationModel>(regulation);

            return Ok(model);
        }

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

        [HttpPost]
        [SwaggerOperation("AddRegulation")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task AddRegulation([FromBody] RegulationModel model)
        {
            var regulation = Mapper.Map<Core.Domain.Regulation>(model);

            await _regulationService.AddAsync(regulation);

            await _log.WriteInfoAsync(nameof(RegulationController), nameof(AddRegulation),
                $"Regulation added. Model: ${model.ToJson()}. IP: {HttpContext.GetIp()}");
        }

        [HttpDelete]
        [Route("{regulationId}")]
        [SwaggerOperation("RemoveRegulation")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Remove(string regulationId)
        {
            try
            {
                await _regulationService.RemoveAsync(regulationId);
            }
            catch (ServiceException exception)
            {
                await _log.WriteWarningAsync(nameof(RegulationController), nameof(Remove),
                    $"{exception.Message} RegulationId: {regulationId}. IP: {HttpContext.GetIp()}");

                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            await _log.WriteInfoAsync(nameof(RegulationController), nameof(Remove),
                $"Regulation removed. RegulationId: {regulationId}. IP: {HttpContext.GetIp()}");

            return Ok();
        }

        [HttpPut]
        [SwaggerOperation("UpdateRegulation")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task Update([FromBody] RegulationModel model)
        {
            var regulation = Mapper.Map<Core.Domain.Regulation>(model);
            await _regulationService.UpdateAsync(regulation);
        }
    }
}
