using CommunicationLibrary.Communication;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services.MessageBrokers;

public interface IChatUserPublisherService
{
    public Task Create(ChatUser chatUser, Rule[] rules);
}
