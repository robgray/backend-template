using core.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace core.Plumbing.Messaging;

public static class MessagingStartup
{
    public static void AddCustomMessaging(this IServiceCollection services)
    {
        services.AddTransient<IQueueSender, QueueSender>(provider =>
            new QueueSender(provider.GetService<IConfiguration>().GetConnectionString("AzureStorage")));
    }
}