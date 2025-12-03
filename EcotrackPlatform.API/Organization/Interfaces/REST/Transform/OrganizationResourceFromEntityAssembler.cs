using EcotrackPlatform.API.Organization.Interfaces.REST.Resources;

namespace EcotrackPlatform.API.Organization.Interfaces.REST.Transform;

public static class OrganizationResourceFromEntityAssembler
{
    public static OrganizationResource ToResource(Domain.Model.Aggregates.Organization entity)
        => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Status = entity.Status,
            CreatedAt = entity.CreatedAt
        };
}