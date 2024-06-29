using Core.Domain.Commands.Example;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Infrastructure.Validation;

public static class ValidationStartup
{
    public static IServiceCollection AddCustomValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateExample.Validator>();

        return services;
    }
}