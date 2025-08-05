using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
using App.Shared.Dtos;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]

    public class CommunicationsController : ControllerBase
    {
        private readonly ICommunicationService _communicationService;

        public CommunicationsController(ICommunicationService communicationService)
        {
            _communicationService = communicationService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CommunicationDetailsDto>> GetDetails(Guid id)
        {
            var details = await _communicationService.GetCommunicationDetailsAsync(id);
            if (details == null)
                return NotFound();

            return Ok(details);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommunicationDto>>> GetAll([FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10)
        {
            var comms = await _communicationService.GetPaginatedCommunicationsAsync(pageNumber, pageSize);
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