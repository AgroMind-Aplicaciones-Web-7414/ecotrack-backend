namespace EcotrackPlatform.API.Organization.Domain.Model.Commands;

public record CreateCropCommand(
    string Name,
    string? Description,
    int OrganizationId
);