namespace CommonObjects.Requests.Members;

public class AddMemberRequest
{
    public Guid MemberId { get; set; }

    public Guid ChatId { get; set; }

    public int RoleId { get; set; }
}
