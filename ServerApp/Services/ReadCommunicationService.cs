using Shared;

namespace ServerApp.Services
{
    public class ReadCommunicationService
{
    private readonly HttpClient _client;

    public ReadCommunicationService(IHttpClientFactory factory)
    {
        _client = factory.CreateClient("ApiClient");
    }

    public async Task<List<CommunicationDto>> GetCommunicationsAsync()
    {
        return await _client.GetFromJsonAsync<List<CommunicationDto>>("/api/Communications");
    }
}

}