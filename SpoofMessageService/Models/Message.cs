using DataSaveHelpers;

namespace SpoofMessageService.Models;

public partial class Message : IdentifiedSoftDeletableEntity<Guid>
{
    public string Text { get; set; } = null!;

    public Guid ChatId { get; set; }

    public Guid UserId { get; set; }

    public DateTime SentAt { get; set; }

    public DateTime LastModified { get; set; }

    public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();

    public virtual ICollection<MessageOperationStatus> MessageOperationStatuses { get; set; } = new List<MessageOperationStatus>();

    public virtual ICollection<ViewMessage> ViewMessages { get; set; } = new List<ViewMessage>();
}
