using System.Net.Http.Json;
using App.Shared.Dtos;
namespace ServerApp.Services
{


    public class CRUDCommunicationTypeService
    {
        private readonly HttpClient _http;

        public CRUDCommunicationTypeService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient"); // must match exactly the name used in AddHttpClient
        }

        public async Task<List<CommunicationTypeDto>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<CommunicationTypeDto>>("api/communicationtype")
                   ?? new List<CommunicationTypeDto>();
        }

        public async Task<bool> CreateAsync(CommunicationTypeDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/communicationtype", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(CommunicationTypeDto dto)
        {
            var response = await _http.PutAsJsonAsync("api/communicationtype", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(string typeCode)
        {
            var response = await _http.DeleteAsync($"api/communicationtype/{typeCode}");
            return response.IsSuccessStatusCode;
        }
        
        public async Task<List<CommunicationTypeStatusDto>> GetStatusesForTypeAsync(string typeCode)
        {
            return await _http.GetFromJsonAsync<List<CommunicationTypeStatusDto>>($"api/communicationtype/{typeCode}/statuses")
                ?? new List<CommunicationTypeStatusDto>();
        }

        public async Task<bool> UpdateStatusesAsync(string typeCode, List<CommunicationTypeStatusDto> statuses)
        {
            var response = await _http.PutAsJsonAsync($"api/communicationtype/{typeCode}/statuses", statuses);
            return response.IsSuccessStatusCode;
        }

    }

}