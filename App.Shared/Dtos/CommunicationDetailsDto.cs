namespace App.Shared.Dtos
{
    public class CommunicationDetailsDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    
    public Guid CommunicationTypeId { get; set; }  // FK to CommunicationType.Id
    public required string TypeCode { get; set; } 
    public string CurrentStatusCode { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    
    public DateTime LastUpdatedUtc { get; set; }
    public string? SourceFileUrl { get; set; }

    public List<CommunicationStatusHistoryDto> StatusHistory { get; set; } = new();
}
}