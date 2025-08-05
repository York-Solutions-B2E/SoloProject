namespace WebApi.Entities
{
    //One to many relationship with Status
    public class CommunicationType
    {
        public Guid Id { get; set; }  // new PK
        public string TypeCode { get; set; } = null!;
        public string DisplayName { get; set; } = null!;

        public ICollection<Communication> Communications { get; set; } = new List<Communication>();
        public ICollection<CommunicationTypeStatus> Statuses { get; set; } = new List<CommunicationTypeStatus>();
    }
}