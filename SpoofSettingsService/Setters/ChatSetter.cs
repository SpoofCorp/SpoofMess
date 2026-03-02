using CommonObjects.Requests.Changes;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Setters;

public static class ChatSetter
{
    public static void Set(this Chat chat, ChangeChatSettingsRequest request)
    {
        chat.OwnerId = request.OwnerId ?? chat.OwnerId;
        chat.ChatTypeId = request.ChatTypeId ?? chat.ChatTypeId;
        chat.Name = request.ChatName ?? chat.Name;
        chat.UniqueName = request.UniqueName ?? chat.UniqueName;
    }
}
