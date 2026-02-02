namespace CommonObjects.Requests.ChatUsers;

public class ChangeRulesRequest
{
    public required Guid ChatId { get; set; }
    public required Guid UserId { get; set; }
    public required short RuleId { get; set; }
    public bool IsPermission { get; set; } = true;
}
