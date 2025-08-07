using System.Security.Claims;
using System.Web.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModulbankInternship.Accounts.DTO;
using ModulbankInternship.Accounts.Models;
using ModulbankInternship.Accounts.Requests;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.DTO;
using Guid = System.Guid;

namespace ModulbankInternship.Accounts;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("Accounts")]
public class AccountsController(IMediator mediator): ControllerBase
{
    /// <summary>
    /// Получить данные счета по id
    /// </summary>
    /// <param name="id">Id счета</param>
    /// <returns>AccountModel: данные счета</returns>
    /// <response code="200">Успешно. Данные получены</response>
    /// <response code="403">Отсутствуют права на просмотр счета</response>
    /// <response code="404">Счет отсутствует или закрыт</response>
    [Authorize]
    [Microsoft.AspNetCore.Mvc.HttpGet]
    [Microsoft.AspNetCore.Mvc.Route("{id:guid}")]
    public async Task<MbResult<AccountModel>> GetAccountById([FromUri] Guid id)
    {
        var executor = new ExecutorData()
        {
            UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
            Role = User.FindFirst(ClaimTypes.Role)?.Value
        };
        
        var account = await mediator.Send(new GetAccountByIdQuery(id, executor));
        return MbResult.Success(account);
    }
    
    /// <summary>
    /// Создать новый счет пользователю
    /// </summary>
    /// <returns>Id созданного счета<see cref="MbResult{T}"/></returns>
    /// <response code="200">Успешно. Данные получены</response>
    /// <response code="403">У текущего аккаунта нет возможности создавать счет</response>
    /// <response code="404">Пользователь не найден</response>
    [Authorize]
    [Microsoft.AspNetCore.Mvc.HttpPost]
    [Microsoft.AspNetCore.Mvc.Route("new")]
    public async Task<MbResult<Guid>> CreateAccount([System.Web.Http.FromBody] NewAccountForAnyUserRequest request)
    {
        var executor = new ExecutorData()
        {
            UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
            Role = User.FindFirst(ClaimTypes.Role)?.Value
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
    [Microsoft.AspNetCore.Mvc.HttpPost]
    [Microsoft.AspNetCore.Mvc.Route("newForMe")]
    public async Task<MbResult<Guid>> CreateAccountForCurrentUser([System.Web.Http.FromBody] NewAccountForCurrentUserRequest request)
    {
        var executor = new ExecutorData()
        {
            UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
            Role = User.FindFirst(ClaimTypes.Role)?.Value
        };

        var forAny = new NewAccountForAnyUserRequest()
        {
            AccountType = request.AccountType, InterestRate = request.InterestRate, Currency = request.Currency,
            OwnerId = executor.UserId
        };
        var newAccountId = await mediator.Send(new CreateAccountCommand(forAny, executor));
        return MbResult.Success(newAccountId);
    }

    /// <summary>
    /// Изменить параметры счета
    /// </summary>
    /// <param name="id">Id счета</param>
    /// <param name="request">Новые данные для счета</param>
    /// <returns>Список измененных полей с новыми данными в них</returns>
    /// <response code="200">Успешно. Изменения внесены</response>
    /// <response code="403">Отсутствуют права на модификацию счета</response>
    /// <response code="404">Счет отсутствует или закрыт</response>
    [Authorize]
    [Microsoft.AspNetCore.Mvc.HttpPatch]
    [Microsoft.AspNetCore.Mvc.Route("{id:guid}")]
    public async Task<MbResult<string>> ModifyAccountParameters([FromUri] Guid id, [System.Web.Http.FromBody] ModifyAccountRequest request)
    {
        var executor = new ExecutorData()
        {
            UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
            Role = User.FindFirst(ClaimTypes.Role)?.Value
        };

        var modifiedParameters =
            await mediator.Send(new ModifyAccountParametersCommand(id, request, executor));
        var changesRecord = string.Join(", ", modifiedParameters.Select(x => $"{x.Key}: {x.Value}"));
        return MbResult.Success($"У счета id: {id} были изменены следующие параметры {changesRecord}");
    }

    /// <summary>
    /// Закрыть счет
    /// </summary>
    /// <param name="id">Id счета</param>
    /// <returns>Return 200. Счет успешно закрыт</returns>
    /// <response code="200">Успешно. Счет закрыт</response>
    /// <response code="403">Отсутствуют права на закрытие счета</response>
    /// <response code="404">Счет отсутствует или уже закрыт</response>
    [Authorize]
    [Microsoft.AspNetCore.Mvc.HttpPost]
    [Microsoft.AspNetCore.Mvc.Route("{id:guid}")]
    public async Task<MbResult<string>> CloseAccount([FromUri] Guid id)
    {
        var executor = new ExecutorData()
        {
            UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
            Role = User.FindFirst(ClaimTypes.Role)?.Value
        };

        await mediator.Send(new CloseAccountCommand(id, executor));
        return MbResult.Success($"Закрытие счета id:{id} успешно завершено");
    }

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
            UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
            Role = User.FindFirst(ClaimTypes.Role)?.Value
        };

        var response = await mediator.Send(new GetAccountStatementQuery(id, startDate, finishDate, executor));
        return MbResult.Success(response);
    }

    /// <summary>
    /// Создать перевод с одного счета на другой
    /// </summary>
    /// <param name="request">Данные перевода</param>
    /// <returns>Return 200. Успешный перевод</returns>
    /// <response code="200">Успешно</response>
    /// <response code="403">Отсутствуют права на создание перевода между данными счетами</response>
    /// <response code="404">Один из счетов отсутствует или закрыт</response>
    [Authorize]
    [Microsoft.AspNetCore.Mvc.HttpPost]
    [Microsoft.AspNetCore.Mvc.Route("make_transfer")]
    public async Task<MbResult<string>> MakeTransfer([System.Web.Http.FromBody] TransferRequest request)
    {
        var executor = new ExecutorData()
        {
            UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
            Role = User.FindFirst(ClaimTypes.Role)?.Value
        };

        await mediator.Send(new MakeTransferCommand(request, executor));
        return MbResult.Success(
            $"Перевод со счета id: {request.AccountId} на счет id: {request.CounterpartyAccountId} на сумму {request.Amount} проведет успешно");
    }
}