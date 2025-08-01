using App.Shared.Dtos;

namespace ServerApp.Services
{
    public class ReadCommunicationService
{
    private readonly HttpClient _client;

    public ReadCommunicationService(IHttpClientFactory factory)
    {
        _client = factory.CreateClient("ApiClient"); // must match exactly the name used in AddHttpClient
    }

    public async Task<List<CommunicationDto>> GetCommunicationsAsync()
    {
        Console.WriteLine("Calling API with access token...");  // add this to see if this is called
        return await _client.GetFromJsonAsync<List<CommunicationDto>>("/api/communications");
    }
}



}