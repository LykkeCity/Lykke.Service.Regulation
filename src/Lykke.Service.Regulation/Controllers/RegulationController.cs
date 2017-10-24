using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Lykke.Service.Regulation.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;

namespace Lykke.Service.Regulation.Controllers
{
    [Route("api/[controller]")]
    public class RegulationController : Controller
    {
        [HttpPost]
        [SwaggerOperation("AddNewRegulation")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public Task<IActionResult> AddRegulation([FromBody] RegulationDto regulation)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [SwaggerOperation("GetAllRegulation")]
        [ProducesResponseType(typeof(IEnumerable<RegulationDto>), (int)HttpStatusCode.OK)]
        public Task<IActionResult> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        [SwaggerOperation("GetRegulationById")]
        [ProducesResponseType(typeof(RegulationDto), (int)HttpStatusCode.OK)]
        public Task<IActionResult> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation("RemoveRegulationById")]
        [ProducesResponseType(typeof(IEnumerable<RegulationDto>), (int)HttpStatusCode.OK)]
        public Task RemoveAsync(string id)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        [SwaggerOperation("UpdateRegulation")]
        [ProducesResponseType(typeof(IEnumerable<RegulationDto>), (int)HttpStatusCode.OK)]
        public Task UpdateAsync(RegulationDto asset)
        {
            throw new NotImplementedException();
        }
    }
}
