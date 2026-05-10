namespace backend;

public class HomeAssistantEntityDto
{
    public string Entity_Id { get; set; } = "";
    public string State { get; set; } = "";
    public HomeAssistantAttributes Attributes { get; set; } = new();
}

public class HomeAssistantAttributes
{
    public string Friendly_Name { get; set; } = "";
}