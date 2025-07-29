using ModulbankInternship.Users.Enums;

namespace ModulbankInternship.Auth;

public class AuthContract
{
    public Guid Id { get; set; }
    public string PhoneNumber { get; set; }
    public long PasswordHash { get; set; }
    public EUserRole Role { get; set; }
}