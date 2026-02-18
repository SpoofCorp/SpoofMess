using CommunicationLibrary.Communication;

namespace SpoofSettingsService.Services.MessageBrokers;


public interface IChatPublisherService
{
    public Task Publish(CreateChat chat);
}
