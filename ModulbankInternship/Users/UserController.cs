using System.Web.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModulbankInternship.Account;
using ModulbankInternship.Auth;
using ModulbankInternship.Auth.Exceptions;
using ModulbankInternship.Users.Requests;

namespace ModulbankInternship.Users;

[ApiController]
[Route("user")]
public class UserController(IMediator mediator) : ControllerBase
{
    [Microsoft.AspNetCore.Mvc.HttpGet]
    [Route("{id:guid}/wallets")]
    public async Task<WalletModel[]> GetUserWallets([FromUri] Guid id)
    {
        if (!Request.Cookies.TryGetValue(CookieConstants.UserId, out var executorId))
        {
            throw new UnauthorizedException();
        }
        var wallets = await mediator.Send(new GetUserWalletsQuery(id, Guid.Parse(executorId)));
        return wallets;
    }
}