using DotnetDemo2.Service.Interfaces;
using DotnetDemo2.Service.Services;

namespace DotnetDemo2.API.Extensions
{
    internal static class ServicesExtensions
    {
        internal static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();

            return services;
        }
    }
}
