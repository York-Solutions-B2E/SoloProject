using App.Shared.Dtos;
namespace WebApi.Services
{
    public interface ICommunicationService
{
    Task<Guid> CreateCommunicationAsync(CreateCommunicationDto dto);
    Task<IEnumerable<CommunicationDto>> GetAllCommunicationsAsync();

    Task<CommunicationDto?> GetCommunicationAsync(Guid id);
}
}