using System.Threading.Tasks;
using EcotrackPlatform.API.Profile.Domain.Repositories;

// Alias para agregado
using ProfileSettingsAgg = EcotrackPlatform.API.Profile.Domain.Model.Aggregates.ProfileSettings;

namespace EcotrackPlatform.API.Profile.Application.Internal.QueryServices
{
    public class SettingsQueryService
    {
        private readonly IProfileSettingsRepository _settings;

        public SettingsQueryService(IProfileSettingsRepository settings)
        {
            _settings = settings;
        }

        public Task<ProfileSettingsAgg?> GetByProfileIdAsync(int profileId) =>
            _settings.FindByProfileIdAsync(profileId);
    }
}