namespace CommonObjects.Requests.Changes;

public class ChangeChatSettingsRequest
{
    public Guid Id { get; set; }

    public int? ChatTypeId { get; set; } = null;

    public Guid? OwnerId { get; set; } = null;

    public string? ChatName { get; set; } = null;

    public bool? IsPublic { get; set; } = null;

    public string? UniqueName { get; set; } = null;
}
