namespace Tests.Plumbing;

using System.Reflection;
using Api;
using Api.Plumbing.Auth;
using Api.Plumbing.Controllers;
using Api.Plumbing.Cors;
using Core.Plumbing.Automapper;
using Core.Plumbing.Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

public class TestStartup
{
    public TestStartup(
        IConfiguration configuration,
        IWebHostEnvironment env)
    {
        Configuration = configuration;
        Env = env;
    }
    
    public IConfiguration Configuration { get; }

    public IWebHostEnvironment Env { get; }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCustomCors(Configuration);
        services.AddHealthChecks();

        // Note: this is necessary because AddControllers only adds from the current Assembly
        // i.e. this testing Assembly. We need to add controllers from the actual Api assembly.
        var assembly = typeof(Program).GetTypeInfo().Assembly;
        var part = new AssemblyPart(assembly);
        services.AddCustomControllers(part);
        
        services.AddCustomMediator();
        
        services.AddCustomAutoMapper(typeof(Startup));
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSerilogRequestLogging();
        
        app.UseHealthChecks("/health");
        
        app.ConfigureCustomCors();

        app.UseHttpsRedirection();

        app.UseRouting();

        app.ConfigureCustomAuth();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}