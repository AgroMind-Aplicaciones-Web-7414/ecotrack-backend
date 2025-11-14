using EcotrackPlatform.API.Profile.Interfaces.REST.Resources;
using EcotrackPlatform.API.Profile.Domain.Model.Commands;

namespace EcotrackPlatform.API.Profile.Interfaces.REST.Transform
{
    public static class UpdateProfileCommandFromResourceAssembler
    {
        public static UpdateProfileCommand ToCommand(int id, UpdateProfileResource r)
            => new(id, r.DisplayName, r.Email);
    }
}