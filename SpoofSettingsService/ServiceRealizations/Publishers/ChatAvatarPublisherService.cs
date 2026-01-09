using SpoofSettingsService.Services.Publisher;

namespace SpoofSettingsService.ServiceRealizations.Publishers;

public class ChatAvatarPublisherService : IChatAvatarPublisherService
{
    public Task Publish<T>(T obj)
    {
        throw new NotImplementedException();
    }
}
