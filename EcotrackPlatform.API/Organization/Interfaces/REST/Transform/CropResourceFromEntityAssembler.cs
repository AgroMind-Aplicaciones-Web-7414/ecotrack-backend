using EcotrackPlatform.API.Organization.Domain.Model.Aggregates;
using EcotrackPlatform.API.Organization.Interfaces.REST.Resources;

namespace EcotrackPlatform.API.Organization.Interfaces.REST.Transform;

public static class CropResourceFromEntityAssembler
{
    public static CropResource ToResource(Crop entity)
        => new CropResource
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            CreatedAt = entity.CreatedAt
        };
}