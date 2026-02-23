using CommunicationLibrary.Communication;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services.MessageBrokers;

public interface IChatUserPublisherService
{
    public Task Create(ChatUser chatUser, Rule[] rules);

    public Task Update(ChatUser chatUser, Rule[] rules);

    public Task Delete(ChatUser chatUser);

    public Task Create(CreateChatUser createChatUser);

    public Task Update(UpdateChatUser updateChatUser);

    public Task Delete(DeleteChatUser deleteChatUser);
}
