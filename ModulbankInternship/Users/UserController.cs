using System.Web.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModulbankInternship.Account;
using ModulbankInternship.Auth;
using ModulbankInternship.Auth.Exceptions;
using ModulbankInternship.Users.Requests;

namespace ModulbankInternship.Users;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("user")]
public class UserController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Получить список кошельков пользователя
    /// </summary>
    /// <param name="id">Идентификатор пользователя</param>
    /// <returns>Список кошельков пользователя</returns>
    /// <response code="200">Успешно. Возвращает список кошельков</response>
    /// <response code="404">Пользователь не найден</response>
    [Microsoft.AspNetCore.Mvc.HttpGet]
    [Microsoft.AspNetCore.Mvc.Route("{id:guid}/Accounts")]
    public async Task<AccountModel[]> GetUserAccounts([FromUri] Guid id)
    {
        if (!Request.Cookies.TryGetValue(CookieConstants.UserId, out var executorId))
        {
            throw new UnauthorizedException();
        }
        var Accounts = await mediator.Send(new GetUserAccountsQuery(id, Guid.Parse(executorId)));
        return Accounts;
    }
}