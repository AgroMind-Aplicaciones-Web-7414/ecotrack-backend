using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EcotrackPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using EcotrackPlatform.API.Profile.Domain.Repositories;
// Alias para evitar colisión con namespace Profile
using ProfileAgg = EcotrackPlatform.API.Profile.Domain.Model.Aggregates.Profile;

namespace EcotrackPlatform.API.Profile.Infrastructure.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly AppDbContext _ctx;
        public ProfileRepository(AppDbContext ctx) => _ctx = ctx;

        public async Task<IEnumerable<ProfileAgg>> ListAsync() =>
            await _ctx.Set<ProfileAgg>().AsNoTracking().ToListAsync();

        public async Task<ProfileAgg?> FindByIdAsync(int id) =>
            await _ctx.Set<ProfileAgg>().FirstOrDefaultAsync(p => p.Id == id);

        public async Task<ProfileAgg?> FindByEmailAsync(string email) =>
            await _ctx.Set<ProfileAgg>().FirstOrDefaultAsync(p => p.Email == email);

        public async Task AddAsync(ProfileAgg profile) => await _ctx.Set<ProfileAgg>().AddAsync(profile);

        public void Update(ProfileAgg profile) => _ctx.Set<ProfileAgg>().Update(profile);

        public void Remove(ProfileAgg profile) => _ctx.Set<ProfileAgg>().Remove(profile);
    }
}