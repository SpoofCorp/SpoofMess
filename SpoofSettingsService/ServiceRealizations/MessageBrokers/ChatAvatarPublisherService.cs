using AdditionalHelpers.Services;
using CommunicationLibrary;
using CommunicationLibrary.ServiceRealizations;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.MessageBrokers;

namespace SpoofSettingsService.ServiceRealizations.MessageBrokers;

public class ChatAvatarPublisherService(RabbitMQSettings settings, ISerializer serializer) : FileMessagePublisherService<ChatAvatar>(settings, serializer), IChatAvatarPublisherService
{
}
