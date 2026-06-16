namespace backend;

public class Device
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";
    public bool IsOn { get; set; }

    public int RoomId { get; set; }
    public Room? Room { get; set; }

    //Für IoT Entwicklung
    public string IntegrationType { get; set; } = "Simulation";
    public string? ExternalId { get; set; }

    public int AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}
