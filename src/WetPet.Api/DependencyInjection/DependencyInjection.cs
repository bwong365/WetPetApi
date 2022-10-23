using System.Text.Json.Serialization;
using WetPet.Api.Mapping;
using WetPet.Api.Util;

namespace WetPet.Api.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RouteOptions>(opt =>
        {
            opt.LowercaseUrls = true;
            opt.LowercaseQueryStrings = true;
        });
        services.AddControllers().AddJsonOptions(opt =>
        {
            opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SchemaFilter<RequireNonNullablePropertiesSchemaFilter>();
            c.SupportNonNullableReferenceTypes();
            c.UseAllOfToExtendReferenceSchemas();
        });
        services.AddMapster();
        services.ConfigureCors(configuration);
        return services;
    }
}
