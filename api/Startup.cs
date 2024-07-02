using Api.Infrastructure.Auth;
using Api.Infrastructure.Controllers;
using Api.Infrastructure.Cors;
using Api.Infrastructure.Swagger;
using Api.Infrastructure.Validation;
using Core.Infrastructure.KeyVault;
using Core.Infrastructure.Mediator;
using Core.Infrastructure.Messaging;
using Core.Infrastructure.Storage;

namespace Api;

using System;
using Api.Infrastructure.Hangfire;
using Api.Infrastructure.Options;
using Api.Infrastructure.User;
using Core.Infrastructure.Caching;
using Core.Infrastructure.Database;
using Core.Infrastructure.MappingLibrary;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Internal;
using Serilog;

public class Startup
{
    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        Configuration = configuration;
        Env = env;
    }

    public IConfiguration Configuration { get; }
    
    public IWebHostEnvironment Env { get; }


    private IOptionsProvider OptionsProvider { get; set; } =
        Api.Infrastructure.Options.OptionsProvider.Empty;
    

    public void ConfigureServices(IServiceCollection services)
    {
        OptionsProvider = services.AddCustomOptions(Configuration);
        
        services.AddSingleton(TimeProvider.System);
        
        services.AddLogging();
        
        services.AddApplicationInsightsTelemetry();
        
        services.AddCustomCors(Configuration);

        services.AddCustomControllers();

        services.AddCustomValidation();

        services.AddCustomSwagger();

        services.AddCustomAuth();

        services.AddHealthChecks();

        services.AddCustomMediator();

        services.AddCustomMappingLibrary(typeof(Startup));

        services.AddCustomMessaging();

        services.AddCustomAzureStorage();

        services.AddCustomKeyVault();

        services.AddCustomHangfire(Configuration);

        services.AddCustomUsers();

        services.AddCustomCaching();

        services.AddCustomDataContext(OptionsProvider.GetOptions<ConnectionStringsOptions>());
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSerilogRequestLogging();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

            app.UseCustomSwagger();
        }

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