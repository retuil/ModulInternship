using Microsoft.AspNetCore.Mvc;
using ModulbankInternship.Auth;
using ModulbankInternship.Auth.Exceptions;
using ModulbankInternship.Transactions.DTO;

namespace ModulbankInternship.Transactions;

[ApiController]
[Route("transactions")]
public class TransactionController: ControllerBase
{
    [HttpPost]
    [Route("")]
    public IActionResult MakeTransaction([FromBody] TransactionRequest request)
    {
        if (!Request.Cookies.TryGetValue(CookieConstants.UserId, out var executorId))
        {
            throw new UnauthorizedException();
        }
        // TODO: DI
        var transactionId = TransactionRequestHendler.MakeTransaction(request, executorId);
        return Ok($"Транзакция id: {transactionId} завершилась успешно");
    }
}