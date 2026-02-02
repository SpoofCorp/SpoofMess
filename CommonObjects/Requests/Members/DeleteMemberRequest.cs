namespace CommonObjects.Requests.Members;

public class DeleteMemberRequest
{
    public Guid MemberId { get; set; }

    public Guid ChatId { get; set; }
}
