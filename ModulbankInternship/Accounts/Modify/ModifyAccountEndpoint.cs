using System.Security.Claims;
using System.Web.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModulbankInternship.Accounts.DTO;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.DTO;
using ModulbankInternship.Users.Requests;

namespace ModulbankInternship.Accounts.Modify;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("Accounts")]
public class ModifyAccountEndpoint(IMediator mediator): ControllerBase
{
    /// <summary>
    /// Изменить параметры счета
    /// </summary>
    /// <param name="id">Id счета</param>
    /// <param name="request">Новые данные для счета</param>
    /// <returns>Список измененных полей с новыми данными в них</returns>
    /// <response code="200">Успешно. Изменения внесены</response>
    /// <response code="403">Отсутствуют права на модификацию счета</response>
    /// <response code="404">Счет отсутствует или закрыт</response>
    [Microsoft.AspNetCore.Authorization.Authorize]
    [Microsoft.AspNetCore.Mvc.HttpPatch]
    [Microsoft.AspNetCore.Mvc.Route("{id:guid}")]
    public async Task<MbResult<string>> ModifyAccountParameters([FromUri] Guid id, [System.Web.Http.FromBody] ModifyAccountRequest request)
    {
        var executor = new ExecutorData()
        {
            UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value),
            Role = await mediator.Send(new GetRoleFromUserDataCommand(User))
        };

        var modifiedParameters =
            await mediator.Send(new ModifyAccountParametersCommand(id, request, executor));
        var changesRecord = string.Join(", ", modifiedParameters.Select(x => $"{x.Key}: {x.Value}"));
        return MbResult.Success($"У счета id: {id} были изменены следующие параметры {changesRecord}");
    }
}