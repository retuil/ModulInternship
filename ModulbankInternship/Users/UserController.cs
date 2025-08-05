using System.Security.Claims;
using System.Web.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModulbankInternship.Account;
using ModulbankInternship.Auth;
using ModulbankInternship.Auth.Exceptions;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Users.DTO;
using ModulbankInternship.Users.Models;
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
    [Authorize]
    [Microsoft.AspNetCore.Mvc.HttpGet]
    [Microsoft.AspNetCore.Mvc.Route("{id:guid}/Accounts")]
    public async Task<MbResult<AccountModel[]>> GetUserAccounts([FromUri] Guid id)
    {
        var executor = new ExecutorData()
        {
            UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
            Role = User.FindFirst(ClaimTypes.Role)?.Value
        };
        var accounts = await mediator.Send(new GetUserAccountsQuery(id, executor));
        return MbResult.Success(accounts);
    }
    
    /// <summary>
    /// Получить список кошельков текущего пользователя
    /// </summary>
    /// <returns>Список кошельков пользователя</returns>
    /// <response code="200">Успешно. Возвращает список кошельков</response>
    /// <response code="404">Пользователь не найден</response>
    [Authorize]
    [Microsoft.AspNetCore.Mvc.HttpGet]
    [Microsoft.AspNetCore.Mvc.Route("/Accounts")]
    public async Task<MbResult<AccountModel[]>> GetCurrentUserAccounts()
    {
        var executor = new ExecutorData()
        {
            UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
            Role = User.FindFirst(ClaimTypes.Role)?.Value
        };
        var accounts = await mediator.Send(new GetUserAccountsQuery(executor.UserId, executor));
        return MbResult.Success(accounts);
    }
}