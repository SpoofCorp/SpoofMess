using DataSaveHelpers.EntityTypesRealizations.DoubleIdentified;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpoofMessageService.Models;

public partial class ViewMessage : DoubleIdentifiedSoftDeletableChangeableEntity<Guid, Guid>
{
    [Column("UserId")]
    public new Guid Key1 { get; set; }

    [Column("MessageId")]
    public new Guid Key2 { get; set; }

    public DateTime ViewTime { get; set; }

    public virtual Message Message { get; set; } = null!;
}
