using CommonObjects.DTO;
using CommonObjects.Requests.Messages;
using SpoofMessageService.Models;
using SpoofMessageService.Models.Enums;

namespace SpoofMessageService.Services.Setters;

public static class MessageSetter
{
    public static void Set(this Message message, EditMessageRequest request)
    {
        message.Text = request.Text ?? message.Text;
        //message.Attachments = request.Attachments?.Select(x => x.Set(operationsStatus)).ToList() ?? message.Attachments;
    }
    public static Message Set(this CreateMessageRequest request, Guid userId) =>
        new() { 
            ChatId = request.ChatId,
            UserId = userId,
            Attachments = [..
                request.Attachments.Select(x => x.Set())
                ], 
            Text = request.Text 
        };

    public static MessageDTO Set(this Message message, List<byte[]> attachments) =>
        new(
            message.Id, 
            message.ChatId, 
            message.UserId,
            message.User.Name,
            message.User.AvatarId,
            message.Text, 
            message.SentAt,
            attachments
            );
}
