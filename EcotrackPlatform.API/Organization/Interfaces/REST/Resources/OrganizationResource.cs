namespace EcotrackPlatform.API.Organization.Interfaces.REST.Resources;

public class OrganizationResource
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
}