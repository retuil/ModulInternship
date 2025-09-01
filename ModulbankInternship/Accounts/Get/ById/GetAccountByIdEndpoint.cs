using System.Security.Claims;
using System.Web.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModulbankInternship.Accounts.Domain.Models;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.DTO;
using ModulbankInternship.Users.Requests;

namespace ModulbankInternship.Accounts.Get;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("Accounts")]
public class GetAccountByIdEndpoint(IMediator mediator): ControllerBase
{
    /// <summary>
    /// Получить данные счета по id
    /// </summary>
    /// <param name="id">Id счета</param>
    /// <returns>AccountModel: данные счета</returns>
    /// <response code="200">Успешно. Данные получены</response>
    /// <response code="403">Отсутствуют права на просмотр счета</response>
    /// <response code="404">Счет отсутствует или закрыт</response>
    [Authorize]
    [Microsoft.AspNetCore.Mvc.HttpGet]
    [Microsoft.AspNetCore.Mvc.Route("{id:guid}")]
    public async Task<MbResult<AccountModel>> GetAccountById([FromUri] Guid id)
    {
        var executor = new ExecutorData()
        {
            UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value),
            Role = await mediator.Send(new GetRoleFromUserDataCommand(User))
        };
        
        var account = await mediator.Send(new GetAccountByIdQuery(id, executor));
        return MbResult.Success(account);
    }
}