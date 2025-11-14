using System.Threading.Tasks;
using ProfileSettingsAgg = EcotrackPlatform.API.Profile.Domain.Model.Aggregates.ProfileSettings;

namespace EcotrackPlatform.API.Profile.Domain.Repositories
{
    public interface IProfileSettingsRepository
    {
        Task<ProfileSettingsAgg?> FindByProfileIdAsync(int profileId);
        Task AddAsync(ProfileSettingsAgg settings);
        void Update(ProfileSettingsAgg settings);
    }
}