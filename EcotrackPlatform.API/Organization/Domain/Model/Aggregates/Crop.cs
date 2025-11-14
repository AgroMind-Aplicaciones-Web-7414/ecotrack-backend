namespace EcotrackPlatform.API.Organization.Domain.Model.Aggregates;

public class Crop
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }

    public int OrganizationId { get; set; }
    public DateTime CreatedAt { get; set; }
}