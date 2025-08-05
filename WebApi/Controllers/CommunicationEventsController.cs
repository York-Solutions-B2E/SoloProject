using App.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommunicationEventsController : ControllerBase
    {
        private readonly CommunicationEventPublisher _publisher;

        public CommunicationEventsController(CommunicationEventPublisher publisher)
        {
        _publisher = publisher;
        }

        [HttpPost]
        public async Task<IActionResult> PublishEvent([FromBody] CommunicationEventDto dto)
        {
            await _publisher.PublishAsync(dto);
            return Ok(new { message = "Event published." });
        }
}

}