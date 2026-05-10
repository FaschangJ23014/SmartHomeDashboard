using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DevicesController : ControllerBase
{
    private readonly SmartHomeDbContext _context;
    private readonly HomeAssistantService _homeAssistantService;

    public DevicesController(SmartHomeDbContext context, HomeAssistantService homeAssistantService)
    {
        _context = context;
        _homeAssistantService = homeAssistantService;
    }

    [HttpGet]
    public async Task<IActionResult> GetDevices()
    {
        var devices = await _context.Devices
            .Include(d => d.Room)
            .ToListAsync();

        return Ok(devices);
    }

    [HttpPost("{id}/toggle")]
    public async Task<IActionResult> ToggleDevice(int id)
    {
        var device = await _context.Devices.FindAsync(id);

        if (device == null)
            return NotFound();

        if (device.IntegrationType == "HomeAssistant")
        {
            if (string.IsNullOrWhiteSpace(device.ExternalId))
                return BadRequest("Home Assistant entity id is missing.");

            bool success;

            if (device.IsOn)
            {
                success = await _homeAssistantService.TurnOffAsync(device.ExternalId);
                device.IsOn = false;
            }
            else
            {
                success = await _homeAssistantService.TurnOnAsync(device.ExternalId);
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
        var roomExists = await _context.Rooms.AnyAsync(r => r.Id == device.RoomId);

        if (!roomExists)
            return BadRequest("Room does not exist.");

        device.Room = null;

        _context.Devices.Add(device);
        await _context.SaveChangesAsync();

        var createdDevice = await _context.Devices
            .Include(d => d.Room)
            .FirstOrDefaultAsync(d => d.Id == device.Id);

        return Ok(createdDevice);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDevice(int id)
    {
        var device = await _context.Devices.FindAsync(id);

        if (device == null)
            return NotFound();

        _context.Devices.Remove(device);
        await _context.SaveChangesAsync();

        return NoContent();
    }


}
