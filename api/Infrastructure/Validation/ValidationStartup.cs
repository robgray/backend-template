namespace Api.Infrastructure.Validation;

using Core.Domain.Commands.Example;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

public static class ValidationStartup
{
    public static IServiceCollection AddCustomValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateExample.Validator>();

        return services;
    }
}