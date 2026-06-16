using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace backend;

public class HomeAssistantService
{
    private readonly HttpClient _httpClient;

    public HomeAssistantService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> TurnOnAsync(
        string baseUrl,
        string token,
        string entityId)
    {
        return await CallService(baseUrl, token, entityId, "turn_on");
    }

    public async Task<bool> TurnOffAsync(
        string baseUrl,
        string token,
        string entityId)
    {
        return await CallService(baseUrl, token, entityId, "turn_off");
    }

    private async Task<bool> CallService(
        string baseUrl,
        string token,
        string entityId,
        string service)
    {
        var domain = entityId.Split('.')[0];

        var url = $"{baseUrl.TrimEnd('/')}/api/services/{domain}/{service}";

        var request = new HttpRequestMessage(HttpMethod.Post, url);

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var json = JsonSerializer.Serialize(new
        {
            entity_id = entityId
        });

        request.Content = new StringContent(
            json,
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.SendAsync(request);

        return response.IsSuccessStatusCode;
    }

    public async Task<List<HomeAssistantEntityDto>> GetEntitiesAsync(
        string baseUrl,
        string token)
    {
        var url = $"{baseUrl.TrimEnd('/')}/api/states";

        var request = new HttpRequestMessage(HttpMethod.Get, url);

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            return new List<HomeAssistantEntityDto>();

        var json = await response.Content.ReadAsStringAsync();

        var entities = JsonSerializer.Deserialize<List<HomeAssistantEntityDto>>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return entities ?? new List<HomeAssistantEntityDto>();
    }
}