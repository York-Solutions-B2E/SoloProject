using App.Shared.Dtos;


namespace ServerApp.Services
{
    public class CreateReadCommunicationService
{
    private readonly HttpClient _client;

    public CreateReadCommunicationService(IHttpClientFactory factory)
    {
        _client = factory.CreateClient("ApiClient"); // must match exactly the name used in AddHttpClient
    }
    
    public async Task<bool> CreateCommunicationAsync(CreateCommunicationDto dto)
    {
        var response = await _client.PostAsJsonAsync("api/communications", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<List<CommunicationDto>> GetAllAsync()
    {
        var response = await _client.GetFromJsonAsync<List<CommunicationDto>>("api/communications/all");
        if (response == null)
            throw new Exception("Failed to retrieve communications from API.");
        return response;
    }
    public async Task<PaginatedResult<CommunicationDto>> GetCommunicationsAsync(int pageNumber, int pageSize)
        {
            var response = await _client.GetFromJsonAsync<PaginatedResult<CommunicationDto>>(
                $"api/communications?pageNumber={pageNumber}&pageSize={pageSize}");

            if (response == null)
                throw new Exception("Failed to retrieve communications from API.");

            return response;
        }

  }

}



