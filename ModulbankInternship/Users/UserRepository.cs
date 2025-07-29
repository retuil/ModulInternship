using ModulbankInternship.Infrastructure;
using ModulbankInternship.Users.Interfaces;
using ModulbankInternship.Users.Models;

namespace ModulbankInternship.Users;

public class UserRepository: BaseRepository<UserModel>, IUserRepository
{
    public UserModel? GetByPhoneAndPassword(string phoneNumber, long passwordHash)
    {
        return dataBase.FirstOrDefault(u => u.PhoneNumber == phoneNumber && u.PasswordHash == passwordHash);
    }
}