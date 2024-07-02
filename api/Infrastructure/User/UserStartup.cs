namespace Api.Infrastructure.User;

using Core.Infrastructure.User;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

public static class UserStartup
{
    public static IServiceCollection AddCustomUsers(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext>(provider =>
        {
            var contextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
            
            if (contextAccessor.HttpContext?.User.Identity is null
                || !contextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return new SystemContext();
            }

            return new UserContext(contextAccessor);
        });

        return services;
    } 
}
