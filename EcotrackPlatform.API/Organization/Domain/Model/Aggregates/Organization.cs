namespace EcotrackPlatform.API.Organization.Domain.Model.Aggregates;

public class Organization(string name, string description, string status)
{
    public int Id { get; private set; }
    public string Name { get; private set; } = name;
    public string Description { get; private set; } = description;
    public string Status { get; private set; } = status;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
}