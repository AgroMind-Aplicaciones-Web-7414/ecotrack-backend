namespace EcotrackPlatform.API.Profile.Domain.Services
{
    public interface IProfileDomainService
    {
        bool IsValidEmail(string email);
    }

    public class ProfileDomainService : IProfileDomainService
    {
        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            // Validación simple; puedes refinar con Regex si quieres
            return email.Contains('@') && email.Contains('.');
        }
    }
}