using CommonObjects.DTO;
using SpoofMessageService.Models;

namespace SpoofMessageService.Services.Setters;

public static class ChatUserSetter
{
    public static ChatUserDTO Set(this ChatUser chatUser) =>
        new(
            chatUser.Key1, 
            11, 
            chatUser.Chat.UniqueName, 
            chatUser.Chat.Name, 
            11
        );
}
