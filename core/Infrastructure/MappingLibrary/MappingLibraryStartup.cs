namespace Core.Infrastructure.MappingLibrary;

using System;
using System.Linq;
using Core.Domain.Models;
using Microsoft.Extensions.DependencyInjection;

public static class MappingLibraryStartup
{
    public static IServiceCollection AddCustomMappingLibrary(this IServiceCollection services, params Type[] types)
    {
        services.AddAutoMapper(types.Concat(new[] { typeof(Example) }).ToArray());
        
        return services;
    }
}