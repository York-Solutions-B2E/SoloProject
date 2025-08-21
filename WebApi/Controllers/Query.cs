using System.Data;
using App.Shared.Dtos;
using HotChocolate.Authorization;
using WebApi.Services;

namespace WebApi.Controllers
{
    public class Query
    {
        [Authorize] // require auth for testing
        public string Health() => "pong";

        [Authorize(Roles = new[] { "Admin" })] // example protected field
        public string Secret() => "shhh";

        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<CommunicationDto> Communications(
            [Service] ICommunicationService communicationService)
        {
            return communicationService.GetPaginatedCommunicationsAsync();
        }
        // Node resolver for CommunicationDto
        
        public async Task<CommunicationDto?> GetCommunicationByIdAsync(Guid id, [Service] ICommunicationService service)
        {
            return await service.GetCommunicationAsync(id);
        }

        
        

        
        
            
    }
}