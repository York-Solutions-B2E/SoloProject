namespace WebApi.Entities
{
    //One to many relationship with Status
    public class CommunicationType
    {
        public required string TypeCode { get; set; }
        public required string DisplayName { get; set; }
        public List<Communication> Communications { get; set; } = [];
        public List<CommunicationTypeStatus> Statuses { get; set; } = []; //navigation for using FKs
    }
}