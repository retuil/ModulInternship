using ModulbankInternship.Auth.Exceptions;
using ModulbankInternship.Auth.Requests;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Users.Interfaces;
using ModulbankInternship.Users.Models;

namespace ModulbankInternship.Auth.Handlers;

public class LoginUserHandler(IUserRepository _userRepository)
    : ICommandHandler<LoginUserCommand, UserModel>
{
    public Task<UserModel> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = _userRepository.GetByPhoneAndPassword(request.AuthContract.PhoneNumber, request.AuthContract.PasswordHash);
        if (user is null)
        {
            throw new UnauthorizedException();
        }
        return Task.FromResult(user);
    }
}