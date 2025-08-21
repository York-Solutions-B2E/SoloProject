using WebApi.CommunicationDbContext;

namespace WebApi.Entities
{
    [Node]
    public class Communication
    {
        public Guid Id { get; set; } // PK
        public required string Title { get; set; }
        public Guid CommunicationTypeId { get; set; } // FK to CommunicationType
        public required string CurrentStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedUtc { get; set; }
        public string? SourceFileUrl { get; set; }
        public CommunicationType? CommunicationType { get; set; }
        public List<CommunicationStatusHistory> StatusHistory { get; set; } = new List<CommunicationStatusHistory>();

        [NodeResolver]
        public static  Communication GetCommunicationAsync(Guid id, [Service] AppDbContext db)
        {
        return db.Communications.First(c => c.Id == id);
        }

        
    }
}