using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HomeAssistantController : ControllerBase
{
    private readonly HomeAssistantService _homeAssistantService;

    public HomeAssistantController(HomeAssistantService homeAssistantService)
    {
        _homeAssistantService = homeAssistantService;
    }

    [HttpGet("entities")]
    public async Task<IActionResult> GetEntities()
    {
        var entities = await _homeAssistantService.GetEntitiesAsync();

        var filtered = entities
            .Where(e =>
                e.Entity_Id.StartsWith("light.") ||
                e.Entity_Id.StartsWith("switch.") ||
                e.Entity_Id.StartsWith("fan.") ||
                e.Entity_Id.StartsWith("input_boolean."))
            .Select(e => new
            {
                entityId = e.Entity_Id,
                name = string.IsNullOrWhiteSpace(e.Attributes.Friendly_Name)
                    ? e.Entity_Id
                    : e.Attributes.Friendly_Name,
                state = e.State
            })
            .ToList();

        return Ok(filtered);
    }
}