using NpgsqlTypes;

namespace SpoofSettingsService.Models.Enums;

public enum ChatViewType
{
    [PgName("Main")]
    Main,
    [PgName("Topic")]
    Topic,
    [PgName("Thread")]
    Thread
}