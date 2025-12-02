using System.Collections.Generic;
using System.Threading.Tasks;
using ProfileAgg = EcotrackPlatform.API.Profile.Domain.Model.Aggregates.Profile;

namespace EcotrackPlatform.API.Profile.Domain.Repositories
{
    public interface IProfileRepository
    {
        Task<IEnumerable<ProfileAgg>> ListAsync();
        Task<ProfileAgg?> FindByIdAsync(int id);
        Task<ProfileAgg?> FindByEmailAsync(string email);
        Task AddAsync(ProfileAgg profile);
        void Update(ProfileAgg profile);
        void Remove(ProfileAgg profile);
        
        
    }
}