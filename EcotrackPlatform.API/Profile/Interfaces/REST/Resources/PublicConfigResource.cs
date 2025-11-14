namespace EcotrackPlatform.API.Profile.Interfaces.REST.Resources
{
    public record PublicConfigResource(bool RegistrationEnabled, string[] Locales, string[] Themes);
}