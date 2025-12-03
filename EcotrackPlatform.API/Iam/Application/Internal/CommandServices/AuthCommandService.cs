using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using EcotrackPlatform.API.Profile.Domain.Repositories;
using EcotrackPlatform.API.Iam.Domain.Model.Aggregates;
using EcotrackPlatform.API.Iam.Domain.Repositories;
using EcotrackPlatform.API.Shared.Domain.Repositories;
// Alias para el agregado Profile
using ProfileAgg = EcotrackPlatform.API.Profile.Domain.Model.Aggregates.Profile;

namespace EcotrackPlatform.API.Iam.Application.Internal.CommandServices
{
    public class AuthCommandService
    {
        private readonly IProfileRepository _profiles;
        private readonly IAuthSessionRepository _sessions;
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _config;

        // hasher tipado al alias
        private readonly PasswordHasher<ProfileAgg> _hasher = new();

        public AuthCommandService(
            IProfileRepository profiles,
            IAuthSessionRepository sessions,
            IUnitOfWork uow,
            IConfiguration config)
        {
            _profiles = profiles; _sessions = sessions; _uow = uow; _config = config;
        }

        public async Task<ProfileAgg?> RegisterAsync(string email, string password, string displayName)
        {
            // Validar que el email no esté en uso
            var existingUser = await _profiles.FindByEmailAsync(email);
            if (existingUser is not null) return null; // Email ya registrado

            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(displayName))
                return null;

            if (password.Length < 6) return null; // Password muy corto

            // Hashear la contraseña
            var tempProfile = new ProfileAgg(email, displayName, "temp");
            var passwordHash = _hasher.HashPassword(tempProfile, password);

            // Crear el perfil con el hash real
            var profile = new ProfileAgg(email, displayName, passwordHash);
            await _profiles.AddAsync(profile);
            await _uow.CompleteAsync();
            
            return profile;
        }

        public async Task<AuthSession?> LoginAsync(string email, string password, string? ua, string? ip)
        {
            var user = await _profiles.FindByEmailAsync(email);
            if (user is null) return null;

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (result == PasswordVerificationResult.Failed) return null;

            var hours = _config.GetValue<int?>("Session:TtlHours") ?? 8;
            var session = new AuthSession(user.Id, TimeSpan.FromHours(hours), ua, ip);
            await _sessions.AddAsync(session);
            await _uow.CompleteAsync();
            return session;
        }

        public async Task<bool> LogoutAsync(Guid sessionId)
        {
            var s = await _sessions.FindByIdAsync(sessionId);
            if (s is null || !s.IsActive()) return false;
            s.Revoke();
            _sessions.Update(s);
            await _uow.CompleteAsync();
            return true;
        }
    }
}
