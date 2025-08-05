// Services/ICommunicationTypeService.cs
using App.Shared.Dtos;


public interface ICommunicationTypeService
{
    Task<IEnumerable<CommunicationTypeDto>> GetAllAsync();
    Task<CommunicationTypeDto?> CreateAsync(CommunicationTypeDto dto);

    Task<bool> UpdateAsync(CommunicationTypeDto dto);
    Task<bool> DeleteAsync(string typeCode);

    Task<List<CommunicationTypeStatusDto>> GetStatusesForTypeAsync(string typeCode);
    Task<bool> UpdateStatusesAsync(string typeCode, List<CommunicationTypeStatusDto> statuses);
}
