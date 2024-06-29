using System;
using System.Linq;
using Core.Domain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.Automapper;

public static class AutomapperStartup
{
    public static void AddCustomAutoMapper(this IServiceCollection services, params Type[] types) =>
        services.AddAutoMapper(types.Concat(new[] { typeof(Example) }).ToArray());
}