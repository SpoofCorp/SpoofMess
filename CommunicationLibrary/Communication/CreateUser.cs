namespace CommunicationLibrary.Communication;

public class CreateUser
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = null!;
    public CreateUser() { }
    public CreateUser(Guid userId, string name)
    {
        UserId = userId;
        Name = name;
    }
}
