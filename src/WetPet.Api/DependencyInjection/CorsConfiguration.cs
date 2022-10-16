namespace WetPet.Api.DependencyInjection;

public static partial class StartupConfigurationExtensions
{
    public const string CorsPolicy = "TheCorsPolicy";

    public static IServiceCollection ConfigureCors(this IServiceCollection services, IConfiguration config)
    {
        var settings = config.GetSection(CorsSettings.SectionName).Get<CorsSettings>();
        if (settings?.AllowedOrigins is null)
        {
            return services;
        }
        services.AddCors(opt =>
        {
            opt.AddPolicy(name: CorsPolicy, builder =>
            {
                builder
                    .WithOrigins(settings.AllowedOrigins)
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
        return services;
    }

    public static IApplicationBuilder UseCustomCors(this IApplicationBuilder app)
    {
        app.UseCors(CorsPolicy);
        return app;
    }
}

public class CorsSettings
{
    public const string SectionName = "CorsSettings";
    public string[]? AllowedOrigins { get; set; }
}