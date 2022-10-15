using System.Reflection;
using Mapster;
using MapsterMapper;

namespace WetPet.Api.Mapping;

public static class AddMapsterExtension
{
    public static IServiceCollection AddMapster(this IServiceCollection services)
    {
        var config = new TypeAdapterConfig();
        config.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
        return services;
    }
}