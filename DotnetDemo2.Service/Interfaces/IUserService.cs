using DotnetDemo2.Domain.Models;
using DotnetDemo2.Service.DTOs;

namespace DotnetDemo2.Service.Interfaces
{
    public interface IUserService : IBaseService<User>
    {
        Task<LoginResponse> Authenticate(UserKeycloak userKeycloak);
    }
}
