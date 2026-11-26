namespace Api.Infrastructure.Swagger;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.OpenApi;

public static class SwaggerStartup
{
    public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "api", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                In = ParameterLocation.Header, 
                Description = "Please insert 'Bearer ' and the then JWT into the field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey, 
            });
        });

        return services;
    }

    public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "api v1");
            c.RoutePrefix = string.Empty;
            c.DocExpansion(DocExpansion.List);
            c.DefaultModelExpandDepth(3);
            c.DefaultModelsExpandDepth(0);
        });
        
        return app;
    }
}