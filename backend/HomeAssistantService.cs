using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace backend;

public class HomeAssistantService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public HomeAssistantService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;

        var token = _configuration["HomeAssistant:Token"];
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<bool> TurnOnAsync(string entityId)
    {
        return await CallService(entityId, "turn_on");
    }

    public async Task<bool> TurnOffAsync(string entityId)
    {
        return await CallService(entityId, "turn_off");
    }

    private async Task<bool> CallService(string entityId, string service)
    {
        var baseUrl = _configuration["HomeAssistant:BaseUrl"];

        var domain = entityId.Split('.')[0];

        var url = $"{baseUrl}/api/services/{domain}/{service}";

        var json = JsonSerializer.Serialize(new
        {
            entity_id = entityId
        });

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(url, content);

        return response.IsSuccessStatusCode;
    }

    public async Task<List<HomeAssistantEntityDto>> GetEntitiesAsync()
    {
        var baseUrl = _configuration["HomeAssistant:BaseUrl"];

        var response = await _httpClient.GetAsync($"{baseUrl}/api/states");

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