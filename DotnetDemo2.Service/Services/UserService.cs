using DotnetDemo2.Domain.Models;
using DotnetDemo2.Repository.Data;
using DotnetDemo2.Service.DTOs;
using DotnetDemo2.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DotnetDemo2.Service.Services
{
    public class UserService(AppDbContext db, IConfiguration configuration) : BaseService<User>(db), IUserService
    {
        private readonly IConfiguration _configuration = configuration;

        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("keycloak_id", user.KeycloakId.ToString()),
                new Claim(ClaimTypes.Surname, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var keyString = _configuration["JwtKey"] ?? throw new Exception("JwtKey não encontrado.");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.Now.AddHours(5),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<User> SyncUser(UserKeycloak userKeycloak)
        {
            var user = Get(u => u.KeycloakId == userKeycloak.KeycloakId).FirstOrDefault();

            if (user == null)
            {
                user = new User
                {
                    Username = userKeycloak.Username ?? "Sem Nome",
                    Email = userKeycloak.Email ?? "Sem Email",
                };

                user.SetKeycloakId(userKeycloak.KeycloakId);
                
                await Insert(user);
            }
            else
            {
                user.Username = userKeycloak.Username ?? user.Username;
                user.Email = userKeycloak.Email ?? user.Email;

                await Update(user);
            }

            return user;
        }

        public async Task<LoginResponse> Authenticate(UserKeycloak userKeycloak)
        {
            var user = await SyncUser(userKeycloak);
            var token = GenerateToken(user);

            return new LoginResponse
            {
                Token = token,
                User = user,
            };
        }
    }
}
