using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Infrastructure.Controllers;

public static class ControllerStartup
{
    public static IServiceCollection AddCustomControllers(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        return services;
    }
}