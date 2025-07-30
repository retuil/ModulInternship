using ModulbankInternship.Users.Enums;

namespace ModulbankInternship.Auth;

public class AuthContract
{
    public string PhoneNumber { get; set; }
    public long PasswordHash { get; set; }
}