using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.CommunicationDbContext;
using App.Shared.Dtos;
using WebApi.Entities;

namespace WebApi.Controllers
{

    [Authorize(Roles = "Admin")] // Only admins
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
        [HttpPut("{typeCode}")]
        public async Task<ActionResult> Update(string typeCode, CommunicationTypeDto updatedType)
        {
            if (typeCode != updatedType.TypeCode)
            {
            return BadRequest("Type code in URL and body do not match.");
            }

            var success = await _communicationTypeService.UpdateAsync(updatedType);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{typeCode}")]
        public async Task<IActionResult> Delete(string typeCode)
        {
            var success = await _communicationTypeService.DeleteAsync(typeCode);
            return success ? NoContent() : NotFound();
        }
    }

}