using System;
using System.Linq;
using AutoMapper;
using core.Domain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace core.Plumbing.Automapper;

public static class AutomapperStartup
{
    public static void AddCustomAutoMapper(this IServiceCollection services, params Type[] types) =>
        services.AddAutoMapper(types.Concat(new[] { typeof(Example) }).ToArray());
}