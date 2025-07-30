namespace WebApi.Entities 
{
    public class CommunicationTypeStatus
    {
        public required string TypeCode { get; set; } //FK to CommunicationType
        public required string StatusCode { get; set; }
        public required string Description { get; set; }

        public required CommunicationType CommunicationType { get; set; }
    }
}