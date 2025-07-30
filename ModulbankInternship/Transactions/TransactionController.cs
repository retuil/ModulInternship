using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModulbankInternship.Auth;
using ModulbankInternship.Auth.Exceptions;
using ModulbankInternship.Transactions.DTO;
using ModulbankInternship.Transactions.Requests;

namespace ModulbankInternship.Transactions;

[ApiController]
[Route("transactions")]
public class TransactionController(IMediator _mediator)
    : ControllerBase
{
    /// <summary>
    /// Создать отдельную транзакцию
    /// </summary>
    /// <param name="request">Данные новой транзакции</param>
    /// <returns>Return 200. Успешное создание транзакции</returns>
    /// <response code="200">Успешно. Транзакция создана</response>
    /// <response code="403">Отсутствуют права на создание этой транзакции</response>
    /// <response code="404">Используемый счет отсутствует или закрыт</response>
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> MakeTransaction([FromBody] NewTransactionRequest request)
    {
        if (!Request.Cookies.TryGetValue(CookieConstants.UserId, out var executorId))
        {
            throw new UnauthorizedException();
        }
        var transactionId = await _mediator.Send(new MakeTransactionCommand(request, Guid.Parse(executorId)));
        return Ok($"Транзакция id: {transactionId} завершилась успешно");
    }
}