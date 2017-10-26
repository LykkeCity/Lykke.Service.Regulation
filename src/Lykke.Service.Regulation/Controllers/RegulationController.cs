using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.Regulation.Core.Services;
using Lykke.Service.Regulation.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using Lykke.Service.Regulation.Core.Domain;

namespace Lykke.Service.Regulation.Controllers
{
    [Route("api/[controller]")]
    public class RegulationController : Controller
    {
        private readonly IRegulationService _regulationService;

        public RegulationController(IRegulationService regulationService)
        {
            _regulationService = regulationService;
        }

        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation("Get")]
        [ProducesResponseType(typeof(RegulationModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAsync(string id)
        {
            IRegulation regulation = await _regulationService.GetAsync(id);

            if (regulation == null)
            {
                return NotFound(ErrorResponse.Create("Regulation not found"));
            }

            var model = Mapper.Map<RegulationModel>(regulation);

            return Ok(model);
        }

        [HttpGet]
        [SwaggerOperation("GetAll")]
        [ProducesResponseType(typeof(IEnumerable<RegulationModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllAsync()
        {
            IEnumerable<IRegulation> regulations = await _regulationService.GetAllAsync();

            IEnumerable<RegulationModel> model =
                Mapper.Map<IEnumerable<IRegulation>, IEnumerable<RegulationModel>>(regulations);

            return Ok(model);
        }

        [HttpPost]
        [SwaggerOperation("Add")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task AddRegulation([FromBody] RegulationModel regulation)
        {
            var model = Mapper.Map<Core.Domain.Regulation>(regulation);
            await _regulationService.AddAsync(model);
        }

        [HttpDelete]
        [Route("{id}")]
        [SwaggerOperation("Remove")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveAsync(string id)
        {
            // TODO: Make custom exception
            try
            {
                await _regulationService.RemoveAsync(id);
            }
            catch (Exception exception)
            {
                return BadRequest(ErrorResponse.Create(exception.Message));
            }

            return Ok();
        }

        [HttpPut]
        [SwaggerOperation("Update")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task UpdateAsync([FromBody] RegulationModel regulation)
        {
            var model = Mapper.Map<Core.Domain.Regulation>(regulation);

            await _regulationService.UpdateAsync(model);
        }
    }
}
