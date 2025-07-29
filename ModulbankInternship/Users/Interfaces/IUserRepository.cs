using ModulbankInternship.Infrastructure;
using ModulbankInternship.Users.Models;

namespace ModulbankInternship.Users.Interfaces;

public interface IUserRepository : IRepository<UserModel>
{
    public UserModel? GetByPhoneAndPassword(string phoneNumber, long passwordHash);
}