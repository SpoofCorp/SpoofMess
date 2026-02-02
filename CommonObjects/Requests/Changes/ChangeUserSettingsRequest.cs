namespace CommonObjects.Requests.Changes;

public class ChangeUserSettingsRequest
{
    public string? Name { get; set; }

    public int? MonthsBeforeDelete { get; set; }

    public bool? SearchMe { get; set; }

    public bool? ShowMe { get; set; }

    public bool? ForwardMessage { get; set; }

    public bool? InviteMe { get; set; }

}
