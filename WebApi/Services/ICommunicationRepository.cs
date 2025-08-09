using App.Shared.Dtos;

namespace WebApi.Services
{
    public interface ICommunicationRepository
    {
        Task<CommunicationDetailsDto?> GetCommunicationDetailsAsync(Guid communicationId);
        Task<Guid> CreateCommunicationAsync(CreateCommunicationDto dto);
        Task<PaginatedResult<CommunicationDto>> GetPaginatedCommunicationsAsync(int pageNumber, int pageSize);
        Task<CommunicationDto?> GetCommunicationAsync(Guid id);
        Task<List<CommunicationDto>> GetAllAsync();
    }
}
