using DotnetDemo2.API.Extensions;
using DotnetDemo2.API.Shared.Extensions;

namespace DotnetDemo2.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAppAuthentication(
                builder.Configuration,
                builder.Environment
            );

            builder.Services.AddAppControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddAppDbContext();

            builder.Services.AddAppServices();

            builder.Services.AddAppSwaggerGen(
                builder.Configuration
            );

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
