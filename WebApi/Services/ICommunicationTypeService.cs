// Services/ICommunicationTypeService.cs
using WebApi.Dtos;

public interface ICommunicationTypeService
{
    Task<IEnumerable<CommunicationTypeDto>> GetAllAsync();
    Task<CommunicationTypeDto?> CreateAsync(CommunicationTypeDto dto);
    Task<bool> DeleteAsync(string typeCode);
}
