using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.Interfaces;
using ModulbankInternship.Users.Enums;

namespace ModulbankInternship.Users.Models;

public class UserModel: IModel
{
    public Guid Id { get; set; }
    public string PhoneNumber { get; set; }
    public long PasswordHash { get; set; }
    public EUserRole Role { get; set; }
    public bool IsExist { get; set; } = true;
}