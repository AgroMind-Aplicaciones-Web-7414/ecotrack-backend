using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using EcotrackPlatform.API.Profile.Interfaces.REST.Resources;

namespace EcotrackPlatform.API.Profile.Interfaces.REST
{
    [ApiController]
    [Route("config")]
    public class ConfigController : ControllerBase
    {
        private readonly IConfiguration _config;
        public ConfigController(IConfiguration config) => _config = config;

        [HttpGet("public")]
        [SwaggerOperation(Summary = "Obtener configuración pública para el front")]
        public IActionResult GetPublic()
        {
            var registrationEnabled = _config.GetValue<bool?>("PublicConfig:RegistrationEnabled") ?? true;
            var locales = _config.GetSection("PublicConfig:Locales").Get<string[]>() ?? new[] { "en", "es" };
            var themes  = _config.GetSection("PublicConfig:Themes").Get<string[]>() ?? new[] { "light", "dark" };

            return Ok(new PublicConfigResource(registrationEnabled, locales, themes));
        }
    }
}