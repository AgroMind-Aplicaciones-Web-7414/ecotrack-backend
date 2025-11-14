using System.Threading.Tasks;
using EcotrackPlatform.API.Shared.Domain.Repositories;
using EcotrackPlatform.API.Profile.Domain.Repositories;

// Alias de agregados
using ProfileSettingsAgg = EcotrackPlatform.API.Profile.Domain.Model.Aggregates.ProfileSettings;

namespace EcotrackPlatform.API.Profile.Application.Internal.CommandServices
{
    public class SettingsCommandService
    {
        private readonly IProfileSettingsRepository _settings;
        private readonly IUnitOfWork _uow;

        public SettingsCommandService(IProfileSettingsRepository settings, IUnitOfWork uow)
        {
            _settings = settings;
            _uow = uow;
        }

        /// <summary>
        /// Crea o actualiza settings del perfil (upsert liviano).
        /// </summary>
        public async Task<ProfileSettingsAgg> UpsertAsync(
            int profileId,
            bool? notificationsEmail = null,
            string? locale = null,
            string? theme = null)
        {
            var s = await _settings.FindByProfileIdAsync(profileId);
            if (s is null)
            {
                s = new ProfileSettingsAgg(profileId, notificationsEmail ?? true, locale ?? "en", theme ?? "light");
                await _settings.AddAsync(s);
            }
            else
            {
                // setters simples (ajústalos si tu aggregate expone métodos específicos)
                if (notificationsEmail is not null)
                    typeof(ProfileSettingsAgg).GetProperty("NotificationsEmail")!.SetValue(s, notificationsEmail.Value);
                if (!string.IsNullOrWhiteSpace(locale))
                    typeof(ProfileSettingsAgg).GetProperty("Locale")!.SetValue(s, locale);
                if (!string.IsNullOrWhiteSpace(theme))
                    typeof(ProfileSettingsAgg).GetProperty("Theme")!.SetValue(s, theme);

                _settings.Update(s);
            }

            await _uow.CompleteAsync();
            return s;
        }
    }
}