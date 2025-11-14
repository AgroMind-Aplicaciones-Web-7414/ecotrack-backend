using System.Threading.Tasks;
using EcotrackPlatform.API.Shared.Domain.Repositories;
using EcotrackPlatform.API.Profile.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

// Alias para evitar colisión con el namespace Profile
using ProfileAgg = EcotrackPlatform.API.Profile.Domain.Model.Aggregates.Profile;

namespace EcotrackPlatform.API.Profile.Application.Internal.CommandServices
{
    public class ProfileCommandService
    {
        private readonly IProfileRepository _profiles;
        private readonly IUnitOfWork _uow;
        private readonly PasswordHasher<ProfileAgg> _hasher = new();

        public ProfileCommandService(IProfileRepository profiles, IUnitOfWork uow)
        {
            _profiles = profiles;
            _uow = uow;
        }

        public async Task<ProfileAgg> CreateAsync(string email, string displayName, string plainPassword)
        {
            var existing = await _profiles.FindByEmailAsync(email);
            if (existing is not null) throw new InvalidOperationException("Email already in use.");

            var tmp = new ProfileAgg(email, displayName, ""); // se setea hash abajo
            var hash = _hasher.HashPassword(tmp, plainPassword);
            tmp.SetPasswordHash(hash);

            await _profiles.AddAsync(tmp);
            await _uow.CompleteAsync();
            return tmp;
        }

        public async Task<ProfileAgg?> UpdateAsync(int id, string? displayName = null, string? email = null)
        {
            var entity = await _profiles.FindByIdAsync(id);
            if (entity is null) return null;

            if (!string.IsNullOrWhiteSpace(displayName))
                entity.Rename(displayName);

            if (!string.IsNullOrWhiteSpace(email))
            {
                var exists = await _profiles.FindByEmailAsync(email);
                if (exists is not null && exists.Id != id) throw new InvalidOperationException("Email already in use.");
                // pequeño “setter” directo; si prefieres, agrega método en aggregate
                typeof(ProfileAgg).GetProperty("Email")!.SetValue(entity, email);
            }

            _profiles.Update(entity);
            await _uow.CompleteAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _profiles.FindByIdAsync(id);
            if (entity is null) return false;
            _profiles.Remove(entity);
            await _uow.CompleteAsync();
            return true;
        }

        public async Task<bool> ChangePasswordAsync(int id, string currentPassword, string newPassword)
        {
            var entity = await _profiles.FindByIdAsync(id);
            if (entity is null) return false;

            var result = _hasher.VerifyHashedPassword(entity, entity.PasswordHash, currentPassword);
            if (result == PasswordVerificationResult.Failed) return false;

            var newHash = _hasher.HashPassword(entity, newPassword);
            entity.SetPasswordHash(newHash);
            _profiles.Update(entity);
            await _uow.CompleteAsync();
            return true;
        }
    }
}
