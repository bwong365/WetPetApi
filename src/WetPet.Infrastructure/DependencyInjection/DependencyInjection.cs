using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.CircuitBreaker;
using Polly.Contrib.WaitAndRetry;
using WetPet.AppCore.Interfaces;
using WetPet.Infrastructure.Http.OpenWeatherMap;
using WetPet.Infrastructure.Persistence;
using WetPet.Infrastructure.Services;
using WetPetAPI.WetPet.Infrastructure.Http.OpenWeatherMap;

namespace WetPet.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(config.GetConnectionString("AppDb")));
        services.AddScoped<IOwnerRepository, OwnerRepository>();
        services.AddScoped<IPetRepository, PetRepository>();

        services.AddMemoryCache();
        services.Configure<OpenWeatherMapSettings>(config.GetSection(OpenWeatherMapSettings.SectionName));
        services.AddHttpClient<IOpenWeatherMapHttpService, OpenWeatherMapHttpService>()
            .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 5)));
        services.AddScoped<ILocationService, LocationService>();
        services.AddScoped<IWeatherService, WeatherService>();
        return services;
    }

    private static readonly AsyncCircuitBreakerPolicy<HttpResponseMessage> CircuitBreakerPolicy = Policy.HandleResult<HttpResponseMessage>(message => (int) message.StatusCode == 503).CircuitBreakerAsync(2, TimeSpan.FromMinutes(1));
}