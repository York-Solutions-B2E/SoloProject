namespace App.Shared.Dtos
{
    public class CommunicationTypeDto
    {
        public required string TypeCode { get; set; }
        public required string DisplayName { get; set; }
        // Each type can have a subset of global statuses
        public List<CommunicationTypeStatusDto> Statuses { get; set; } = new();
    }

}