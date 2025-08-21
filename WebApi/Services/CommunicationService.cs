// CommunicationService.cs
using App.Shared.Dtos;

namespace WebApi.Services
{
    public class CommunicationService : ICommunicationService
    {
        private readonly ICommunicationRepository _repo;

        public CommunicationService(ICommunicationRepository repo)
        {
            _repo = repo;
        }

        public Task<CommunicationDetailsDto?> GetCommunicationDetailsAsync(Guid communicationId)
            => _repo.GetCommunicationDetailsAsync(communicationId);

        public Task<Guid> CreateCommunicationAsync(CreateCommunicationDto dto)
            => _repo.CreateCommunicationAsync(dto);

        public IQueryable<CommunicationDto> GetPaginatedCommunicationsAsync()
            => _repo.GetPaginatedCommunicationsAsync();

        public Task<CommunicationDto?> GetCommunicationAsync(Guid id)
            => _repo.GetCommunicationAsync(id);

        public Task<List<CommunicationDto>> GetAllAsync()
            => _repo.GetAllAsync();
    }
}
