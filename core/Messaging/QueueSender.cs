using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Newtonsoft.Json;

namespace core.Messaging;

public interface IQueueSender
{
    Task<QueueClient> GetClient(string queueName);
    Task SendAsync(string queueName, object message);
}

public class QueueSender : IQueueSender
{
    private readonly string _connectionString;
    public QueueSender(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<QueueClient> GetClient(string queueName)
    {
        var client = new QueueClient(_connectionString, queueName);
        await client.CreateIfNotExistsAsync();
        return client;
    }

    public async Task SendAsync(string queueName, object message)
    {
        static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        var client = await GetClient(queueName);
        var serializeMessage = JsonConvert.SerializeObject(message);
        
        await client.SendMessageAsync(Base64Encode(serializeMessage));
    }
}