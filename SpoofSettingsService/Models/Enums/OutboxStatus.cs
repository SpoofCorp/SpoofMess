using NpgsqlTypes;

namespace SpoofSettingsService.Models.Enums;

public enum OutboxStatus
{
    [PgName("Delete")]
    Delete,
    [PgName("Create")]
    Create,
    [PgName("Update")]
    Update,
    [PgName("Rollback")]
    Rollback
}