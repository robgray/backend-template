namespace Api;

using System;
using Api.Infrastructure.Auth;
using Api.Infrastructure.Controllers;
using Api.Infrastructure.Cors;
using Api.Infrastructure.Hangfire;
using Api.Infrastructure.HealthChecks;
using Api.Infrastructure.Options;
using Api.Infrastructure.Swagger;
using Api.Infrastructure.User;
using Api.Infrastructure.Validation;
using Core.Infrastructure.Caching;
using Core.Infrastructure.Database;
using Core.Infrastructure.KeyVault;
using Core.Infrastructure.MappingLibrary;
using Core.Infrastructure.Mediator;
using Core.Infrastructure.Messaging;
using Core.Infrastructure.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
        Infrastructure.Options.OptionsProvider.Empty;


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
        var authenticationOptions = OptionsProvider.GetOptions<AuthenticationOptions>();

        app.UseSerilogRequestLogging();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

            app.UseCustomSwagger();
        }

        app.UseRouting();

        app.UseCustomHealthChecks();

        app.ConfigureCustomCors();

        app.UseHttpsRedirection();


        app.ConfigureCustomAuth();

        app.UseCustomHangfire(env, authenticationOptions);

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}