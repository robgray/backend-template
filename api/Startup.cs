namespace Api;

using System.Text.Json.Serialization;
using Core.Plumbing.Automapper;
using Core.Plumbing.KeyVault;
using Core.Plumbing.Mediator;
using Core.Plumbing.Messaging;
using Core.Plumbing.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Internal;
using Plumbing.Auth;
using Plumbing.Cors;
using Plumbing.Mediator;
using Plumbing.Swagger;
using Plumbing.Validation;
using Serilog;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging();
        services.AddApplicationInsightsTelemetry();

        services.AddCustomCors(Configuration);

        services.AddControllers(options => { options.Filters.Add<MediatorExceptionFilter>(); })
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
            .AddCustomValidation();

        services.AddCustomSwagger();

        services.AddCustomAuth();

        services.AddHealthChecks();

        services.AddCustomMediator();

        services.AddCustomAutoMapper(typeof(Startup));

        services.AddCustomMessaging();

        services.AddSingleton<ISystemClock, SystemClock>();

        services.AddCustomAzureStorage();

        services.AddCustomKeyVault();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSerilogRequestLogging();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

            app.UseCustomSwagger();
        }

        app.ConfigureCustomCors();

        app.UseHttpsRedirection();

        app.UseRouting();

        app.ConfigureCustomAuth();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks("/healthz");
            endpoints.MapControllers();
        });
    }
}