using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EcotrackPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using EcotrackPlatform.API.Profile.Domain.Repositories;
// Alias para agregado
using ProfileSettingsAgg = EcotrackPlatform.API.Profile.Domain.Model.Aggregates.ProfileSettings;

namespace EcotrackPlatform.API.Profile.Infrastructure.Repositories
{
    public class ProfileSettingsRepository : IProfileSettingsRepository
    {
        private readonly AppDbContext _ctx;
        public ProfileSettingsRepository(AppDbContext ctx) => _ctx = ctx;

        public async Task<ProfileSettingsAgg?> FindByProfileIdAsync(int profileId) =>
            await _ctx.Set<ProfileSettingsAgg>().AsNoTracking().FirstOrDefaultAsync(s => s.ProfileId == profileId);

        public async Task AddAsync(ProfileSettingsAgg settings) =>
            await _ctx.Set<ProfileSettingsAgg>().AddAsync(settings);

        public void Update(ProfileSettingsAgg settings) =>
            _ctx.Set<ProfileSettingsAgg>().Update(settings);
    }
}