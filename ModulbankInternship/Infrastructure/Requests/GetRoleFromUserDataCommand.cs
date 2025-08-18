using System.Security.Claims;
using ModulbankInternship.Infrastructure.Interfaces;

namespace ModulbankInternship.Users.Requests;

public record GetRoleFromUserDataCommand(ClaimsPrincipal UserData)
    : ICommand<string?>;