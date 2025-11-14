using EcotrackPlatform.API.Profile.Interfaces.REST.Resources;
using EcotrackPlatform.API.Profile.Domain.Model.Commands;

namespace EcotrackPlatform.API.Profile.Interfaces.REST.Transform
{
    public static class CreateProfileCommandFromResourceAssembler
    {
        public static CreateProfileCommand ToCommand(CreateProfileResource r)
            => new(r.Email, r.DisplayName, r.Password);
    }
}