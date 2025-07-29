using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModulbankInternship.Auth;
using ModulbankInternship.Auth.Exceptions;
using ModulbankInternship.Transactions.DTO;
using ModulbankInternship.Transactions.Requests;

namespace ModulbankInternship.Transactions;

[ApiController]
[Route("transactions")]
public class TransactionController(IMediator _mediator,TransactionRequestHandler transactionRequestHandler)
    : ControllerBase
{
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