namespace Core.Infrastructure.MappingLibrary;

using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Mapster;

public static class MappingLibraryStartup
{
    public static IServiceCollection AddCustomMappingLibrary(this IServiceCollection services, params Type[] types)
    {
        services.AddMapster();

        TypeAdapterConfig.GlobalSettings.Scan(types.Select(x => x.Assembly).ToArray());
        
        return services;
    }
}