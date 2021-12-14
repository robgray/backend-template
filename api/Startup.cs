using System.Text.Json.Serialization;
using api.Plumbing.Auth;
using api.Plumbing.Cors;
using api.Plumbing.Mediator;
using api.Plumbing.Swagger;
using api.Plumbing.Validation;
using core.Plumbing.Automapper;
using core.Plumbing.KeyVault;
using core.Plumbing.Mediator;
using core.Plumbing.Messaging;
using core.Plumbing.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Internal;
using Serilog;

namespace api
{
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

                app.ConfigureCustomSwagger();
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
}