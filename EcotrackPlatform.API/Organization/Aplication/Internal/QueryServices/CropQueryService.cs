using EcotrackPlatform.API.Organization.Aplication.Services;
using EcotrackPlatform.API.Organization.Domain.Model.Aggregates;
using EcotrackPlatform.API.Organization.Domain.Repositories;
using EcotrackPlatform.API.Organization.Domain.Model.Queries;

namespace EcotrackPlatform.API.Organization.Aplication.Internal.QueryServices;

public class CropQueryService(ICropRepository repository) : ICropQueryService
{
    public async Task<IEnumerable<Crop>> Handle(GetAllCropsByOrganizationIdQuery query)
        => await repository.FindByOrganizationIdAsync(query.OrganizationId);
}