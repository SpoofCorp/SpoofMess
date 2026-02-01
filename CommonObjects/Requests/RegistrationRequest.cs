namespace CommonObjects.Requests;

public class RegistrationRequest
{
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Name { get; set; } = null!;
}
