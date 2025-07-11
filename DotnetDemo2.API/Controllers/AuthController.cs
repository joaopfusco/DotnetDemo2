using DotnetDemo2.Service.DTOs;
using DotnetDemo2.Service.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DotnetDemo2.API.Controllers
{
    [AllowAnonymous]
    public class AuthController(IUserService service, ILogger<AuthController> logger) : BaseController(logger)
    {
        [HttpGet("[action]")]
        public IActionResult Login()
        {
            return TryExecute(() =>
            {
                return Challenge(new AuthenticationProperties
                {
                    RedirectUri = Url.Action("Callback", "Auth")
                }, OpenIdConnectDefaults.AuthenticationScheme);
            });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Callback()
        {
            return await TryExecuteAsync(async () =>
            {
                var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                if (!result.Succeeded)
                    return Unauthorized();

                var claims = result.Principal.Claims;

                var keycloakId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value 
                    ?? throw new Exception("Keycloak ID não encontrado nos claims.");

                var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
                    ?? throw new Exception("Email não encontrado nos claims.");

                var username = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value 
                    ?? throw new Exception("Nome não encontrado nos claims.");

                var userKeycloak = new UserKeycloak
                {
                    KeycloakId = keycloakId,
                    Email = email,
                    Username = username
                };

                var response = await service.Authenticate(userKeycloak);
                return Ok(response);
            });
        }
    }
}
