namespace backend;

public class HomeAssistantConfig
{
    public int Id { get; set; }
    public string BaseUrl { get; set; } = "";
    public string TokenEncrypted { get; set; } = "";

    public int AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}
