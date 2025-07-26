using ModulbankInternship.Users.Enums;

namespace ModulbankInternship.Auth;

public class RegisterContract: AuthContract
{
    public EUserRole Role { get; set; }
}