using core.Domain.Commands.Example;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace api.Plumbing.Validation;

public static class ValidationStartup
{
    public static void AddCustomValidation(this IMvcBuilder mvcBuilder)
    {
        mvcBuilder.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateCommandValidator>());
    }
}