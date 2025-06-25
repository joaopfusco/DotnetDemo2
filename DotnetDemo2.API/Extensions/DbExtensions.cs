using DotnetDemo2.Repository.Data;
using Microsoft.Extensions.DependencyInjection;

namespace DotnetDemo2.API.Shared.Extensions
{
    public static class DbExtensions
    {
        public static IServiceCollection AddAppDbContext(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
#if DEBUG
                options.EnableSensitiveDataLogging();
#endif
            });

            return services;
        }
    }
}
