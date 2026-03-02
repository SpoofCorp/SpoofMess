using CommonObjects.DTO;
using CommonObjects.Requests.Messages;
using SpoofMessageService.Models;
using SpoofMessageService.Models.Enums;

namespace SpoofMessageService.Services.Setters;

public static class MessageSetter
{
    public static void Set(this Message message, EditMessageRequest request, OperationsStatus operationsStatus)
    {
        message.Text = request.Text ?? message.Text;
        message.MessageOperationStatuses.Add(new() { OperationStatusId = (short)operationsStatus });
        //message.Attachments = request.Attachments?.Select(x => x.Set(operationsStatus)).ToList() ?? message.Attachments;
    }
    public static Message Set(this CreateMessageRequest request, Guid userId, OperationsStatus operationsStatus) =>
        new() { 
            ChatId = request.ChatId,
            UserId = userId,
            MessageOperationStatuses = [
                new() { 
                    OperationStatusId = (short)operationsStatus 
                }], 
            Attachments = [..
                request.Attachments.Select(x => x.Set(operationsStatus))
                ], 
            Text = request.Text 
        };

    public static MessageDTO Set(this Message message) =>
        new(
            message.Id, 
            message.ChatId, 
            message.UserId,
            message.User.Name,
            message.User.AvatarId,
            message.Text, 
            message.SentAt
            );
}
