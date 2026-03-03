using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace CommonObjects.DTO;

public class ChatUserDTO
{
    public Guid Id { get; set; }
    public int ChatTypeId { get; set; }
    public string UniqueName { get; set; }
    public string Name { get; set; }
    [NotMapped]
    public List<PermissionResult> Rules =>
            string.IsNullOrEmpty(RulesJson) ? [] : JsonSerializer.Deserialize<List<PermissionResult>>(RulesJson) ?? [];
    public string RulesJson { get; set; }

}