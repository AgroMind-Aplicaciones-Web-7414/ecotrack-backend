using EcotrackPlatform.API.Organization.Domain.Repositories;
using EcotrackPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using EcotrackPlatform.API.Shared.Infrastructure.Persistence.Repositories;

namespace EcotrackPlatform.API.Organization.Infrastructure.Repositories;

public class OrganizationRepository(AppDbContext context)
    : BaseRepository<Domain.Model.Aggregates.Organization>(context), IOrganizationRepository;