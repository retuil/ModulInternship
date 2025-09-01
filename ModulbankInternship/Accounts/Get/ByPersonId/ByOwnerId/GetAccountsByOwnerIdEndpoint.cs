using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModulbankInternship.Accounts.Domain.Models;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.DTO;
using ModulbankInternship.Users.Requests;

namespace ModulbankInternship.Accounts.Get.ByPersonId.ByOwnerId;

[ApiController]
[Route("user")]
public class GetAccountsByOwnerIdEndpoint(IMediator mediator): ControllerBase
{
    /// <summary>
    /// Получить список кошельков текущего пользователя
    /// </summary>
    /// <returns>Список кошельков пользователя</returns>
    /// <response code="200">Успешно. Возвращает список кошельков</response>
    /// <response code="404">Пользователь не найден</response>
    [Authorize]
    [HttpGet]
    [Route("/Accounts")]
    public async Task<MbResult<AccountModel[]>> GetCurrentUserAccounts()
    {
        
        
        var executor = new ExecutorData()
        {
            UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!),
            Role = await mediator.Send(new GetRoleFromUserDataCommand(User))
        };
        var accounts = await mediator.Send(new GetOwnerAccountsQuery(executor.UserId, executor));
        return MbResult.Success(accounts);
    }
}