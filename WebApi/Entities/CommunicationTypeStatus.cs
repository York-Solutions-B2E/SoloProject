namespace WebApi.Entities 
{
    public class CommunicationTypeStatus
    {
        public Guid CommunicationTypeId { get; set; } // FK
        public required string StatusCode { get; set; }
        public required string Description { get; set; }
        public CommunicationType? CommunicationType { get; set; }
    }
}