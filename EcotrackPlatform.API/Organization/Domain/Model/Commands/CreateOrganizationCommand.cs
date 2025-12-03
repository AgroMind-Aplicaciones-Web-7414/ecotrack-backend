namespace EcotrackPlatform.API.Organization.Domain.Model.Commands;

public record CreateOrganizationCommand(
    string Name,
    string Description,
    string Status
);

