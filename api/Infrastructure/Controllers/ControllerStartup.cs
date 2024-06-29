using System.Text.Json.Serialization;
using Api.Infrastructure.Mediator;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Infrastructure.Controllers;

public static class ControllerStartup
{
    public static IServiceCollection AddCustomControllers(this IServiceCollection services, params AssemblyPart[] parts)
    {
        services
            .AddControllers(options => { options.Filters.Add<MediatorExceptionFilter>(); })
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
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