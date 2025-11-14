using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

using EcotrackPlatform.API.Profile.Application.Internal.QueryServices;
using EcotrackPlatform.API.Profile.Application.Internal.CommandServices;
using EcotrackPlatform.API.Profile.Interfaces.REST.Resources;
using EcotrackPlatform.API.Profile.Interfaces.REST.Transform;
using EcotrackPlatform.API.Profile.Domain.Model.Commands;
using EcotrackPlatform.API.Iam.Domain.Repositories;

namespace EcotrackPlatform.API.Profile.Interfaces.REST
{
    [ApiController]
    [Route("users")]
    public class ProfilesController : ControllerBase
    {
        private readonly ProfileQueryService _queries;
        private readonly ProfileCommandService _commands;
        private readonly IAuthSessionRepository _sessions;

        public ProfilesController(ProfileQueryService queries, ProfileCommandService commands, IAuthSessionRepository sessions)
        {
            _queries = queries;
            _commands = commands;
            _sessions = sessions;
        }

        // Resuelve el usuario actual desde cookie "sid" (sin JWT)
        private async Task<int?> GetCurrentProfileIdAsync()
        {
            if (!Request.Cookies.TryGetValue("sid", out var sid)) return null;
            if (!Guid.TryParse(sid, out var sidGuid)) return null;
            var s = await _sessions.FindByIdAsync(sidGuid);
            return (s is not null && s.IsActive()) ? s.ProfileId : (int?)null;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Listar todos los usuarios")]
        public async Task<IActionResult> List()
        {
            var list = await _queries.ListAsync();
            return Ok(list.Select(ProfileResourceFromEntityAssembler.ToResource));
        }

        [HttpGet("{id:int}")]
        [SwaggerOperation(Summary = "Obtener un usuario por id")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var e = await _queries.FindByIdAsync(id);
            if (e is null) return NotFound();
            return Ok(ProfileResourceFromEntityAssembler.ToResource(e));
        }

        [HttpGet("me")]
        [SwaggerOperation(Summary = "Obtener el usuario actual (requiere sesión)")]
        public async Task<IActionResult> Me()
        {
            var pid = await GetCurrentProfileIdAsync();
            if (pid is null) return Unauthorized();

            var e = await _queries.GetCurrentAsync(pid.Value);
            if (e is null) return NotFound();
            return Ok(ProfileResourceFromEntityAssembler.ToResource(e));
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Crear un nuevo usuario")]
        public async Task<IActionResult> Create([FromBody] CreateProfileResource resource)
        {
            var cmd = CreateProfileCommandFromResourceAssembler.ToCommand(resource);
            var entity = await _commands.CreateAsync(cmd.Email, cmd.DisplayName, cmd.Password);
            var res = ProfileResourceFromEntityAssembler.ToResource(entity);
            return CreatedAtAction(nameof(GetById), new { id = res.Id }, res);
        }

        [HttpPatch("{id:int}")]
        [SwaggerOperation(Summary = "Actualizar un usuario")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateProfileResource resource)
        {
            var cmd = UpdateProfileCommandFromResourceAssembler.ToCommand(id, resource);
            try
            {
                var updated = await _commands.UpdateAsync(cmd.Id, cmd.DisplayName, cmd.Email);
                if (updated is null) return NotFound();
                return Ok(ProfileResourceFromEntityAssembler.ToResource(updated));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message }); // email en uso
            }
        }

        [HttpDelete("{id:int}")]
        [SwaggerOperation(Summary = "Eliminar un usuario")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var ok = await _commands.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}
