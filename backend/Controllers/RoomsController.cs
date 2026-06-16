using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace backend.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RoomsController : ControllerBase
{
    private readonly SmartHomeDbContext _context;

    public RoomsController(SmartHomeDbContext context)
    {
        _context = context;
    }

    private int GetUserId()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    [HttpGet]
    public async Task<IActionResult> GetRooms()
    {
        var userId = GetUserId();

        var rooms = await _context.Rooms
            .Where(room => room.AppUserId == userId)
            .ToListAsync();

        return Ok(rooms);
    }

    [HttpPost]
    public async Task<IActionResult> AddRoom(Room room)
    {
        var userId = GetUserId();

        room.Id = 0;
        room.AppUserId = userId;
        room.AppUser = null;

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();

        return Ok(room);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoom(int id)
    {
        var userId = GetUserId();

        var room = await _context.Rooms
            .FirstOrDefaultAsync(room =>
                room.Id == id &&
                room.AppUserId == userId
            );

        if (room == null)
            return NotFound();

        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}