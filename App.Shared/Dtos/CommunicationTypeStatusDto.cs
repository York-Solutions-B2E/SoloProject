namespace App.Shared.Dtos
{
    public class CommunicationTypeStatusDto
    {


        public required string TypeCode { get; set; }
        public required string StatusCode { get; set; }
        public required string Description { get; set; }
        public bool IsValid { get; set; } 
    }

}