using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Api.Infrastructure.Options;

public static class OptionsStartup
{
	public static IOptionsProvider AddValdOptions(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var optionsServices = new ServiceCollection();
        optionsServices.AddValidateOnStartOptions();
        optionsServices.AddValidateOnGetOptions();

        optionsServices.AddOptions<ForwardedHeadersOptions>().Configure(
            options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.All;
                /* only loopback proxies are allowed by default, so clear that restriction */
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });

        foreach (var descriptor in optionsServices)
        {
            services.Add(descriptor);
        }

        services.AddSingleton<IOptionsValidator>(sp => new OptionsValidator(sp));

        // // 'services' has IConfiguration registered; 'optionsServices' does not
        optionsServices.AddSingleton(configuration);

        var serviceProvider = optionsServices.BuildServiceProvider();
        return new OptionsProvider(serviceProvider);
    }

    private static IServiceCollection AddValidateOnGetOptions(
        this IServiceCollection services)
    {
        // Add options that don't need to correct at app startup
        // services.AddValidateOnGetOptions<AnOptions>(AnOptions.Key);
        
        return services;
    }

    private static OptionsBuilder<TOptions> AddValidateOnGetOptions<TOptions>(
        this IServiceCollection services,
        string configSectionPath)
        where TOptions : class
    {
        OptionsValidator.AddOptionsType<TOptions>();

        return services.AddOptions<TOptions>()
            .BindConfiguration(configSectionPath)
            .ValidateDataAnnotations();
    }

    private static IServiceCollection AddValidateOnStartOptions(
        this IServiceCollection services)
    {
        services.AddValidateOnStartOptions<AuthenticationOptions>(AuthenticationOptions.Key);
        services.AddValidateOnStartOptions<ConnectionStringsOptions>(ConnectionStringsOptions.Key);
        
        return services;
    }

    private static OptionsBuilder<TOptions> AddValidateOnStartOptions<TOptions>(
        this IServiceCollection services,
        string configSectionPath)
        where TOptions : class =>
        services.AddValidateOnGetOptions<TOptions>(configSectionPath)
            .ValidateOnStart();
}