using DotnetDemo2.API.Controllers.Abstracts;
using DotnetDemo2.Service.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DotnetDemo2.API.Controllers.Api
{
    public class AuthController(ILogger<AuthController> logger) : BaseController(logger)
    {
        [AllowAnonymous]
        [HttpGet("[action]")]
        public IActionResult Login(string returnUrl = "/")
        {
            return TryExecute(() =>
            {
                return Challenge(
                    new AuthenticationProperties { RedirectUri = returnUrl },
                    OpenIdConnectDefaults.AuthenticationScheme
                );
            });
        }

        [HttpGet("[action]")]
        public IActionResult Logout()
        {
            return TryExecute(() =>
            {
                return SignOut(
                    new AuthenticationProperties
                    {
                        RedirectUri = Url.Content("~/api/Auth/Login")
                    },
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    OpenIdConnectDefaults.AuthenticationScheme
                );
            });
        }

        [HttpGet("[action]")]
        public IActionResult Me()
        {
            return TryExecute(() =>
            {
                var claims = User.Claims.ToDictionary(c => c.Type, c => c.Value);
                return Ok(claims);
            });
        }
    }
}
