using DataSaveHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpoofEntranceService.Models;

public partial class Token : IdentifiedSoftDeletableEntity<string>
{
    [Column("RefreshTokenHash")]
    public new string Id { get; set; } = default!;

    public Guid SessionInfoId { get; set; }

    public DateTime ValidTo { get; set; }

    public virtual SessionInfo SessionInfo { get; set; } = null!;
}
