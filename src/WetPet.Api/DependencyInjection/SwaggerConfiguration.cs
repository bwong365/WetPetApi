using Microsoft.OpenApi.Models;
using WetPet.Api.Util;

namespace WetPet.Api.DependencyInjection;

public static partial class DependencyInjection
{
    public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "WetPet API", Version = "v1" });
            var securityScheme = new OpenApiSecurityScheme
            {
                Description = "Enter your token below:",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
            };
            c.AddSecurityDefinition("Bearer", securityScheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                    { securityScheme, new[] { "Bearer" } }
            });

            c.SchemaFilter<RequireNonNullablePropertiesSchemaFilter>();
            c.SupportNonNullableReferenceTypes();
            c.UseAllOfToExtendReferenceSchemas();
        });

        return services;
    }

    public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        return app;
    }
}