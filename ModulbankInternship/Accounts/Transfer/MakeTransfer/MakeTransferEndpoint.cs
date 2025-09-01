using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.DTO;
using ModulbankInternship.Users.Requests;

namespace ModulbankInternship.Accounts.Transfer.MakeTransfer;

[ApiController]
[Route("Accounts")]
public class MakeTransferEndpoint(IMediator mediator): ControllerBase
{
    /// <summary>
    /// Создать перевод с одного счета на другой
    /// </summary>
    /// <param name="request">Данные перевода</param>
    /// <returns>Return 200. Успешный перевод</returns>
    /// <response code="200">Успешно</response>
    /// <response code="403">Отсутствуют права на создание перевода между данными счетами</response>
    /// <response code="404">Один из счетов отсутствует или закрыт</response>
    [Authorize]
    [HttpPost]
    [Route("make_transfer")]
    public async Task<MbResult<string>> MakeTransfer([System.Web.Http.FromBody] TransferRequest request)
    {
        var executor = new ExecutorData()
        {
            UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value),
            Role = await mediator.Send(new GetRoleFromUserDataCommand(User))
        };

        await mediator.Send(new MakeTransferCommand(request, executor));
        return MbResult.Success(
            $"Перевод со счета id: {request.AccountId} на счет id: {request.CounterpartyAccountId} на сумму {request.Amount} проведет успешно");
    }
}