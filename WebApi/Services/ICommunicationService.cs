using App.Shared.Dtos;
namespace WebApi.Services
{
    public interface ICommunicationService
{
    Task<Guid> CreateCommunicationAsync(CreateCommunicationDto dto);
    Task<PaginatedResult<CommunicationDto>> GetPaginatedCommunicationsAsync(int pageNumber, int pageSize);


    Task<CommunicationDto?> GetCommunicationAsync(Guid id);
}
}