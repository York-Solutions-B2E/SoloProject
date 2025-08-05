using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.CommunicationDbContext;
using App.Shared.Dtos;
using WebApi.Entities;

namespace WebApi.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CommunicationTypeController : ControllerBase
    {
        private readonly ICommunicationTypeService _communicationTypeService;

        public CommunicationTypeController(ICommunicationTypeService service)
        {
            _communicationTypeService = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommunicationTypeDto>>> GetAll()
        {
            var types = await _communicationTypeService.GetAllAsync();
            return Ok(types);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CommunicationTypeDto type)
        {
            var created = await _communicationTypeService.CreateAsync(type);
            return Ok(created);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CommunicationTypeDto dto)
        {
            var updated = await _communicationTypeService.UpdateAsync(dto);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{typeCode}")]
        public async Task<IActionResult> Delete(string typeCode)
        {
            var success = await _communicationTypeService.DeleteAsync(typeCode);
            return success ? NoContent() : NotFound();
        }
        [HttpGet("{typeCode}/statuses")]
        public async Task<ActionResult<List<CommunicationTypeStatusDto>>> GetStatusesForType(string typeCode)
        {
            var statuses = await _communicationTypeService.GetStatusesForTypeAsync(typeCode);
            return Ok(statuses);
        }

        [HttpPut("{typeCode}/statuses")]
        public async Task<IActionResult> UpdateStatuses(string typeCode, List<CommunicationTypeStatusDto> statuses)
        {
            await _communicationTypeService.UpdateStatusesAsync(typeCode, statuses);
            return NoContent();
        }
    }

}