using DataSaveHelpers.EntityTypesRealizations.Identified;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpoofEntranceService.Models;

public partial class Token : IdentifiedSoftDeletableEntity<string>
{
    [Column("RefreshTokenHash")]
    public new string Id { get; set; } = default!;

    public Guid SessionInfoId { get; set; }

    public DateTime ValidTo { get; set; }

    public virtual SessionInfo SessionInfo { get; set; } = null!;

    public Token() { }

    public Token(string tokenHash, SessionInfo sessionInfo, DateTime validTo)
    {
        Id = tokenHash;
        SessionInfo = sessionInfo;
        SessionInfoId = sessionInfo.Id;
        ValidTo = validTo;
    }
}
