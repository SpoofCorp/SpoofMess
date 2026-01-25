using AdditionalHelpers.Services;
using CommunicationLibrary;
using CommunicationLibrary.ServiceRealizations;
using SpoofMessageService.Models;
using SpoofMessageService.Services;

namespace SpoofMessageService.ServiceRealizations;

public class AttachmentConsumerService(RabbitMQSettings settings, ISerializer serializer) : FileMessageConsumerService<FileMetadatum>(settings, serializer), IAttachmentConsumerService
{
    private readonly string _exchange = "settings-service";

    protected override string FileNomination => "attachment";

    protected override Func<FileMetadatum, Task> ConfirmDeletedFunc => throw new NotImplementedException();

    protected override Func<FileMetadatum, Task> ConfirmAddedFunc => throw new NotImplementedException();

    protected override Func<FileMetadatum, Task> ErrorDeletedFunc => throw new NotImplementedException();

    protected override Func<FileMetadatum, Task> ErrorAddedFunc => throw new NotImplementedException();
}