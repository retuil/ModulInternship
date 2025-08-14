using ModulbankInternship.Infrastructure.Interfaces;
using ModulbankInternship.Users.Requests;

namespace ModulbankInternship.Infrastructure.Handlers;

public class GetRoleFromUserDataHandler: ICommandHandler<GetRoleFromUserDataCommand, string?>
{
    public Task<string?> Handle(GetRoleFromUserDataCommand request, CancellationToken cancellationToken)
    {
        var realmAccess = request.UserData.FindFirst("realm_access")?.Value;
        if (!string.IsNullOrEmpty(realmAccess))
        {
            var parsed = System.Text.Json.JsonDocument.Parse(realmAccess);
            if (parsed.RootElement.TryGetProperty("roles", out var roles))
            {
                return Task.FromResult(roles.EnumerateArray().FirstOrDefault().ToString());
            }
        }
        return Task.FromResult(default(string));
    }
}