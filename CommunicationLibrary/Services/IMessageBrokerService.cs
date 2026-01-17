namespace CommunicationLibrary.Services;

public interface IMessageBrokerService
{
    public Task<string> ConsumeFromQueueAsync<T>(
       string exchange,
       string queueName,
       string routingKey,
       Func<T, Task> handler);
}