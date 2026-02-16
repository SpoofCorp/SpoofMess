namespace CommunicationLibrary.Communication;

public class CreateUser
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = null!;
    public string Login { get; set; } = null!;
    public CreateUser() { }
    public CreateUser(Guid userId, string name, string login)
    {
        UserId = userId;
        Name = name;
        Login = login;
    }
}
