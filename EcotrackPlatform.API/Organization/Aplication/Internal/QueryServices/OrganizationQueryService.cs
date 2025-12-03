using EcotrackPlatform.API.Organization.Aplication.Services;
using EcotrackPlatform.API.Organization.Domain.Model.Queries;
using EcotrackPlatform.API.Organization.Domain.Repositories;

namespace EcotrackPlatform.API.Organization.Aplication.Internal.QueryServices;

public class OrganizationQueryService(IOrganizationRepository organizationRepository) : IOrganizationQueryService
{
    public async Task<Domain.Model.Aggregates.Organization?> Handle(GetOrganizationByIdQuery query)
    {
        return await organizationRepository.FindByIdAsync(query.Id);
    }

    public async Task<IEnumerable<Domain.Model.Aggregates.Organization>> Handle(GetAllOrganizationsQuery query)
    {
        return await organizationRepository.ListAsync();
    }
}

