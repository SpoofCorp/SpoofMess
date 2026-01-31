namespace CommunicationLibrary.Communication;

public class CreateUser
{
    public Guid UserId { get; set; }
    public CreateUser() { }
    public CreateUser(Guid userId)
    {
        UserId = userId;
    }
}
