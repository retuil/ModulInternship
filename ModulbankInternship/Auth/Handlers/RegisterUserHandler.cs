using ModulbankInternship.Auth.Requests;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Users.Interfaces;
using ModulbankInternship.Users.Models;

namespace ModulbankInternship.Auth.Handlers;

public class RegisterUserHandler(IUserRepository _userRepository)
    : ICommandHandler<RegisterUserCommand, UserModel>
{
    public Task<UserModel> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var contract = request.Contract;
        var id = _userRepository.Add(new UserModel()
        {
            PasswordHash = contract.PasswordHash,
            PhoneNumber = contract.PhoneNumber,
            Role = contract.Role
        });
        var user = _userRepository.Get(id);
        return Task.FromResult(user);
    }
}