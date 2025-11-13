namespace EcotrackPlatform.API.Profile.Domain.Model.Commands
{
    public record CreateProfileCommand(string Email, string DisplayName, string Password);
}