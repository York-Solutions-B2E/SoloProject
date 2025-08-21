using App.Shared.Dtos;

namespace WebApi.Services
{
    public interface ICommunicationRepository
    {
        Task<CommunicationDetailsDto?> GetCommunicationDetailsAsync(Guid communicationId);
        Task<Guid> CreateCommunicationAsync(CreateCommunicationDto dto);
        IQueryable<CommunicationDto> GetPaginatedCommunicationsAsync();
        Task<CommunicationDto?> GetCommunicationAsync(Guid id);
        Task<List<CommunicationDto>> GetAllAsync();
    }
}
