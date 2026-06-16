using Microsoft.EntityFrameworkCore;

namespace backend;

public class SmartHomeDbContext : DbContext
{
    public SmartHomeDbContext(DbContextOptions<SmartHomeDbContext> options)
        : base(options)
    {
    }

    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Device> Devices => Set<Device>();
    public DbSet<AppUser> AppUsers => Set<AppUser>();
    public DbSet<HomeAssistantConfig> HomeAssistantConfigs => Set<HomeAssistantConfig>();
}