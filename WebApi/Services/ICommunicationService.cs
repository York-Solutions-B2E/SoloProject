using App.Shared.Dtos;
namespace WebApi.Services
{
    public interface ICommunicationService
    {
        Task<Guid> CreateCommunicationAsync(CreateCommunicationDto dto);
        IQueryable<CommunicationDto> GetPaginatedCommunicationsAsync();
        Task<CommunicationDetailsDto?> GetCommunicationDetailsAsync(Guid communicationId);
        Task<CommunicationDto?> GetCommunicationAsync(Guid id);
        Task<List<CommunicationDto>> GetAllAsync();
}
}