using App.Shared.Dtos;

namespace ServerApp.Services
{
    public class CommunicationDetailsService
{
    private readonly HttpClient _client;

    public CommunicationDetailsService(IHttpClientFactory factory)
    {
        _client = factory.CreateClient("ApiClient");
    }

    public async Task<CommunicationDetailsDto?> GetCommunicationDetailsAsync(string communicationId)
    {
        return await _client.GetFromJsonAsync<CommunicationDetailsDto>($"api/communications/{communicationId}");
    }
}

}