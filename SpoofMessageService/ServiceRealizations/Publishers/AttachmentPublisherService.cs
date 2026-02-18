using AdditionalHelpers.Services;
using CommunicationLibrary;
using CommunicationLibrary.ServiceRealizations;
using SpoofMessageService.Models;
using SpoofMessageService.Services.Publishers;

namespace SpoofMessageService.ServiceRealizations.Publishers;

public class AttachmentPublisherService(
    RabbitMQSettings settings, 
    ISerializer serializer
    ) : FileMessagePublisherService<Attachment>(
        settings,
        serializer
        ), IAttachmentPublisherService
{
    
}