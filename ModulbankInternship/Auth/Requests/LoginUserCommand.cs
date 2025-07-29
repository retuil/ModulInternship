using ModulbankInternship.Infrastructure;
using ModulbankInternship.Users.Models;

namespace ModulbankInternship.Auth.Requests;

public record LoginUserCommand(AuthContract AuthContract): ICommand<UserModel>;