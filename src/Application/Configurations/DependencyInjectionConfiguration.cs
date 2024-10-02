using Application.Features.Motorcycles.Create;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Configurations;

public static class DependencyInjectionConfiguration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateMotorcycleCommandValidator>(includeInternalTypes: true);

        return services;
    }
}