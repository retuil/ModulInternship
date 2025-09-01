using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModulbankInternship.Accounts.Create.ForAnyUser;
using ModulbankInternship.Accounts.Domain.Enums;
using ModulbankInternship.Accounts.DTO;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.DTO;
using ModulbankInternship.Users.Requests;

namespace ModulbankInternship.Accounts.Create;


[ApiController]
[Route("Accounts")]
public class CreateAccountEndpoint(IMediator mediator, ILogger<CreateAccountEndpoint> logger): ControllerBase
{
    /// <summary>
    /// Создать новый счет пользователю
    /// </summary>
    /// <returns>Id созданного счета<see cref="MbResult"/></returns>
    /// <response code="200">Успешно. Данные получены</response>
    /// <response code="403">У текущего аккаунта нет возможности создавать счет</response>
    /// <response code="404">Пользователь не найден</response>
    [Authorize]
    [HttpPost]
    [Route("new")]
    public async Task<MbResult<Guid>> CreateAccount([System.Web.Http.FromBody] NewAccountForAnyUserRequest request)
    {
        var executor = new ExecutorData()
        {
            UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value),
            Role = await mediator.Send(new GetRoleFromUserDataCommand(User))
        };
        
        var newAccountId = await mediator.Send(new CreateAccountCommand(request, executor));
        return MbResult.Success(newAccountId);
    }
    
    /// <summary>
    /// Создать новый счет текущему пользователю
    /// </summary>
    /// <returns>Id созданного счета<see cref="MbResult{T}"/></returns>
    /// <response code="200">Успешно. Данные получены</response>
    /// <response code="403">У текущего аккаунта нет возможности создавать счет</response>
    /// <response code="404">Пользователь не найден</response>
    [Authorize]
    [HttpPost]
    [Route("newForMe")]
    public async Task<MbResult<Guid>> CreateAccountForCurrentUser([System.Web.Http.FromBody] NewAccountForCurrentUserRequest request)
    {
        var executor = new ExecutorData()
        {
            UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value),
            Role = await mediator.Send(new GetRoleFromUserDataCommand(User))
        };

        logger.LogInformation("Запрос с AccountType = {accountType}, в форматированном виде: {EAccountType}", request.AccountType, ((EAccountType)request.AccountType).ToString());
        var forAny = new NewAccountForAnyUserRequest()
        {
            AccountType = request.AccountType, InterestRate = request.InterestRate, Currency = request.Currency,
            OwnerId = executor.UserId
        };
        var newAccountId = await mediator.Send(new CreateAccountCommand(forAny, executor));
        return MbResult.Success(newAccountId);
    }
}