namespace SpoofMessageService.Models.Enums;

public enum Rules : long
{
    SendTexts = 1L << 0,
    SendAudios = 1L << 1,
    SendVideos = 1L << 2,
    SendFiles = 1L << 3,
    SendEmoji = 1L << 4,
    SendSticker = 1L << 5,
    SendVoiceMessage = 1L << 6,
    SendVideoMessage = 1L << 7,
    ShareMessage = 1L << 8,
    DeleteMessage = 1L << 9,
    EditMessage = 1L << 10
}
