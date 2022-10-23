namespace WetPet.Api.DependencyInjection;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WetPet.Infrastructure.Common.Utils;

public static partial class DependencyInjection
{
    public static void AddAuth(this IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
    {
        var auth0Settings = config.GetRequiredSection(Auth0Settings.SectionName).Get<Auth0Settings>();

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(opt =>
        {
            opt.Authority = $"https://{auth0Settings.Domain}/";
            opt.Audience = auth0Settings.Audience;
        });

        services.AddAuthorization(opt =>
        {
            var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireClaim(ClaimTypes.NameIdentifier)
                .Build();
            opt.DefaultPolicy = opt.FallbackPolicy = policy;
        });

        if (env.IsDevelopment())
        {
            services.AddSingleton<IAuthorizationHandler, AnonymousAuthorizationHandler>();
        }
    }

    public static IApplicationBuilder UseAuth(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }

}

public class Auth0Settings
{
    public const string SectionName = "Auth0Settings";
    public string Domain { get; set; } = null!;
    public string Audience { get; set; } = null!;
}

