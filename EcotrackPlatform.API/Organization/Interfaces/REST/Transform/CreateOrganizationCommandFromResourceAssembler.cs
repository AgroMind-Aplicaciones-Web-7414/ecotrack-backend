using EcotrackPlatform.API.Organization.Domain.Model.Commands;
using EcotrackPlatform.API.Organization.Interfaces.REST.Resources;

namespace EcotrackPlatform.API.Organization.Interfaces.REST.Transform;

public static class CreateOrganizationCommandFromResourceAssembler
{
    public static CreateOrganizationCommand ToCommand(CreateOrganizationResource resource)
        => new(
            resource.Name,
            resource.Description,
            resource.Status
        );
}
