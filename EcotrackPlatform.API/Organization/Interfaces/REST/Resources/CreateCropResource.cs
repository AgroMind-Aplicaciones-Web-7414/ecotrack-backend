namespace EcotrackPlatform.API.Organization.Interfaces.REST.Resources;

public class CreateCropResource
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public int OrganizationId { get; set; }
}