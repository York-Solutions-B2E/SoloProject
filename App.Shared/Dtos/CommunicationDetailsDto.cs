namespace App.Shared.Dtos
{
    public class CommunicationDetailsDto
{
    public string Id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string TypeCode { get; set; } = null!;
    public string CurrentStatusCode { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public string? SourceFileUrl { get; set; }

    public List<CommunicationStatusHistoryDto> StatusHistory { get; set; } = new();
}
}