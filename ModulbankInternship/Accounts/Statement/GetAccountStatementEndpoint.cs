using System.Security.Claims;
using System.Web.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.DTO;
using ModulbankInternship.Users.Requests;

namespace ModulbankInternship.Accounts.Statement;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("Accounts")]
public class GetAccountStatementEndpoint(IMediator mediator): ControllerBase
{
    /// <summary>
    /// Собрать выписку по счету
    /// </summary>
    /// <param name="id">Id счета</param>
    /// <param name="startDate">Дата начала выписки (включительно)</param>
    /// <param name="finishDate">Дата окончания выписки (включительно)</param>
    /// <returns>Выписка по счету за указанные период</returns>
    /// <response code="200">Успешно</response>
    /// <response code="403">Отсутствуют права на заказ выписки по счету</response>
    /// <response code="404">Счет отсутствует или закрыт</response>
    [Authorize]
    [Microsoft.AspNetCore.Mvc.HttpGet]
    [Microsoft.AspNetCore.Mvc.Route("{id:guid}/statement")]
    public async Task<MbResult<AccountStatementResponse>> GetAccountStatement([FromUri] Guid id, [FromUri] DateTime startDate,
        [FromUri] DateTime finishDate)
    {
        var executor = new ExecutorData()
        {
            UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value),
            Role = await mediator.Send(new GetRoleFromUserDataCommand(User))
        };

        var response = await mediator.Send(new GetAccountStatementQuery(id, startDate, finishDate, executor));
        return MbResult.Success(response);
    }
}