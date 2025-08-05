using System.Security.Claims;
using System.Web.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModulbankInternship.Auth;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.DTO;
using ModulbankInternship.Transactions.DTO;
using ModulbankInternship.Transactions.Requests;

namespace ModulbankInternship.Transactions;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("transactions")]
public class TransactionController(IMediator _mediator)
    : ControllerBase
{
    /// <summary>
    /// Создать отдельную транзакцию
    /// </summary>
    /// <param name="request">Данные новой транзакции</param>
    /// <returns>Id проведенной транзакции</returns>
    /// <response code="200">Успешно. Транзакция создана</response>
    /// <response code="403">Отсутствуют права на создание этой транзакции</response>
    /// <response code="404">Используемый счет отсутствует или закрыт</response>
    [Authorize]
    [Microsoft.AspNetCore.Mvc.HttpPost]
    [Microsoft.AspNetCore.Mvc.Route("")]
    public async Task<MbResult<Guid>> MakeTransaction([Microsoft.AspNetCore.Mvc.FromBody] NewTransactionRequest request)
    {
        var executor = new ExecutorData()
        {
            UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
            Role = User.FindFirst(ClaimTypes.Role)?.Value
        };
        var transactionId = await _mediator.Send(new MakeTransactionCommand(request, executor));
        return MbResult.Success(transactionId);
    }
}