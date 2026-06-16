using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace backend.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class HomeAssistantConfigController : ControllerBase
{
    private readonly SmartHomeDbContext _context;

    public HomeAssistantConfigController(SmartHomeDbContext context)
    {
        _context = context;
    }

    private int GetUserId()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var userId = GetUserId();

        var config = await _context.HomeAssistantConfigs
            .FirstOrDefaultAsync(config => config.AppUserId == userId);

        if (config == null)
            return NotFound();

        return Ok(new
        {
            config.BaseUrl,
            hasToken = !string.IsNullOrWhiteSpace(config.TokenEncrypted)
        });
    }

    [HttpPost]
    public async Task<IActionResult> Save(SaveHomeAssistantConfigRequest request)
    {
        var userId = GetUserId();

        if (string.IsNullOrWhiteSpace(request.BaseUrl))
            return BadRequest("Home Assistant URL is required.");

        if (string.IsNullOrWhiteSpace(request.Token))
            return BadRequest("Home Assistant token is required.");

        var config = await _context.HomeAssistantConfigs
            .FirstOrDefaultAsync(config => config.AppUserId == userId);

        if (config == null)
        {
            config = new HomeAssistantConfig
            {
                AppUserId = userId,
                BaseUrl = request.BaseUrl.Trim(),
                TokenEncrypted = request.Token.Trim()
            };

            _context.HomeAssistantConfigs.Add(config);
        }
        else
        {
            config.BaseUrl = request.BaseUrl.Trim();
            config.TokenEncrypted = request.Token.Trim();
        }

        await _context.SaveChangesAsync();

        return Ok(new
        {
            config.BaseUrl,
            hasToken = true
        });
    }
}