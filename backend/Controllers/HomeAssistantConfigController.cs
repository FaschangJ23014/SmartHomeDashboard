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
    private readonly TokenProtector _tokenProtector;

    public HomeAssistantConfigController(SmartHomeDbContext context, TokenProtector tokenProtector)
    {
        _context = context;
        _tokenProtector = tokenProtector;

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
                TokenEncrypted = _tokenProtector.Protect(request.Token.Trim())
            };

            _context.HomeAssistantConfigs.Add(config);
        }
        else
        {
            config.BaseUrl = request.BaseUrl.Trim();
            config.TokenEncrypted = _tokenProtector.Protect(request.Token.Trim());
        }

        await _context.SaveChangesAsync();

        return Ok(new
        {
            config.BaseUrl,
            hasToken = true
        });
    }

    [HttpPost("test")]
    public async Task<IActionResult> Test(SaveHomeAssistantConfigRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.BaseUrl))
            return BadRequest("Home Assistant URL is required.");

        if (string.IsNullOrWhiteSpace(request.Token))
            return BadRequest("Home Assistant token is required.");

        using var httpClient = new HttpClient();

        var url = $"{request.BaseUrl.TrimEnd('/')}/api/states";

        var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
        httpRequest.Headers.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer",
                request.Token
            );

        try
        {
            var response = await httpClient.SendAsync(httpRequest);

            if (!response.IsSuccessStatusCode)
                return BadRequest("Connection failed.");

            return Ok("Connection successful.");
        }
        catch
        {
            return BadRequest("Connection failed.");
        }
    }
}