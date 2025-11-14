namespace EcotrackPlatform.API.Profile.Domain.Model.Aggregates
{
    public class ProfileSettings
    {
        public int Id { get; private set; }
        public int ProfileId { get; private set; }

        public bool NotificationsEmail { get; private set; }
        public string Locale { get; private set; } = "en";
        public string Theme { get; private set; } = "light";

        protected ProfileSettings() { } // EF

        public ProfileSettings(int profileId, bool notificationsEmail, string locale, string theme)
        {
            if (profileId <= 0) throw new ArgumentException("Invalid profile id.");
            ProfileId = profileId;
            SetNotificationsEmail(notificationsEmail);
            SetLocale(locale);
            SetTheme(theme);
        }

        public void SetNotificationsEmail(bool enabled) => NotificationsEmail = enabled;

        public void SetLocale(string locale)
        {
            if (string.IsNullOrWhiteSpace(locale)) throw new ArgumentException("Locale is required.");
            Locale = locale.Trim().ToLowerInvariant(); // ej. en, es, pt-br
        }

        public void SetTheme(string theme)
        {
            if (string.IsNullOrWhiteSpace(theme)) throw new ArgumentException("Theme is required.");
            Theme = theme.Trim().ToLowerInvariant();    // ej. light, dark
        }
    }
}