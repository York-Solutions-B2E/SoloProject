namespace App.Shared.Dtos
{
    public class CreateCommunicationDto
    {

        public required string Title { get; set; }
        public required string TypeCode { get; set; }
        public required string InitialStatusCode { get; set; }
    }

}