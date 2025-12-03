using EcotrackPlatform.API.Organization.Aplication.Services;
using EcotrackPlatform.API.Organization.Domain.Model.Aggregates;
using EcotrackPlatform.API.Organization.Domain.Model.Queries;
using EcotrackPlatform.API.Organization.Domain.Repositories;

namespace EcotrackPlatform.API.Organization.Aplication.Internal.QueryServices;

public class CropQueryService(ICropRepository cropRepository) : ICropQueryService
{
    public async Task<Crop?> Handle(GetCropByIdQuery query)
    {
        return await cropRepository.FindByIdAsync(query.Id);
    }

    public async Task<IEnumerable<Crop>> Handle(GetAllCropsQuery query)
    {
        return await cropRepository.ListAsync();
    }

    public async Task<IEnumerable<Crop>> Handle(GetAllCropsByOrganizationIdQuery query)
    {
        return await cropRepository.FindByOrganizationIdAsync(query.OrganizationId);
    }
}

