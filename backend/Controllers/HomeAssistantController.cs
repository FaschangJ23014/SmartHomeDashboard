using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace backend.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class HomeAssistantController : ControllerBase
{
    private readonly SmartHomeDbContext _context;
    private readonly HomeAssistantService _homeAssistantService;
    private readonly TokenProtector _tokenProtector;

    public HomeAssistantController(
        SmartHomeDbContext context,
        HomeAssistantService homeAssistantService,
        TokenProtector tokenProtector   )
    {
        _context = context;
        _homeAssistantService = homeAssistantService;
        _tokenProtector = tokenProtector;

    }

    private int GetUserId()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    [HttpGet("entities")]
    public async Task<IActionResult> GetEntities()
    {
        var userId = GetUserId();

        var config = await _context.HomeAssistantConfigs
            .FirstOrDefaultAsync(config => config.AppUserId == userId);

        if (config == null)
            return BadRequest("Home Assistant is not configured.");

        var token = _tokenProtector.Unprotect(config.TokenEncrypted);

        var entities = await _homeAssistantService.GetEntitiesAsync(config.BaseUrl, token);

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