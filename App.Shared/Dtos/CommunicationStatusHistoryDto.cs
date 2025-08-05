namespace App.Shared.Dtos
{
    public class CommunicationStatusHistoryDto
    {
        
        public required string StatusCode { get; set; }
        public DateTime OccurredUtc { get; set; }
    }

}