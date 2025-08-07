namespace WebApi.Entities
{
    public class CommunicationStatusHistory
    {
        public int Id { get; set; } // PK
        public Guid CommunicationId { get; set; } // FK to communication
        public required Communication Communication { get; set; }// Navigation for using FKs
        public required string StatusCode { get; set; }
        public DateTime OccurredUtc { get; set; }
    }
}