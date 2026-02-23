using NpgsqlTypes;

namespace SpoofSettingsService.Models;

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
