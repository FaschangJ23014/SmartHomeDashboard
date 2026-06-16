namespace backend;

public class AppUser
{
    public int Id { get; set; }
    public string Email { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public string PasswordHash { get; set; } = "";

    public List<Room> Rooms { get; set; } = new();
    public HomeAssistantConfig? HomeAssistantConfig { get; set; }
}
