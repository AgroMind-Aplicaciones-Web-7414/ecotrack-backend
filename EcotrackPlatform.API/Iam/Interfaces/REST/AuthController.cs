using Microsoft.AspNetCore.Mvc;
using EcotrackPlatform.API.Iam.Application.Internal.CommandServices;
using EcotrackPlatform.API.Iam.Interfaces.REST.Resources;

namespace EcotrackPlatform.API.Iam.Interfaces.REST;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly AuthCommandService _auth;

    public AuthController(AuthCommandService auth) { _auth = auth; }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginResource body)
    {
        var ua = Request.Headers.UserAgent.ToString();
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        var session = await _auth.LoginAsync(body.Email, body.Password, ua, ip);
        if (session is null) return Unauthorized(new { message = "Invalid credentials" });

        Response.Cookies.Append("sid", session.Id.ToString(), new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = session.ExpiresAt
        });

        return Ok(new { message = "Logged in" });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        if (!Request.Cookies.TryGetValue("sid", out var sid) || !Guid.TryParse(sid, out var id))
        {
            // idempotente
            Response.Cookies.Delete("sid");
            return Ok(new { message = "Logged out" });
        }

        await _auth.LogoutAsync(id);
        Response.Cookies.Delete("sid");
        return Ok(new { message = "Logged out" });
    }
}