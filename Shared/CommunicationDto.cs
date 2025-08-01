namespace Shared
{
    public class CommunicationDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string TypeCode { get; set; } 
        public required string CurrentStatus { get; set; } 
        public DateTime LastUpdatedUtc { get; set; }
    }

}