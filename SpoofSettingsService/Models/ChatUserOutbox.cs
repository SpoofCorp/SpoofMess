using DataSaveHelpers.EntityTypesRealizations.Identified;

namespace SpoofSettingsService.Models;

public partial class ChatUserOutbox : IdentifiedEntity<Guid>
{
    public Guid ChatId { get; set; }

    public Guid UserId { get; set; }

    public bool IsSynced { get; set; }

    public OutboxStatus Status { get; set; }

    public DateTime LastTryDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Data { get; set; } = null!;

    public virtual ChatUser ChatUser { get; set; } = null!;
}
