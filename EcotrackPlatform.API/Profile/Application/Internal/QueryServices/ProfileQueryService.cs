using System.Collections.Generic;
using System.Threading.Tasks;
using EcotrackPlatform.API.Profile.Domain.Repositories;

// Alias para agregado
using ProfileAgg = EcotrackPlatform.API.Profile.Domain.Model.Aggregates.Profile;

namespace EcotrackPlatform.API.Profile.Application.Internal.QueryServices
{
    public class ProfileQueryService
    {
        private readonly IProfileRepository _profiles;

        public ProfileQueryService(IProfileRepository profiles)
        {
            _profiles = profiles;
        }

        public Task<IEnumerable<ProfileAgg>> ListAsync() => _profiles.ListAsync();

        public Task<ProfileAgg?> FindByIdAsync(int id) => _profiles.FindByIdAsync(id);

        public Task<ProfileAgg?> FindByEmailAsync(string email) => _profiles.FindByEmailAsync(email);

        /// <summary>
        /// “Me”: el controller te pasará el `currentProfileId` resuelto desde la cookie `sid`.
        /// </summary>
        public Task<ProfileAgg?> GetCurrentAsync(int currentProfileId) => _profiles.FindByIdAsync(currentProfileId);
    }
}