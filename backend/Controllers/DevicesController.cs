using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace backend.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DevicesController : ControllerBase
{
    private readonly SmartHomeDbContext _context;
    private readonly HomeAssistantService _homeAssistantService;

    public DevicesController(
        SmartHomeDbContext context,
        HomeAssistantService homeAssistantService
    )
    {
        _context = context;
        _homeAssistantService = homeAssistantService;
    }

    private int GetUserId()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    [HttpGet]
    public async Task<IActionResult> GetDevices()
    {
        var userId = GetUserId();

        var devices = await _context.Devices
            .Include(device => device.Room)
            .Where(device => device.AppUserId == userId)
            .ToListAsync();

        return Ok(devices);
    }

    [HttpPost("{id}/toggle")]
    public async Task<IActionResult> ToggleDevice(int id)
    {
        var userId = GetUserId();

        var device = await _context.Devices
            .FirstOrDefaultAsync(device =>
                device.Id == id &&
                device.AppUserId == userId
            );

        if (device == null)
            return NotFound();

        if (device.IntegrationType == "HomeAssistant")
        {
            if (string.IsNullOrWhiteSpace(device.ExternalId))
                return BadRequest("Home Assistant entity id is missing.");

            var config = await _context.HomeAssistantConfigs
                .FirstOrDefaultAsync(config => config.AppUserId == userId);

            if (config == null)
                return BadRequest("Home Assistant is not configured.");

            bool success;

            if (device.IsOn)
            {
                success = await _homeAssistantService.TurnOffAsync(
                    config.BaseUrl,
                    config.TokenEncrypted,
                    device.ExternalId
                );

                if (success)
                    device.IsOn = false;
            }
            else
            {
                success = await _homeAssistantService.TurnOnAsync(
                    config.BaseUrl,
                    config.TokenEncrypted,
                    device.ExternalId
                );

                if (success)
                    device.IsOn = true;
            }

            if (!success)
                return StatusCode(500, "Home Assistant request failed.");
        }
        else
        {
            device.IsOn = !device.IsOn;
        }

        await _context.SaveChangesAsync();

        return Ok(device);
    }

    [HttpPost]
    public async Task<IActionResult> AddDevice(Device device)
    {
        var userId = GetUserId();

        var roomExists = await _context.Rooms
            .AnyAsync(room =>
                room.Id == device.RoomId &&
                room.AppUserId == userId
            );

        if (!roomExists)
            return BadRequest("Room does not exist.");

        device.Id = 0;
        device.AppUserId = userId;
        device.AppUser = null;
        device.Room = null;

        _context.Devices.Add(device);
        await _context.SaveChangesAsync();

        var createdDeviceId = device.Id;

        var createdDevice = await _context.Devices
            .Include(d => d.Room)
            .FirstOrDefaultAsync(d =>
                d.Id == createdDeviceId &&
                d.AppUserId == userId
            );

        return Ok(createdDevice);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDevice(int id)
    {
        var userId = GetUserId();

        var device = await _context.Devices
            .FirstOrDefaultAsync(device =>
                device.Id == id &&
                device.AppUserId == userId
            );

        if (device == null)
            return NotFound();

        _context.Devices.Remove(device);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}