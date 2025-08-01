using ModulbankInternship.Users.Enums;

namespace ModulbankInternship.Auth;

public class AuthContract
{
    /// <summary>Номер телефона, к которому привязан аккаунт</summary>
    public string PhoneNumber { get; set; }
    
    /// <summary>Хэш от пароля, введенного пользователем</summary>
    public long PasswordHash { get; set; }
}