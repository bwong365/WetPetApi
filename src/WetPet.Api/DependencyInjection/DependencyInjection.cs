using System.Text.Json.Serialization;
using WetPet.Api.Mapping;
using WetPet.Api.Util;

namespace WetPet.Api.DependencyInjection;

public static partial class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        services.Configure<RouteOptions>(opt =>
        {
            opt.LowercaseUrls = true;
            opt.LowercaseQueryStrings = true;
        });
        services.AddAuth(configuration, env);
        services.AddControllers().AddJsonOptions(opt =>
        {
            opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        services.AddCustomSwagger();
        services.AddMapster();
        services.ConfigureCors(configuration);
        return services;
    }
}
