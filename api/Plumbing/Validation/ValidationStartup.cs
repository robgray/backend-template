namespace Api.Plumbing.Validation;

using Core.Domain.Commands.Example;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

public static class ValidationStartup
{
    public static IMvcBuilder AddCustomValidation(this IMvcBuilder mvcBuilder)
    {
        mvcBuilder.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateCommandValidator>());

        return mvcBuilder;
    }
}