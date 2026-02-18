using AdditionalHelpers.Services;
using CommunicationLibrary;
using CommunicationLibrary.ServiceRealizations;
using SpoofMessageService.Models;
using SpoofMessageService.Services.Consumers;

namespace SpoofMessageService.ServiceRealizations.Consumers;

public class AttachmentConsumerService(
    RabbitMQSettings settings,
    ISerializer serializer, 
    ILoggerService loggerService
    ) : FileMessageConsumerService<FileMetadatum>(
        settings, 
        serializer,
        loggerService
        ), IAttachmentConsumerService
{
    protected override string FileNomination => "attachment";

    protected override string BaseQueueName => throw new NotImplementedException();

    protected override Func<FileMetadatum, Task> ConfirmDeletedFunc => throw new NotImplementedException();

    protected override Func<FileMetadatum, Task> ConfirmAddedFunc => throw new NotImplementedException();

    protected override Func<FileMetadatum, Task> ErrorDeletedFunc => throw new NotImplementedException();

    protected override Func<FileMetadatum, Task> ErrorAddedFunc => throw new NotImplementedException();

}