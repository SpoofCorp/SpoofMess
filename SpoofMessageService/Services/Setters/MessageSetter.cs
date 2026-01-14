using CommonObjects.DTO;
using CommonObjects.Requests;
using SpoofMessageService.Models;

namespace SpoofMessageService.Services.Setters;

public static class MessageSetter
{
    public static void Set(this Message message, EditMessageRequest request)
    {
        message.Text = request.Text ?? message.Text;
        message.Attachments =  message.Attachments;
    }
    public static Message Set(this CreateMessageRequest request, Guid userId) =>
        new() { ChatId = request.ChatId, UserId = userId, Attachments = [.. request.Attachments.Select(x => x.Set())], Text = request.Text };
}
