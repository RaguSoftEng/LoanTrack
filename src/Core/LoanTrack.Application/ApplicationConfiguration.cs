using System.Reflection;
using FluentValidation;
using LoanTrack.Application.Common.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace LoanTrack.Application;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services
    )
    {
        Assembly assembly = typeof(ApplicationConfiguration).Assembly;
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(assembly);

            config.AddOpenBehavior(typeof(ExceptionHandlingPipelineBehavior<,>));
            config.AddOpenBehavior(typeof(RequestLoggingPipelineBehavior<,>));
            config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
        });

        services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true);

        return services;
    }
}
