using ModulbankInternship.Infrastructure;
using ModulbankInternship.Users.Models;

namespace ModulbankInternship.Auth.Requests;

public record RegisterUserCommand(RegisterContract Contract): ICommand<UserModel>;