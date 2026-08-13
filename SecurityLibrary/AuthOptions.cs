using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace SecurityLibrary;
public class AuthOptions
{
    public const string ISSUER = "SpoofMessenger";

    public const string AUDIENCE = "RealClient";

    public const int LIFETIME = 1;

    const string KEY = "Super-Puper_Mega-Duper-Very-Hard-EasyButFalsePasswordOyBililyaKey";

    public static SymmetricSecurityKey GetSecurityKey() =>
        new(Encoding.UTF8.GetBytes(KEY));
}