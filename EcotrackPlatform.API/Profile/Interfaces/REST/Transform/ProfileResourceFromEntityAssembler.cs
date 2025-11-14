using EcotrackPlatform.API.Profile.Interfaces.REST.Resources;
using ProfileAgg = EcotrackPlatform.API.Profile.Domain.Model.Aggregates.Profile;

namespace EcotrackPlatform.API.Profile.Interfaces.REST.Transform
{
    public static class ProfileResourceFromEntityAssembler
    {
        public static ProfileResource ToResource(ProfileAgg e)
            => new(e.Id, e.Email, e.DisplayName);
    }
}