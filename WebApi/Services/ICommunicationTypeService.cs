// Services/ICommunicationTypeService.cs
using App.Shared.Dtos;


public interface ICommunicationTypeService
{
    Task<IEnumerable<CommunicationTypeDto>> GetAllAsync();
    Task<CommunicationTypeDto?> CreateAsync(CommunicationTypeDto dto);

    Task<bool> UpdateAsync(CommunicationTypeDto dto);
    Task<bool> DeleteAsync(Guid id);

    Task<List<CommunicationTypeStatusDto>> GetStatusesForTypeAsync(Guid id);
    Task<bool> UpdateStatusesAsync(Guid id, List<CommunicationTypeStatusDto> statuses);
}
