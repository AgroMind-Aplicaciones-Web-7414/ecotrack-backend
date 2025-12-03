using EcotrackPlatform.API.Organization.Domain.Model.Aggregates;
using EcotrackPlatform.API.Organization.Domain.Repositories;
using EcotrackPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EcotrackPlatform.API.Organization.Infrastructure.Repositories;

public class CropRepository(AppDbContext context) : ICropRepository
{
    public async Task AddAsync(Crop entity) =>
        await context.Crops.AddAsync(entity);

    public async Task<Crop?> FindByIdAsync(int id) =>
        await context.Crops.FindAsync(id);

    public async Task<IEnumerable<Crop>> ListAsync() =>
        await context.Crops.ToListAsync();

    public async Task<IEnumerable<Crop>> FindByOrganizationIdAsync(int organizationId) =>
        await context.Crops.Where(c => c.OrganizationId == organizationId).ToListAsync();

    public void Remove(Crop entity) => context.Crops.Remove(entity);

    public void Update(Crop entity) => context.Crops.Update(entity);
}