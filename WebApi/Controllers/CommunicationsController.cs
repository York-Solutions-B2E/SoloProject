using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
using App.Shared.Dtos;

namespace WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]

    public class CommunicationsController : ControllerBase
    {
        private readonly ICommunicationService _communicationService;

        public CommunicationsController(ICommunicationService communicationService)
        {
            _communicationService = communicationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommunicationDto>>> GetAll()
        {
            var comms = await _communicationService.GetAllCommunicationsAsync();
            return Ok(comms);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CommunicationDto>> GetById(Guid id)
        {
            var comm = await _communicationService.GetCommunicationAsync(id);

            if (comm is null)
                return NotFound();

            return Ok(comm);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateCommunicationDto dto)
        {
            var id = await _communicationService.CreateCommunicationAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }
    }
}