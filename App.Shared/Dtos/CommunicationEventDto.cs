namespace App.Shared.Dtos
{
    public class CommunicationEventDto
    {
        public Guid CommunicationId { get; set; }
        public string EventCode { get; set; } = null!;
        public DateTime PublishedAt { get; set; }
    }


}