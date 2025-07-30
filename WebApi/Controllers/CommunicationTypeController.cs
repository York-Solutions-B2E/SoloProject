using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.CommunicationDbContext;
using WebApi.Entities;

namespace WebApi.Controllers
{
[Authorize(Roles = "Admin")] // Only admins
[ApiController]
[Route("api/[controller]")]

public class CommunicationTypeController : ControllerBase
{
    private readonly AppDbContext _CommunicationDbContext;

    public CommunicationTypeController(AppDbContext appDbContext)
    {
        _CommunicationDbContext = appDbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CommunicationType>>> GetAll()
    {
        return await _CommunicationDbContext.CommunicationTypes
            .Include(t => t.Statuses)
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult> Create(CommunicationType type)
    {
        _CommunicationDbContext.CommunicationTypes.Add(type);
        await _CommunicationDbContext.SaveChangesAsync();
        return Ok(type);
    }

    [HttpDelete("{typeCode}")]
    public async Task<IActionResult> Delete(string typeCode)
    {
        var type = await _CommunicationDbContext.CommunicationTypes.FindAsync(typeCode);
        if (type is null) return NotFound();
        _CommunicationDbContext.CommunicationTypes.Remove(type);
        await _CommunicationDbContext.SaveChangesAsync();
        return NoContent();
    }
}

}