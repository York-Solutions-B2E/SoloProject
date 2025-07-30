using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.CommunicationDbContext;
using WebApi.Entities;

namespace WebApi.Controllers
{
    [Authorize(Roles = "User")]
    [ApiController]
    [Route("api/[controller]")]
    public class CommunicationsController : ControllerBase
    {
        private readonly AppDbContext _CommunicationDbContext;

        public CommunicationsController(AppDbContext appDbContext)
        {
            _CommunicationDbContext = appDbContext;

        }
        //communications page load all info
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Communication>>> GetAll()
        {
            return await _CommunicationDbContext.Communications
                .Include(c => c.CommunicationType)
                .Include(c => c.StatusHistory)
                .ToListAsync();
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Communication>> GetById(Guid id)
        {
            var comm = await _CommunicationDbContext.Communications
                .Include(c => c.CommunicationType)
                .Include(c => c.StatusHistory.OrderBy(h => h.OccurredUtc))
                .FirstOrDefaultAsync(c => c.Id == id);

            return comm is null ? NotFound() : Ok(comm);
        }
        [HttpPost]
        public async Task<ActionResult> Create(Communication comm)
        {
            comm.LastUpdatedUtc = DateTime.UtcNow;
            _CommunicationDbContext.Communications.Add(comm);
            await _CommunicationDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = comm.Id }, comm);
        }

        

    }
}