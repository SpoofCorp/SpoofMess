using SpoofSettingsService.Models;

namespace SpoofSettingsService;

public readonly record struct ChatWithOwner(
        User User,
        Chat Chat
    );
