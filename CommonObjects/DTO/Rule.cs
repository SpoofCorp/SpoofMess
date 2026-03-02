using System.ComponentModel.DataAnnotations.Schema;

namespace CommonObjects.DTO;

[ComplexType]
public class PermissionResult
{
    public short RuleId { get; set; }
    public bool IsAllowed { get; set; }
}