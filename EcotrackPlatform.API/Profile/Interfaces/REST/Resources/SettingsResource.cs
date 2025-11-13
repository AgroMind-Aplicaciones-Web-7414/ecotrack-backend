namespace EcotrackPlatform.API.Profile.Interfaces.REST.Resources
{
    public record SettingsResource(bool NotificationsEmail, string Locale, string Theme);
}