namespace CommonObjects.Requests.Changes;

public class ChangeChatSettingsRequest
{
    public Guid Id { get; set; }

    public int? ChatTypeId { get; set; }

    public Guid? OwnerId { get; set; }

    public string? ChatName { get; set; }

    public bool? IsPublic { get; set; }

    public string? UniqueName { get; set; }
}
