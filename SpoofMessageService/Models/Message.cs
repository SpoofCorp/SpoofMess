using DataSaveHelpers.EntityTypesRealizations.Identified;

namespace SpoofMessageService.Models;

public partial class Message : IdentifiedSoftDeletableChangeableEntity<Guid>
{
    public string Text { get; set; } = null!;

    public Guid ChatId { get; set; }

    public Guid UserId { get; set; }

    public DateTime SentAt { get; set; }

    public virtual ICollection<Attachment> Attachments { get; set; } = [];

    public virtual ICollection<MessageOperationStatus> MessageOperationStatuses { get; set; } = [];

    public virtual ICollection<ViewMessage> ViewMessages { get; set; } = [];
}
