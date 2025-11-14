using EcotrackPlatform.API.Organization.Domain.Model.Commands;
using EcotrackPlatform.API.Organization.Interfaces.REST.Resources;

namespace EcotrackPlatform.API.Organization.Interfaces.REST.Transform;

public static class CreateCropCommandFromResourceAssembler
{
    public static CreateCropCommand ToCommand(CreateCropResource resource) =>
        new(resource.Name, resource.Description, resource.OrganizationId);
}