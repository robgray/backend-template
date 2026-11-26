namespace Core.Infrastructure.MappingLibrary;

using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

public static class MappingLibraryStartup
{
    public static IServiceCollection AddCustomMappingLibrary(this IServiceCollection services, params Type[] types)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(types.Select(t => t.Assembly).Distinct());
        });
        
        return services;
    }
}