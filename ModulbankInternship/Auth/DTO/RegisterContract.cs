using ModulbankInternship.Users.Enums;

namespace ModulbankInternship.Auth;

public class RegisterContract: AuthContract
{
    /// <summary>Какая роль будет у нового аккаунта</summary>
    public EUserRole Role { get; set; }
}