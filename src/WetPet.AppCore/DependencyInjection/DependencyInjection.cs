using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using WetPet.AppCore.Common.Behaviors;
using WetPet.AppCore.Interfaces;
using WetPet.AppCore.Providers;
using WetPet.AppCore.Services;

namespace WetPet.AppCore.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddAppCore(this IServiceCollection services)
    {
        services.AddSingleton<IClimateRangeProvider, ClimateRangeProvider>();
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddScoped<IPetStatusService, PetStatusService>();
        return services;
    }
}