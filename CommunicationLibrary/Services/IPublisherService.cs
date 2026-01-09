namespace CommunicationLibrary.Services;

public interface IPublisherService
{
    public Task Publish<T>(T obj);
}
