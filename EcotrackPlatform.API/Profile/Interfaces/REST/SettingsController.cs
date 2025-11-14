using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

using EcotrackPlatform.API.Profile.Application.Internal.QueryServices;
using EcotrackPlatform.API.Profile.Application.Internal.CommandServices;
using EcotrackPlatform.API.Profile.Interfaces.REST.Resources;
using EcotrackPlatform.API.Iam.Domain.Repositories;

namespace EcotrackPlatform.API.Profile.Interfaces.REST
{
    [ApiController]
    [Route("settings")]
    public class SettingsController : ControllerBase
    {
        // DTO local para no crear archivo adicional:
        public record ChangePasswordResource(string CurrentPassword, string NewPassword);

        private readonly SettingsQueryService _queries;
        private readonly SettingsCommandService _commands;
        private readonly ProfileCommandService _profilesCommands;
        private readonly IAuthSessionRepository _sessions;

        public SettingsController(
            SettingsQueryService queries,
            SettingsCommandService commands,
            ProfileCommandService profilesCommands,
            IAuthSessionRepository sessions)
        {
            _queries = queries;
            _commands = commands;
            _profilesCommands = profilesCommands;
            _sessions = sessions;
        }

        private async Task<int?> GetCurrentProfileIdAsync()
        {
            if (!Request.Cookies.TryGetValue("sid", out var sid)) return null;
            if (!Guid.TryParse(sid, out var sidGuid)) return null;
            var s = await _sessions.FindByIdAsync(sidGuid);
            return (s is not null && s.IsActive()) ? s.ProfileId : (int?)null;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Obtener settings del usuario actual")]
        public async Task<IActionResult> Get()
        {
            var pid = await GetCurrentProfileIdAsync();
            if (pid is null) return Unauthorized();

            var s = await _queries.GetByProfileIdAsync(pid.Value);
            if (s is null) return Ok(new SettingsResource(true, "en", "light"));
            return Ok(new SettingsResource(s.NotificationsEmail, s.Locale, s.Theme));
        }

        [HttpPatch]
        [SwaggerOperation(Summary = "Actualizar settings del usuario actual")]
        public async Task<IActionResult> Patch([FromBody] SettingsResource r)
        {
            var pid = await GetCurrentProfileIdAsync();
            if (pid is null) return Unauthorized();

            var updated = await _commands.UpsertAsync(
                pid.Value, r.NotificationsEmail, r.Locale, r.Theme);

            return Ok(new SettingsResource(updated.NotificationsEmail, updated.Locale, updated.Theme));
        }

        [HttpPost("password")]
        [SwaggerOperation(Summary = "Cambiar contraseña del usuario actual")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordResource r)
        {
            var pid = await GetCurrentProfileIdAsync();
            if (pid is null) return Unauthorized();

            var ok = await _profilesCommands.ChangePasswordAsync(pid.Value, r.CurrentPassword, r.NewPassword);
            if (!ok) return BadRequest(new { message = "Current password is invalid." });
            return Ok(new { message = "Password updated." });
        }
    }
}
