namespace App.Shared.Dtos
{
    public class CommunicationDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public Guid CommunicationTypeId { get; set; }  // FK to CommunicationType.Id
        public required string TypeCode { get; set; }
        public required string CurrentStatus { get; set; }
        public DateTime LastUpdatedUtc { get; set; }
        
        //for event simulator
        public List<string> AllowedStatusCodes { get; set; } = new();
    }

}