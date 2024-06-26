using Azure.Messaging.ServiceBus;
using System.Text.Json;
namespace SignLingo.Services;

public class MyServiceBusClient
{
    private readonly ServiceBusClient _client;
    private readonly string _queueName;

    public MyServiceBusClient(string serviceBusConnectionString, string queueName)
    {
        _client = new ServiceBusClient(serviceBusConnectionString);
        _queueName = queueName;
    }

    public async Task SendMessageAsync<T>(T messageObject)
    {
        var sender = _client.CreateSender(_queueName);
        var messageBody = JsonSerializer.Serialize(messageObject);
        var message = new ServiceBusMessage(messageBody)
        {
            ApplicationProperties = { ["MessageType"] = typeof(T).Name }
        };
        await sender.SendMessageAsync(message);
    }
}
