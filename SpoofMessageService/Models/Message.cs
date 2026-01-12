using System;
using System.Collections.Generic;

namespace SpoofMessageService.Models;

public partial class Message
{
    public Guid Id { get; set; }

    public string Text { get; set; } = null!;

    public Guid ChatId { get; set; }

    public Guid UserId { get; set; }

    public DateTime SentAt { get; set; }

    public DateTime LastModified { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();

    public virtual ICollection<ViewMessage> ViewMessages { get; set; } = new List<ViewMessage>();
}
