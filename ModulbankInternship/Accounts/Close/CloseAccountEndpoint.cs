using System.Security.Claims;
using System.Web.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.DTO;
using ModulbankInternship.Users.Requests;

namespace ModulbankInternship.Accounts.Close;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("Accounts")]
public class CloseAccountEndpoint(IMediator mediator, ILogger<CloseAccountEndpoint> logger): ControllerBase
{
    /// <summary>
    /// Закрыть счет
    /// </summary>
    /// <param name="id">Id счета</param>
    /// <returns>Return 200. Счет успешно закрыт</returns>
    /// <response code="200">Успешно. Счет закрыт</response>
    /// <response code="403">Отсутствуют права на закрытие счета</response>
    /// <response code="404">Счет отсутствует или уже закрыт</response>
    [Microsoft.AspNetCore.Authorization.Authorize]
    [Microsoft.AspNetCore.Mvc.HttpPost]
    [Microsoft.AspNetCore.Mvc.Route("{id:guid}")]
    public async Task<MbResult<string>> CloseAccount([FromUri] Guid id)
    {
        var executor = new ExecutorData()
        {
            UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value),
            Role = await mediator.Send(new GetRoleFromUserDataCommand(User))
        };

        await mediator.Send(new CloseAccountCommand(id, executor));
        return MbResult.Success($"Закрытие счета id:{id} успешно завершено");
    }
}