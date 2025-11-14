using EcotrackPlatform.API.Shared.Domain.Repositories;

namespace EcotrackPlatform.API.Organization.Domain.Repositories;

public interface IOrganizationRepository : IBaseRepository<Domain.Model.Aggregates.Organization>
{
    Task<Domain.Model.Aggregates.Organization?> FindByIdAsync(int id);
}