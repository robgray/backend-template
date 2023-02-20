namespace Api.Plumbing.Controllers;

using System.Text.Json.Serialization;
using Api.Plumbing.Mediator;
using Api.Plumbing.Validation;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

public static class ControllerStartup
{
    public static IServiceCollection AddCustomControllers(this IServiceCollection services, params AssemblyPart[] parts)
    {
        services
            .AddControllers(options => { options.Filters.Add<MediatorExceptionFilter>(); })
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
            .AddCustomValidation()
            .ConfigureApplicationPartManager(apm =>
            {
                foreach (var part in parts)
                {
                    apm.ApplicationParts.Add(part);
                }
            });

        return services;
    }
}