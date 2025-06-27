using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.NewtonsoftJson;
using Newtonsoft.Json.Serialization;

namespace DotnetDemo2.API.Extensions
{
    public static class ControllerExtensions
    {
        public static IServiceCollection AddAppControllers(this IServiceCollection services)
        {
            services
                .AddControllers()
                .AddNewtonsoftJson(opt =>
                {
                    opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                })
                .AddOData(opt => opt.Select()
                    .Expand()
                    .SetMaxTop(null)
                    .SkipToken()
                    .OrderBy()
                    .Count()
                    .Filter()
                    .EnableQueryFeatures(1000)
                )
                .AddODataNewtonsoftJson();

            return services;
        }
    }
}
